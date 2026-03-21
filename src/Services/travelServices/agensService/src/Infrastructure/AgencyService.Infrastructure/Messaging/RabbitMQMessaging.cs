using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using AgencyService.Domain.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace AgencyService.Infrastructure.Messaging;

public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent @event) where TEvent : IDomainEvent;
}

public interface IEventConsumer : IHostedService
{
    void Subscribe<TEvent>(Func<TEvent, Task> handler) where TEvent : IDomainEvent;
}

public class RabbitMQEventPublisher : IEventPublisher, IDisposable
{
    private readonly IConnection _connection;
    private IModel? _channel;
    private readonly ILogger<RabbitMQEventPublisher> _logger;
    
    public RabbitMQEventPublisher(IConnection connection, ILogger<RabbitMQEventPublisher> logger)
    {
        _connection = connection;
        _logger = logger;
        _channel = _connection.CreateModel();
    }
    
    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IDomainEvent
    {
        try
        {
            var eventName = typeof(TEvent).Name;
            var exchange = "agency.events";
            
            _channel.ExchangeDeclare(
                exchange: exchange,
                type: ExchangeType.Topic,
                durable: true);
            
            var message = JsonSerializer.Serialize(@event);
            var body = Encoding.UTF8.GetBytes(message);
            
            _channel.BasicPublish(
                exchange: exchange,
                routingKey: eventName,
                body: body);
            
            _logger.LogInformation("Event published: {EventName}", eventName);
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing event: {EventType}", typeof(TEvent).Name);
            throw;
        }
    }
    
    public void Dispose()
    {
        _channel?.Dispose();
        _channel = null;
    }
}

public class RabbitMQEventConsumer : IEventConsumer
{
    private readonly IConnection _connection;
    private IModel? _channel;
    private readonly ILogger<RabbitMQEventConsumer> _logger;
    private readonly Dictionary<string, Delegate> _eventHandlers = new();
    
    public RabbitMQEventConsumer(IConnection connection, ILogger<RabbitMQEventConsumer> logger)
    {
        _connection = connection;
        _logger = logger;
        _channel = connection.CreateModel();
    }
    
    public void Subscribe<TEvent>(Func<TEvent, Task> handler) where TEvent : IDomainEvent
    {
        var eventName = typeof(TEvent).Name;
        _eventHandlers[eventName] = handler;
        _logger.LogInformation("Subscribed to event: {EventName}", eventName);
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var exchange = "agency.events";
        var queue = "agency.events.queue";
        
        _channel.ExchangeDeclare(
            exchange: exchange,
            type: ExchangeType.Topic,
            durable: true);
        
        _channel.QueueDeclare(
            queue: queue,
            durable: true);
        
        foreach (var eventName in _eventHandlers.Keys)
        {
            _channel.QueueBind(
                queue: queue,
                exchange: exchange,
                routingKey: eventName);
        }
        
        _logger.LogInformation("RabbitMQ Event Consumer started");
        await Task.CompletedTask;
    }
    
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("RabbitMQ Event Consumer stopped");
        await Task.CompletedTask;
    }
}

public static class MessagingServiceCollectionExtensions
{
    public static IServiceCollection AddRabbitMQMessaging(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var rabbitMQSettings = configuration.GetSection("RabbitMQ");
        var hostName = rabbitMQSettings["HostName"] ?? "localhost";
        var userName = rabbitMQSettings["UserName"] ?? "guest";
        var password = rabbitMQSettings["Password"] ?? "guest";
        var port = int.Parse(rabbitMQSettings["Port"] ?? "5672");
        
        var factory = new ConnectionFactory
        {
            HostName = hostName,
            UserName = userName,
            Password = password,
            Port = port
        };
        
        // Try to create connection, but don't fail if RabbitMQ is unavailable
        try
        {
            var connection = factory.CreateConnection();
            services.AddSingleton(connection);
            services.AddSingleton<IEventPublisher, RabbitMQEventPublisher>();
            services.AddSingleton<IEventConsumer, RabbitMQEventConsumer>();
            services.AddHostedService(sp => sp.GetRequiredService<IEventConsumer>());
        }
        catch (Exception ex)
        {
            // Log warning but don't throw - allow app to start without RabbitMQ
            Console.WriteLine($"[WARNING] RabbitMQ connection failed: {ex.Message}. Application will run without messaging features.");
            // Still register dummy implementations so DI doesn't fail
            services.AddSingleton<IEventPublisher>(new NoOpEventPublisher());
        }
        
        return services;
    }
}

// Dummy implementation for when RabbitMQ is unavailable
public class NoOpEventPublisher : IEventPublisher
{
    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IDomainEvent
    {
        Console.WriteLine($"[NOOP] Event not published (RabbitMQ unavailable): {typeof(TEvent).Name}");
        await Task.CompletedTask;
    }
}
