using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;

namespace CategoryAndVendorService.Infrastructure.Messaging;

public abstract class RabbitMqConsumerBase : BackgroundService
{
    private readonly string _hostName;
    private readonly string _userName;
    private readonly string _password;
    private readonly string _queueName;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger _logger;
    private IConnection? _connection;
    private IChannel? _channel;

    protected RabbitMqConsumerBase(string hostName, string userName, string password,
        string queueName, IServiceScopeFactory scopeFactory, ILogger logger)
    {
        _hostName = hostName;
        _userName = userName;
        _password = password;
        _queueName = queueName;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            try
            {
                var factory = new ConnectionFactory { HostName = _hostName, UserName = _userName, Password = _password };
                _connection = await factory.CreateConnectionAsync(ct);
                _channel = await _connection.CreateChannelAsync(cancellationToken: ct);

                await _channel.QueueDeclareAsync(queue: _queueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: ct);

                _logger.LogInformation("Connected to RabbitMQ queue {Queue}", _queueName);

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += async (model, ea) =>
                {
                    try
                    {
                        var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                        using var scope = _scopeFactory.CreateScope();
                        await HandleMessageAsync(body, scope.ServiceProvider, ct);
                        await _channel.BasicAckAsync(ea.DeliveryTag, false, ct);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing message from queue {Queue}", _queueName);
                        await _channel.BasicNackAsync(ea.DeliveryTag, false, true, ct);
                    }
                };

                await _channel.BasicConsumeAsync(queue: _queueName, autoAck: false, consumer: consumer, cancellationToken: ct);

                // Keep alive until cancellation
                await Task.Delay(Timeout.Infinite, ct);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "RabbitMQ connection failed for queue {Queue}. Retrying in 30 seconds...", _queueName);
                try { await Task.Delay(TimeSpan.FromSeconds(30), ct); }
                catch (OperationCanceledException) { break; }
            }
        }
    }

    protected abstract Task HandleMessageAsync(string message, IServiceProvider serviceProvider, CancellationToken ct);

    public override async Task StopAsync(CancellationToken ct)
    {
        if (_channel is not null) await _channel.CloseAsync(ct);
        if (_connection is not null) await _connection.CloseAsync(ct);
        await base.StopAsync(ct);
    }
}
