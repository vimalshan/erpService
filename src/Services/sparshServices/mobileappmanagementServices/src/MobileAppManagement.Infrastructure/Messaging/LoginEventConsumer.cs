using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MobileAppManagement.Infrastructure.Messaging;

public class LoginEventConsumer : BackgroundService
{
    private readonly ILogger<LoginEventConsumer> _logger;
    private readonly IConfiguration _configuration;
    private IConnection? _connection;
    private IChannel? _channel;

    public LoginEventConsumer(ILogger<LoginEventConsumer> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQ:HostName"] ?? "localhost",
            UserName = _configuration["RabbitMQ:UserName"] ?? "guest",
            Password = _configuration["RabbitMQ:Password"] ?? "guest",
            Port = int.TryParse(_configuration["RabbitMQ:Port"], out var port) ? port : 5672
        };

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _connection = await factory.CreateConnectionAsync(stoppingToken);
                _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

                await _channel.ExchangeDeclareAsync("mobile-app", ExchangeType.Topic, durable: true,
                    cancellationToken: stoppingToken);
                await _channel.QueueDeclareAsync("login-event-queue", durable: true, exclusive: false,
                    autoDelete: false, cancellationToken: stoppingToken);
                await _channel.QueueBindAsync("login-event-queue", "mobile-app", "user.logged-in",
                    cancellationToken: stoppingToken);

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += async (_, ea) =>
                {
                    var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                    _logger.LogInformation("Received login event: {Message}", body);

                    try
                    {
                        await ProcessLoginEventAsync(body, stoppingToken);
                        await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing login event message");
                        await _channel.BasicNackAsync(ea.DeliveryTag, false, true, stoppingToken);
                    }
                };

                await _channel.BasicConsumeAsync("login-event-queue", false, consumer,
                    cancellationToken: stoppingToken);

                _logger.LogInformation("LoginEventConsumer started listening on queue: login-event-queue");

                // Keep running until cancellation
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("LoginEventConsumer stopping.");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "LoginEventConsumer could not connect to RabbitMQ. Retrying in 30 seconds...");

                if (_channel is { IsOpen: true }) await _channel.CloseAsync(stoppingToken);
                if (_connection is { IsOpen: true }) await _connection.CloseAsync(stoppingToken);

                try { await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken); }
                catch (OperationCanceledException) { break; }
            }
        }
    }

    private Task ProcessLoginEventAsync(string message, CancellationToken ct)
    {
        _logger.LogInformation("Processing login event: {Message}", message);
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel is not null) await _channel.CloseAsync(cancellationToken);
        if (_connection is not null) await _connection.CloseAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}
