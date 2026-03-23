using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using MediatR;
using ApiGateway.DomainEvents;

namespace ApiGateway.Messaging;

public static class RabbitMqExtensions
{
    public static IServiceCollection AddRabbitMqMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<RabbitMqConnectionManager>();
        services.AddHostedService<EmployeeEventConsumer>();
        services.AddHostedService<AttendanceEventConsumer>();
        services.AddHostedService<LeaveEventConsumer>();
        services.AddHostedService<VisitorEventConsumer>();
        services.AddHostedService<AccessEventConsumer>();

        return services;
    }
}

public class RabbitMqConnectionManager : IDisposable
{
    private readonly IConnection? _connection;
    private readonly ILogger<RabbitMqConnectionManager> _logger;

    public RabbitMqConnectionManager(IConfiguration configuration, ILogger<RabbitMqConnectionManager> logger)
    {
        _logger = logger;
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = configuration["RabbitMQ:Host"] ?? "localhost",
                Port = int.Parse(configuration["RabbitMQ:Port"] ?? "5672"),
                UserName = configuration["RabbitMQ:Username"] ?? "guest",
                Password = configuration["RabbitMQ:Password"] ?? "guest",
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };
            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
            _logger.LogInformation("RabbitMQ connection established");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "RabbitMQ connection failed. Consumers will not be active");
        }
    }

    public IConnection? Connection => _connection;

    public void Dispose()
    {
        _connection?.Dispose();
    }
}

public abstract class BaseEventConsumer : BackgroundService
{
    protected readonly RabbitMqConnectionManager ConnectionManager;
    protected readonly IServiceProvider ServiceProvider;
    protected readonly ILogger Logger;
    protected readonly string QueueName;
    protected readonly string ExchangeName;

    protected BaseEventConsumer(
        RabbitMqConnectionManager connectionManager,
        IServiceProvider serviceProvider,
        ILogger logger,
        string queueName,
        string exchangeName)
    {
        ConnectionManager = connectionManager;
        ServiceProvider = serviceProvider;
        Logger = logger;
        QueueName = queueName;
        ExchangeName = exchangeName;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (ConnectionManager.Connection is null)
        {
            Logger.LogWarning("RabbitMQ not connected. {Consumer} will not start", GetType().Name);
            return;
        }

        try
        {
            var channel = await ConnectionManager.Connection.CreateChannelAsync(cancellationToken: stoppingToken);
            await channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Fanout, durable: true, cancellationToken: stoppingToken);
            await channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
            await channel.QueueBindAsync(QueueName, ExchangeName, string.Empty, cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                try
                {
                    var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                    Logger.LogInformation("[{Consumer}] Received: {Message}", GetType().Name, body);
                    await HandleMessageAsync(body);
                    await channel.BasicAckAsync(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "[{Consumer}] Error processing message", GetType().Name);
                    await channel.BasicNackAsync(ea.DeliveryTag, false, true);
                }
            };

            await channel.BasicConsumeAsync(QueueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
            Logger.LogInformation("{Consumer} started listening on queue {Queue}", GetType().Name, QueueName);

            // Keep running until cancelled
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException) { /* Shutdown */ }
        catch (Exception ex)
        {
            Logger.LogError(ex, "{Consumer} encountered an error", GetType().Name);
        }
    }

    protected abstract Task HandleMessageAsync(string message);
}

// ── Employee Events ──
public class EmployeeEventConsumer : BaseEventConsumer
{
    public EmployeeEventConsumer(RabbitMqConnectionManager cm, IServiceProvider sp, ILogger<EmployeeEventConsumer> logger)
        : base(cm, sp, logger, "gateway.employee.events", "employee-events") { }

    protected override async Task HandleMessageAsync(string message)
    {
        using var scope = ServiceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var eventData = JsonSerializer.Deserialize<JsonElement>(message);
        await mediator.Publish(new ServiceStateChangedEvent("employee-service", "EmployeeEvent", message));
    }
}

// ── Attendance Events ──
public class AttendanceEventConsumer : BaseEventConsumer
{
    public AttendanceEventConsumer(RabbitMqConnectionManager cm, IServiceProvider sp, ILogger<AttendanceEventConsumer> logger)
        : base(cm, sp, logger, "gateway.attendance.events", "attendance-events") { }

    protected override async Task HandleMessageAsync(string message)
    {
        using var scope = ServiceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Publish(new ServiceStateChangedEvent("attendance-service", "AttendanceEvent", message));
    }
}

// ── Leave Events ──
public class LeaveEventConsumer : BaseEventConsumer
{
    public LeaveEventConsumer(RabbitMqConnectionManager cm, IServiceProvider sp, ILogger<LeaveEventConsumer> logger)
        : base(cm, sp, logger, "gateway.leave.events", "leave-events") { }

    protected override async Task HandleMessageAsync(string message)
    {
        using var scope = ServiceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Publish(new ServiceStateChangedEvent("leave-service", "LeaveEvent", message));
    }
}

// ── Visitor Events ──
public class VisitorEventConsumer : BaseEventConsumer
{
    public VisitorEventConsumer(RabbitMqConnectionManager cm, IServiceProvider sp, ILogger<VisitorEventConsumer> logger)
        : base(cm, sp, logger, "gateway.visitor.events", "visitor-events") { }

    protected override async Task HandleMessageAsync(string message)
    {
        using var scope = ServiceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Publish(new ServiceStateChangedEvent("visitor-service", "VisitorEvent", message));
    }
}

// ── Access Events ──
public class AccessEventConsumer : BaseEventConsumer
{
    public AccessEventConsumer(RabbitMqConnectionManager cm, IServiceProvider sp, ILogger<AccessEventConsumer> logger)
        : base(cm, sp, logger, "gateway.access.events", "access-events") { }

    protected override async Task HandleMessageAsync(string message)
    {
        using var scope = ServiceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Publish(new ServiceStateChangedEvent("access-service", "AccessEvent", message));
    }
}
