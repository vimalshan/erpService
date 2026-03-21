using System.Text;
using System.Text.Json;
using EnergyService.Domain.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EnergyService.Infrastructure.Messaging.Consumers;

public class ReadingRecordedConsumer : BackgroundService
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly ILogger<ReadingRecordedConsumer> _logger;
    private IConnection? _connection;
    private IChannel? _channel;

    private const string Exchange = "energy-exchange";
    private const string Queue = "energy-reading-recorded-queue";
    private const string RoutingKey = "reading.recorded";

    public ReadingRecordedConsumer(IConnectionFactory connectionFactory, ILogger<ReadingRecordedConsumer> logger)
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

                await _channel.ExchangeDeclareAsync(Exchange, ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
                await _channel.QueueDeclareAsync(Queue, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
                await _channel.QueueBindAsync(Queue, Exchange, RoutingKey, cancellationToken: stoppingToken);

                _logger.LogInformation("ReadingRecordedConsumer connected to RabbitMQ");

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += async (_, ea) =>
                {
                    try
                    {
                        var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                        var reading = JsonSerializer.Deserialize<ReadingRecordedEvent>(body);
                        _logger.LogInformation("Consumed ReadingRecorded: Process {ProcessId}, Usage {Usage}",
                            reading?.ProcessId, reading?.ActualUsage);

                        await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing ReadingRecorded message");
                        await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
                    }
                };

                await _channel.BasicConsumeAsync(Queue, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "RabbitMQ not available for ReadingRecordedConsumer. Retrying in 30s...");
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        base.Dispose();
    }
}
