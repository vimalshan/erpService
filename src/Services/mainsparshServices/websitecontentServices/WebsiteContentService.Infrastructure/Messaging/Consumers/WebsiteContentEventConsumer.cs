namespace WebsiteContentService.Infrastructure.Messaging.Consumers;

using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class WebsiteContentEventConsumer : BackgroundService
{
    private readonly ILogger<WebsiteContentEventConsumer> _logger;
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly ConnectionFactory _connectionFactory;

    public WebsiteContentEventConsumer(ConnectionFactory connectionFactory, ILogger<WebsiteContentEventConsumer> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _connection = await _connectionFactory.CreateConnectionAsync(stoppingToken);
                _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

                await _channel.QueueDeclareAsync(
                    queue: "website-content-events",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    cancellationToken: stoppingToken);

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += async (_, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    _logger.LogInformation("Received message: {Message}", message);

                    await _channel.BasicAckAsync(ea.DeliveryTag, false);
                };

                await _channel.BasicConsumeAsync("website-content-events", false, consumer, stoppingToken);
                _logger.LogInformation("WebsiteContentEventConsumer started listening.");

                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "WebsiteContentEventConsumer could not connect to RabbitMQ. Retrying in 30 seconds...");
                try { await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken); }
                catch (OperationCanceledException) { break; }
            }
        }
    }
}
