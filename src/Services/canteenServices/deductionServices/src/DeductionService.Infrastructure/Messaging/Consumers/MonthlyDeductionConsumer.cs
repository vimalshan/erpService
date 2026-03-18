using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DeductionService.Infrastructure.Messaging.Consumers;

/// <summary>
/// Background service that consumes monthly deduction trigger events from RabbitMQ.
/// </summary>
public class MonthlyDeductionConsumer : BackgroundService
{
    private IConnection? _connection;
    private IChannel? _channel;
    private const string QueueName = "deduction.monthly.trigger";
    private readonly IConfiguration _configuration;
    private readonly ILogger<MonthlyDeductionConsumer> _logger;
    private readonly IServiceProvider _serviceProvider;

    public MonthlyDeductionConsumer(
        IConfiguration configuration,
        ILogger<MonthlyDeductionConsumer> logger,
        IServiceProvider serviceProvider)
    {
        _configuration = configuration;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public override async Task StartAsync(CancellationToken ct)
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:Host"] ?? "localhost",
                UserName = _configuration["RabbitMQ:Username"] ?? "guest",
                Password = _configuration["RabbitMQ:Password"] ?? "guest"
            };

            _connection = await factory.CreateConnectionAsync(ct);
            _channel = await _connection.CreateChannelAsync(cancellationToken: ct);
            await _channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: ct);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "[Consumer] Cannot connect to RabbitMQ — consumer will be inactive until broker is available.");
        }

        await base.StartAsync(ct);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_channel is null)
        {
            _logger.LogWarning("[Consumer] RabbitMQ channel unavailable — skipping message consumption.");
            return;
        }

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation("[Consumer] Received monthly deduction trigger: {Message}", message);

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<MediatR.IMediator>();

                var payload = JsonSerializer.Deserialize<MonthlyTriggerPayload>(message);
                if (payload != null)
                {
                    await mediator.Send(new Application.CQRS.Commands.ProcessMonthlyDeduction.ProcessMonthlyDeductionCommand(
                        payload.MonthYear, payload.ProcessedByUserId), stoppingToken);
                }

                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Consumer] Failed to process monthly deduction trigger.");
                await _channel.BasicNackAsync(ea.DeliveryTag, false, requeue: false);
            }
        };

        await _channel.BasicConsumeAsync(QueueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override async Task StopAsync(CancellationToken ct)
    {
        if (_channel != null) await _channel.DisposeAsync();
        if (_connection != null) await _connection.DisposeAsync();
        await base.StopAsync(ct);
    }

    private record MonthlyTriggerPayload(string MonthYear, long ProcessedByUserId);
}
