using System.Text;
using System.Text.Json;
using EmployeeTransactionsService.Application.Contracts;
using EmployeeTransactionsService.Domain.Events;
using EmployeeTransactionsService.Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EmployeeTransactionsService.Infrastructure.Messaging;

public sealed class RabbitMqOptions
{
    public const string SectionName = "RabbitMQ";
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string VirtualHost { get; set; } = "/";
    public string ExchangeName { get; set; } = "employee-transactions.events";
}

public static class RabbitMqConnectionFactory
{
    public static IConnection? Create(IConfiguration configuration)
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = configuration["RabbitMQ:Host"] ?? "localhost",
                Port = int.TryParse(configuration["RabbitMQ:Port"], out var port) ? port : 5672,
                UserName = configuration["RabbitMQ:UserName"] ?? "guest",
                Password = configuration["RabbitMQ:Password"] ?? "guest",
                VirtualHost = configuration["RabbitMQ:VirtualHost"] ?? "/"
            };
            return factory.CreateConnectionAsync().GetAwaiter().GetResult();
        }
        catch
        {
            return null;
        }
    }
}

public sealed class RabbitMqConnectionProvider(IConnection? connection)
{
    public IConnection? Connection { get; } = connection;
}

public sealed class RabbitMqMessagePublisher(RabbitMqConnectionProvider connectionProvider, IOptions<RabbitMqOptions> options, ResiliencePipeline pipeline, ILogger<RabbitMqMessagePublisher> logger)
    : IMessagePublisher, IAsyncDisposable
{
    private IChannel? _channel;
    private readonly RabbitMqOptions _options = options.Value;

    public async Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken cancellationToken = default)
    {
        if (connectionProvider.Connection is null)
        {
            logger.LogWarning("RabbitMQ unavailable. Skipping publish to {Exchange}/{RoutingKey}", exchange, routingKey);
            return;
        }

        _channel ??= await connectionProvider.Connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await pipeline.ExecuteAsync(async token =>
        {
            await _channel.ExchangeDeclareAsync(exchange, ExchangeType.Topic, durable: true, cancellationToken: token);
            var payload = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            var properties = new BasicProperties { ContentType = "application/json", DeliveryMode = DeliveryModes.Persistent };
            await _channel.BasicPublishAsync(exchange, routingKey, false, properties, payload, token);
        }, cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null)
            await _channel.DisposeAsync();
    }
}

public sealed class EmployeeDomainEventHandler(IMessagePublisher messagePublisher, ILogger<EmployeeDomainEventHandler> logger, IOptions<RabbitMqOptions> options)
    : INotificationHandler<DomainEventNotification>
{
    private readonly string _exchange = options.Value.ExchangeName;

    public async Task Handle(DomainEventNotification notification, CancellationToken cancellationToken)
    {
        switch (notification.DomainEvent)
        {
            case EmployeeCreatedDomainEvent employeeCreated:
                logger.LogInformation("Publishing employee created event for {EmployeeId}", employeeCreated.EmployeeId);
                await messagePublisher.PublishAsync(_exchange, "employee.onboarded", employeeCreated, cancellationToken);
                break;
            case EmployeeGradeChangedDomainEvent gradeChanged:
                logger.LogInformation("Publishing employee grade changed event for {EmployeeId}", gradeChanged.EmployeeId);
                await messagePublisher.PublishAsync(_exchange, "employee.grade.changed", gradeChanged, cancellationToken);
                break;
            case ProbationReviewedDomainEvent probationReviewed:
                logger.LogInformation("Publishing probation reviewed event for {EmployeeId}", probationReviewed.EmployeeId);
                await messagePublisher.PublishAsync(_exchange, "probation.reviewed", probationReviewed, cancellationToken);
                break;
            case AlertGroupCreatedDomainEvent alertGroupCreated:
                await messagePublisher.PublishAsync(_exchange, "alert-group.created", alertGroupCreated, cancellationToken);
                break;
            case StationeryImageUploadedDomainEvent stationeryImageUploaded:
                await messagePublisher.PublishAsync(_exchange, "stationery.image.uploaded", stationeryImageUploaded, cancellationToken);
                break;
        }
    }
}

public abstract class RabbitMqConsumerBase(RabbitMqConnectionProvider connectionProvider, IOptions<RabbitMqOptions> options, ILogger logger) : BackgroundService
{
    private readonly IConnection? _connection = connectionProvider.Connection;
    private readonly RabbitMqOptions _options = options.Value;
    private readonly ILogger _logger = logger;
    private IChannel? _channel;

    protected abstract string QueueName { get; }
    protected abstract string RoutingKey { get; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_connection is null)
        {
            _logger.LogWarning("RabbitMQ unavailable. Consumer {QueueName} will not start.", QueueName);
            return;
        }

        _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);
        await _channel.ExchangeDeclareAsync(_options.ExchangeName, ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
        await _channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
        await _channel.QueueBindAsync(QueueName, _options.ExchangeName, RoutingKey, cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, args) =>
        {
            var body = Encoding.UTF8.GetString(args.Body.ToArray());
            _logger.LogInformation("Consumed message from {Queue}: {Body}", QueueName, body);
            await _channel.BasicAckAsync(args.DeliveryTag, false, stoppingToken);
        };

        await _channel.BasicConsumeAsync(QueueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel is not null)
            await _channel.DisposeAsync();

        await base.StopAsync(cancellationToken);
    }
}

public sealed class EmployeeOnboardedConsumer(RabbitMqConnectionProvider connectionProvider, IOptions<RabbitMqOptions> options, ILogger<EmployeeOnboardedConsumer> logger)
    : RabbitMqConsumerBase(connectionProvider, options, logger)
{
    protected override string QueueName => "employee-transactions.employee.onboarded";
    protected override string RoutingKey => "employee.onboarded";
}

public sealed class ProbationReviewedConsumer(RabbitMqConnectionProvider connectionProvider, IOptions<RabbitMqOptions> options, ILogger<ProbationReviewedConsumer> logger)
    : RabbitMqConsumerBase(connectionProvider, options, logger)
{
    protected override string QueueName => "employee-transactions.probation.reviewed";
    protected override string RoutingKey => "probation.reviewed";
}