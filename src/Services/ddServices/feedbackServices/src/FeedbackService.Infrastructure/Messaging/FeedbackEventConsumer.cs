namespace FeedbackService.Infrastructure.Messaging;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Hosting;
using System.Text;

/// <summary>
/// Background service for consuming feedback messages from RabbitMQ
/// </summary>
public class FeedbackEventConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IServiceProvider _serviceProvider;
    private IModel? _channel;
    private const string ExchangeName = "feedback.events";
    private const string QueueName = "feedback.feedback-processed";

    /// <summary>
    /// Initializes a new instance of the FeedbackEventConsumer class
    /// </summary>
    public FeedbackEventConsumer(IConnection connection, IServiceProvider serviceProvider)
    {
        _connection = connection;
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Executes the background service
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(
            exchange: ExchangeName,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false);

        _channel.QueueDeclare(
            queue: QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        _channel.QueueBind(
            queue: QueueName,
            exchange: ExchangeName,
            routingKey: "feedback.feedbacksubmittedevent");

        _channel.BasicQos(0, 1, false);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = System.Text.Encoding.UTF8.GetString(body);
                
                // Process the message
                await ProcessMessageAsync(message, stoppingToken);
                
                _channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                // Log error and nack message
                System.Diagnostics.Debug.WriteLine($"Error processing message: {ex.Message}");
                _channel.BasicNack(ea.DeliveryTag, false, true);
            }
        };

        _channel.BasicConsume(QueueName, false, consumer);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    /// <summary>
    /// Processes the consumed message
    /// </summary>
    private async Task ProcessMessageAsync(string message, CancellationToken cancellationToken)
    {
        // This is where you would process the feedback submitted event
        // For example, send notifications, update related services, etc.
        await Task.CompletedTask;
    }

    /// <summary>
    /// Disposes the consumer
    /// </summary>
    public override void Dispose()
    {
        _channel?.Dispose();
        base.Dispose();
    }
}
