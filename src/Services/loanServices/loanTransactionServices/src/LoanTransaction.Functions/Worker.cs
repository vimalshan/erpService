using System.Text;
using System.Text.Json;
using LoanTransaction.Application.Queries;
using LoanTransaction.Infrastructure.Messaging;
using MediatR;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LoanTransaction.Functions;

public class WorkerSettings
{
    public int OverdueScanIntervalMinutes { get; set; } = 60;
    public string InboundQueue { get; set; } = "loan-transaction.application-approved";
}

// DTO for the inbound integration event from LoanApplication service
internal sealed class LoanApplicationApprovedMessage
{
    public Guid EventId { get; set; }
    public long LoanApplicationId { get; set; }
    public long ApprovedBy { get; set; }
    public DateTime ApprovedAt { get; set; }
    public string? Remarks { get; set; }
}

/// <summary>
/// Consumes "loan.application.approved" events from the LoanApplication service.
/// Logs the approval so the operations team can trigger manual disbursement if needed,
/// and publishes a downstream notification via the message bus.
/// </summary>
public sealed class LoanApplicationApprovedConsumer(
    IOptions<RabbitMQSettings> rabbitOptions,
    IOptions<WorkerSettings> workerOptions,
    ILogger<LoanApplicationApprovedConsumer> logger) : BackgroundService
{
    private readonly RabbitMQSettings _rabbit = rabbitOptions.Value;
    private readonly string _queueName = workerOptions.Value.InboundQueue;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("LoanApplicationApprovedConsumer starting. Queue: {Queue}", _queueName);

        IConnection? connection = null;
        IChannel? channel = null;

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _rabbit.HostName,
                    Port = _rabbit.Port,
                    UserName = _rabbit.UserName,
                    Password = _rabbit.Password
                };

                connection = await factory.CreateConnectionAsync(stoppingToken);
                channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

                const string inboundExchange = "loan.application.exchange";
                await channel.ExchangeDeclareAsync(inboundExchange, ExchangeType.Topic, durable: true,
                    cancellationToken: stoppingToken);
                await channel.QueueDeclareAsync(_queueName, durable: true, exclusive: false,
                    autoDelete: false, cancellationToken: stoppingToken);
                await channel.QueueBindAsync(_queueName, inboundExchange, "loan.application.approved",
                    cancellationToken: stoppingToken);

                await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 10, global: false,
                    cancellationToken: stoppingToken);

                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.ReceivedAsync += async (_, ea) =>
                {
                    try
                    {
                        var body = Encoding.UTF8.GetString(ea.Body.Span);
                        var evt = JsonSerializer.Deserialize<LoanApplicationApprovedMessage>(body,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                        if (evt is not null)
                        {
                            logger.LogInformation(
                                "Loan application {ApplicationId} approved by {ApprovedBy} at {ApprovedAt}. " +
                                "Remarks: {Remarks}. Ready for disbursement.",
                                evt.LoanApplicationId, evt.ApprovedBy, evt.ApprovedAt, evt.Remarks);
                        }

                        await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error processing LoanApplicationApproved event. Nacking message.");
                        await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);
                    }
                };

                await channel.BasicConsumeAsync(_queueName, autoAck: false, consumer: consumer,
                    cancellationToken: stoppingToken);

                logger.LogInformation("LoanApplicationApprovedConsumer is listening on queue {Queue}", _queueName);

                // Keep running until cancelled or connection drops
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex,
                    "RabbitMQ connection lost. Retrying in 10 seconds...");

                if (channel is not null) await channel.DisposeAsync();
                if (connection is not null) await connection.DisposeAsync();
                channel = null;
                connection = null;

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        if (channel is not null) await channel.DisposeAsync();
        if (connection is not null) await connection.DisposeAsync();

        logger.LogInformation("LoanApplicationApprovedConsumer stopped.");
    }
}

/// <summary>
/// Periodically scans for overdue loan installments and logs them for operations follow-up.
/// </summary>
public sealed class OverdueInstallmentScanner(
    IServiceProvider services,
    IOptions<WorkerSettings> workerOptions,
    ILogger<OverdueInstallmentScanner> logger) : BackgroundService
{
    private readonly TimeSpan _interval =
        TimeSpan.FromMinutes(workerOptions.Value.OverdueScanIntervalMinutes);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation(
            "OverdueInstallmentScanner starting. Interval: {Interval} minutes", _interval.TotalMinutes);

        // Initial delay to let the API warm up first
        await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ScanOverdueInstallmentsAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error during overdue installment scan.");
            }

            await Task.Delay(_interval, stoppingToken);
        }

        logger.LogInformation("OverdueInstallmentScanner stopped.");
    }

    private async Task ScanOverdueInstallmentsAsync(CancellationToken ct)
    {
        logger.LogInformation("Running overdue installment scan at {Time}", DateTimeOffset.UtcNow);

        await using var scope = services.CreateAsyncScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        // Get all active loans (paged) and check for pending overdue installments
        int page = 1;
        const int pageSize = 50;
        int totalOverdue = 0;

        while (true)
        {
            var pagedLoans = await mediator.Send(new GetAllLoansQuery(page, pageSize), ct);

            if (pagedLoans.Items is null || !pagedLoans.Items.Any())
                break;

            foreach (var loan in pagedLoans.Items.Where(l => l.IsActive))
            {
                var pending = await mediator.Send(new GetPendingInstallmentsQuery(loan.LoanNo), ct);
                var overdue = pending.Where(i => i.InstallmentDate < DateTime.UtcNow.Date).ToList();

                if (overdue.Count > 0)
                {
                    totalOverdue += overdue.Count;
                    logger.LogWarning(
                        "Loan {LoanNo} (Employee {EmpId}) has {Count} overdue installment(s). " +
                        "Oldest due: {OldestDue:yyyy-MM-dd}",
                        loan.LoanNo, loan.EmployeeId, overdue.Count,
                        overdue.Min(i => i.InstallmentDate));
                }
            }

            if (!pagedLoans.HasNextPage)
                break;

            page++;
        }

        logger.LogInformation(
            "Overdue installment scan complete. Total overdue installments found: {Total}", totalOverdue);
    }
}

