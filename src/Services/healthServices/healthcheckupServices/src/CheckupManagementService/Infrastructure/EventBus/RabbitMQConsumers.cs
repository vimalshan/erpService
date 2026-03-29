using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Shared.Events;
using Microsoft.Extensions.Logging;
using MediatR;

namespace CheckupManagementService.Infrastructure.EventBus;

/// <summary>
/// Base class for RabbitMQ event consumers
/// </summary>
public abstract class RabbitMQConsumerBase : BackgroundService
{
    protected readonly IConfiguration _configuration;
    protected readonly ILogger<RabbitMQConsumerBase> _logger;
    protected IConnection? _connection;
    protected IModel? _channel;
    protected AsyncEventingBasicConsumer? _consumer;

    public RabbitMQConsumerBase(IConfiguration configuration, ILogger<RabbitMQConsumerBase> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    protected virtual void InitializeRabbitMQ()
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration.GetValue<string>("RabbitMQ:Host") ?? "localhost",
                Port = _configuration.GetValue<int>("RabbitMQ:Port") == 0 ? 5672 : _configuration.GetValue<int>("RabbitMQ:Port"),
                UserName = _configuration.GetValue<string>("RabbitMQ:Username") ?? "guest",
                Password = _configuration.GetValue<string>("RabbitMQ:Password") ?? "guest",
                DispatchConsumersAsync = true,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            var exchangeName = _configuration.GetValue<string>("RabbitMQ:ExchangeName") ?? "health_exchange";
            var queueName = GetQueueName();
            var routingKey = GetRoutingKey();

            _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic, durable: true);
            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);
            _channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey);

            _channel.BasicQos(0, 10, false);

            _consumer = new AsyncEventingBasicConsumer(_channel);
            _consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    
                    await OnMessageReceived(message);
                    _channel?.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message");
                    _channel?.BasicNack(ea.DeliveryTag, false, true);
                }
            };

            _channel.BasicConsume(queue: queueName, autoAck: false, consumer: _consumer);
            _logger.LogInformation("RabbitMQ consumer initialized successfully");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to initialize RabbitMQ - will retry later");
            // Don't rethrow - let the consumer retry gracefully
        }
    }

    protected abstract string GetQueueName();
    protected abstract string GetRoutingKey();
    protected abstract Task OnMessageReceived(string message);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            InitializeRabbitMQ();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to initialize RabbitMQ consumer, will retry on next interval");
        }
        
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // If not connected, try to reconnect
                if (_channel == null || !_channel.IsOpen)
                {
                    InitializeRabbitMQ();
                }
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error in consumer loop, will retry");
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
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

/// <summary>
/// Consumer for checkup created events
/// </summary>
public class CheckupCreatedEventConsumer : RabbitMQConsumerBase
{
    private readonly IMediator _mediator;

    public CheckupCreatedEventConsumer(IConfiguration configuration, ILogger<CheckupCreatedEventConsumer> logger, IMediator mediator)
        : base(configuration, logger)
    {
        _mediator = mediator;
    }

    protected override string GetQueueName() => "checkup_created_queue";
    protected override string GetRoutingKey() => "checkup.created";

    protected override async Task OnMessageReceived(string message)
    {
        try
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var domainEvent = JsonSerializer.Deserialize<dynamic>(message, options);
            
            // Processing checkup created event
            // Handle the event - implement business logic here
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing checkup created event");
            throw;
        }
    }
}

/// <summary>
/// Consumer for health examination events
/// </summary>
public class HealthExaminationEventConsumer : RabbitMQConsumerBase
{
    private readonly IMediator _mediator;

    public HealthExaminationEventConsumer(IConfiguration configuration, ILogger<HealthExaminationEventConsumer> logger, IMediator mediator)
        : base(configuration, logger)
    {
        _mediator = mediator;
    }

    protected override string GetQueueName() => "health_examination_queue";
    protected override string GetRoutingKey() => "checkup.examination.*";

    protected override async Task OnMessageReceived(string message)
    {
        try
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var domainEvent = JsonSerializer.Deserialize<dynamic>(message, options);

            // Processing health examination event
            // Handle the event - implement business logic here
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing health examination event");
            throw;
        }
    }
}

/// <summary>
/// Consumer for checkup approval events
/// </summary>
public class CheckupApprovalEventConsumer : RabbitMQConsumerBase
{
    private readonly IMediator _mediator;

    public CheckupApprovalEventConsumer(IConfiguration configuration, ILogger<CheckupApprovalEventConsumer> logger, IMediator mediator)
        : base(configuration, logger)
    {
        _mediator = mediator;
    }

    protected override string GetQueueName() => "checkup_approval_queue";
    protected override string GetRoutingKey() => "checkup.approval.*";

    protected override async Task OnMessageReceived(string message)
    {
        try
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var domainEvent = JsonSerializer.Deserialize<dynamic>(message, options);

            // Processing checkup approval event
            // Handle the event - implement business logic here
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing checkup approval event");
            throw;
        }
    }
}
