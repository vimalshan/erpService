using MediatR;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using SwipeTransactionService.Application.Features.SwipeTransactions.Commands;

namespace SwipeTransactionService.Infrastructure.Messaging;

public sealed class SwipeTransactionConsumer : IDisposable
{
    private const string QueueName = "swipe.transactions.incoming";
    private const string ExchangeName = "canteen.exchange";

    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly IMediator _mediator;
    private readonly ILogger<SwipeTransactionConsumer> _logger;

    public SwipeTransactionConsumer(
        IConnection connection,
        IChannel channel,
        IMediator mediator,
        ILogger<SwipeTransactionConsumer> logger)
    {
        _connection = connection;
        _channel = channel;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task StartConsumingAsync(CancellationToken ct)
    {
        await _channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Direct, durable: true, cancellationToken: ct);
        await _channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: ct);
        await _channel.QueueBindAsync(QueueName, ExchangeName, "swipe.upload", cancellationToken: ct);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                var command = JsonSerializer.Deserialize<RecordSwipeUploadCommand>(json);
                if (command is not null)
                    await _mediator.Send(command, ct);

                await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process swipe transaction message.");
                await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true, ct);
            }
        };

        await _channel.BasicConsumeAsync(QueueName, autoAck: false, consumer: consumer, cancellationToken: ct);
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}
