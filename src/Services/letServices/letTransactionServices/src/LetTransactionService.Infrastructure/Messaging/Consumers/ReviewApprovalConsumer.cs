using System.Text;
using System.Text.Json;
using LetTransactionService.Application.Commands.ApproveReview;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LetTransactionService.Infrastructure.Messaging.Consumers;

public sealed class ReviewApprovalConsumer(
    IOptions<RabbitMqOptions> options,
    IServiceScopeFactory scopeFactory,
    ILogger<ReviewApprovalConsumer> logger)
    : BackgroundService
{
    private const string QueueName = "let.review.approval.queue";
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly RabbitMqOptions _opts = options.Value;

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _opts.Host,
                Port = _opts.Port,
                UserName = _opts.UserName,
                Password = _opts.Password,
                VirtualHost = _opts.VirtualHost
            };

            _connection = await factory.CreateConnectionAsync(ct);
            _channel = await _connection.CreateChannelAsync(cancellationToken: ct);

            await _channel.ExchangeDeclareAsync(_opts.Exchange, ExchangeType.Topic, durable: true, cancellationToken: ct);
            await _channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: ct);
            await _channel.QueueBindAsync(QueueName, _opts.Exchange, "let.review.approval.#", cancellationToken: ct);
            await _channel.BasicQosAsync(0, 10, false, cancellationToken: ct);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                try
                {
                    var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var message = JsonSerializer.Deserialize<ApproveReviewCommand>(json);

                    if (message is not null)
                    {
                        using var scope = scopeFactory.CreateScope();
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        await mediator.Send(message, ct);
                    }

                    await _channel.BasicAckAsync(ea.DeliveryTag, false, cancellationToken: ct);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error processing review approval message");
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, true, cancellationToken: ct);
                }
            };

            await _channel.BasicConsumeAsync(QueueName, autoAck: false, consumer: consumer, cancellationToken: ct);

            while (!ct.IsCancellationRequested)
                await Task.Delay(1000, ct);
        }
        catch (OperationCanceledException) { /* graceful shutdown */ }
        catch (Exception ex)
        {
            logger.LogError(ex, "RabbitMQ review approval consumer failed to start");
        }
    }

    public override async Task StopAsync(CancellationToken ct)
    {
        if (_channel is not null) await _channel.CloseAsync(ct);
        if (_connection is not null) await _connection.CloseAsync(ct);
        await base.StopAsync(ct);
    }
}
