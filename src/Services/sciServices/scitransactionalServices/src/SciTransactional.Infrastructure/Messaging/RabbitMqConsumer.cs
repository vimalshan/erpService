using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace SciTransactional.Infrastructure.Messaging;

public abstract class RabbitMqConsumerBase<TMessage> : BackgroundService
{
    private readonly ILogger _logger;
    private readonly string _queueName;
    private readonly string _exchange;
    private readonly string _routingKey;
    private readonly IConfiguration _configuration;
    private IConnection? _connection;
    private IChannel? _channel;

    protected RabbitMqConsumerBase(IConfiguration configuration, ILogger logger,
        string queueName, string exchange, string routingKey)
    {
        _configuration = configuration;
        _logger = logger;
        _queueName = queueName;
        _exchange = exchange;
        _routingKey = routingKey;
    }

    protected abstract Task HandleMessageAsync(TMessage message, CancellationToken ct);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:HostName"] ?? "localhost",
                UserName = _configuration["RabbitMQ:UserName"] ?? "guest",
                Password = _configuration["RabbitMQ:Password"] ?? "guest"
            };

            _connection = await factory.CreateConnectionAsync(stoppingToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await _channel.ExchangeDeclareAsync(_exchange, ExchangeType.Topic,
                durable: true, cancellationToken: stoppingToken);
            await _channel.QueueDeclareAsync(_queueName, durable: true, exclusive: false,
                autoDelete: false, cancellationToken: stoppingToken);
            await _channel.QueueBindAsync(_queueName, _exchange, _routingKey,
                cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                try
                {
                    var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var message = JsonSerializer.Deserialize<TMessage>(body);
                    if (message is not null)
                        await HandleMessageAsync(message, stoppingToken);
                    await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message from {Queue}", _queueName);
                    await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false,
                        requeue: true, cancellationToken: stoppingToken);
                }
            };

            await _channel.BasicConsumeAsync(_queueName, autoAck: false, consumer: consumer,
                cancellationToken: stoppingToken);

            _logger.LogInformation("RabbitMQ consumer started on queue {Queue}", _queueName);
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogWarning(ex, "RabbitMQ consumer for {Queue} could not connect.", _queueName);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel is not null) await _channel.CloseAsync(cancellationToken);
        if (_connection is not null) await _connection.CloseAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}

public sealed record NavigationUpdateMessage(long RequestNum, string NewStatus);

public sealed class NavigationUpdatedConsumer(
    IConfiguration configuration,
    ILogger<NavigationUpdatedConsumer> logger,
    IServiceProvider serviceProvider)
    : RabbitMqConsumerBase<NavigationUpdateMessage>(
        configuration, logger, "sci-transactional-updates", "sci-transactional", "navigation.updated")
{
    protected override async Task HandleMessageAsync(NavigationUpdateMessage message, CancellationToken ct)
    {
        using var scope = serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<MediatR.IMediator>();
        await mediator.Send(
            new Application.Commands.UpdateNavigation.UpdateNavigationCommand(
                message.RequestNum, message.NewStatus), ct);
        logger.LogInformation("Processed navigation update for {RequestNum}", message.RequestNum);
    }
}
