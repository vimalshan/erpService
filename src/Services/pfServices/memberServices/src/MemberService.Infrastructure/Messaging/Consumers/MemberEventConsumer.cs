using System.Text;
using System.Text.Json;
using MemberService.Infrastructure.Messaging.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MemberService.Infrastructure.Messaging.Consumers;

public class MemberEventConsumer : BackgroundService
{
    private readonly ILogger<MemberEventConsumer> _logger;
    private readonly IConfiguration _configuration;
    private IConnection? _connection;
    private IChannel? _channel;
    private const string QueueName = "member.events";
    private const string ExchangeName = "member.exchange";

    public MemberEventConsumer(ILogger<MemberEventConsumer> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQ:Host"] ?? "localhost",
            Port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672"),
            UserName = _configuration["RabbitMQ:Username"] ?? "guest",
            Password = _configuration["RabbitMQ:Password"] ?? "guest"
        };

        try
        {
            _connection = await factory.CreateConnectionAsync(stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "RabbitMQ unavailable — MemberEventConsumer will not start. " +
                "Ensure RabbitMQ is running on {Host}:{Port}.",
                factory.HostName, factory.Port);
            return;
        }

        _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await _channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
        await _channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
        await _channel.QueueBindAsync(QueueName, ExchangeName, "member.#", cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            var body = Encoding.UTF8.GetString(ea.Body.ToArray());
            var routingKey = ea.RoutingKey;

            try
            {
                _logger.LogInformation("Received message on {RoutingKey}: {Body}", routingKey, body);

                switch (routingKey)
                {
                    case "member.created":
                        var createdEvent = JsonSerializer.Deserialize<MemberCreatedMessage>(body);
                        await HandleMemberCreated(createdEvent!);
                        break;
                    case "member.closed":
                        var closedEvent = JsonSerializer.Deserialize<MemberClosedMessage>(body);
                        await HandleMemberClosed(closedEvent!);
                        break;
                    default:
                        _logger.LogWarning("Unhandled routing key: {RoutingKey}", routingKey);
                        break;
                }

                await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message on {RoutingKey}", routingKey);
                await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);
            }
        };

        await _channel.BasicConsumeAsync(QueueName, autoAck: false, consumer, stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
            await Task.Delay(1000, stoppingToken);
    }

    private Task HandleMemberCreated(MemberCreatedMessage message)
    {
        _logger.LogInformation("Processing MemberCreated: MemberNo={MemberNo}, Name={Name}",
            message.MemberNo, message.MemberName);
        // downstream processing (notifications, audit, etc.) goes here
        return Task.CompletedTask;
    }

    private Task HandleMemberClosed(MemberClosedMessage message)
    {
        _logger.LogInformation("Processing MemberClosed: MemberNo={MemberNo}, Reason={Reason}",
            message.MemberNo, message.LeaveReason);
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
        if (_channel != null) await _channel.DisposeAsync();
        if (_connection != null) await _connection.DisposeAsync();
    }
}
