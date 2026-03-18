using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ReviewService.Infrastructure.Consumers;

/// <summary>
/// Background service that consumes feedback.submitted messages from RabbitMQ.
/// </summary>
public class FeedbackSubmittedConsumer : BackgroundService
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<FeedbackSubmittedConsumer> _logger;
    private const string Exchange = "review.exchange";
    private const string Queue = "feedback.submitted.queue";
    private const string RoutingKey = "feedback.submitted";

    public FeedbackSubmittedConsumer(
        IConnectionFactory connectionFactory,
        IServiceScopeFactory scopeFactory,
        ILogger<FeedbackSubmittedConsumer> logger)
    {
        _connectionFactory = connectionFactory;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var connection = await _connectionFactory.CreateConnectionAsync(stoppingToken);
            var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await channel.ExchangeDeclareAsync(Exchange, ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
            await channel.QueueDeclareAsync(Queue, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
            await channel.QueueBindAsync(Queue, Exchange, RoutingKey, cancellationToken: stoppingToken);
            await channel.BasicQosAsync(0, 10, false, stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    _logger.LogInformation("Received feedback.submitted message: {Message}", message);

                    // Parse and process message
                    var payload = JsonSerializer.Deserialize<FeedbackMessage>(message);
                    if (payload is not null)
                    {
                        using var scope = _scopeFactory.CreateScope();
                        _logger.LogInformation(
                            "Processing feedback for CourseId={CourseId}, UserId={UserId}",
                            payload.FdCrsId, payload.FdUsrId);
                    }

                    await channel.BasicAckAsync(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing feedback.submitted message");
                    await channel.BasicNackAsync(ea.DeliveryTag, false, requeue: false);
                }
            };

            await channel.BasicConsumeAsync(Queue, autoAck: false, consumer, stoppingToken);
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException) { /* shutting down */ }
        catch (Exception ex)
        {
            _logger.LogError(ex, "FeedbackSubmittedConsumer failed");
        }
    }

    private sealed record FeedbackMessage(long FdCrsId, string FdUsrId, DateTime FdRevDat);
}
