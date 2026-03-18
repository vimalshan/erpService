namespace AccidentManagementService.Infrastructure.EventBus;

/// <summary>
/// RabbitMQ configuration settings
/// Maps from appsettings.json -> RabbitMQ section
/// </summary>
public class RabbitMQSettings
{
    /// <summary>
    /// RabbitMQ broker hostname or IP address
    /// Default: localhost
    /// </summary>
    public string Host { get; set; } = "localhost";

    /// <summary>
    /// RabbitMQ broker port
    /// Default: 5672 (AMQP), 5671 (AMQP over TLS)
    /// </summary>
    public int Port { get; set; } = 5672;

    /// <summary>
    /// RabbitMQ username for authentication
    /// Default: guest
    /// </summary>
    public string Username { get; set; } = "guest";

    /// <summary>
    /// RabbitMQ password for authentication
    /// Default: guest
    /// </summary>
    public string Password { get; set; } = "guest";

    /// <summary>
    /// Virtual host on RabbitMQ broker
    /// Default: /
    /// </summary>
    public string VirtualHost { get; set; } = "/";

    /// <summary>
    /// Enable SSL/TLS encryption
    /// Default: false
    /// </summary>
    public bool UseSsl { get; set; } = false;

    /// <summary>
    /// Queue name prefix (used to create environment-specific queues)
    /// Example: dev-accident-events, prod-accident-events
    /// </summary>
    public string QueuePrefix { get; set; } = "accident";

    /// <summary>
    /// Number of consumers (parallel processing threads)
    /// Default: 3
    /// </summary>
    public int ConsumerCount { get; set; } = 3;

    /// <summary>
    /// Message prefetch count (QoS)
    /// Default: 10
    /// </summary>
    public ushort PrefetchCount { get; set; } = 10;

    /// <summary>
    /// Request timeout in milliseconds
    /// Default: 30000 (30 seconds)
    /// </summary>
    public int RequestTimeoutMs { get; set; } = 30000;
}

/// <summary>
/// Event bus abstraction for publishing integration events
/// Implemented by RabbitMQEventBus
/// </summary>
public interface IEventBus
{
    /// <summary>
    /// Publishes an integration event to the message bus
    /// </summary>
    /// <typeparam name="TIntegrationEvent">Type of integration event</typeparam>
    /// <param name="event">Event to publish</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing asynchronous operation</returns>
    Task PublishAsync<TIntegrationEvent>(
        TIntegrationEvent @event,
        CancellationToken cancellationToken = default)
        where TIntegrationEvent : IntegrationEvent;

    /// <summary>
    /// Subscribes to integration events of a specific type
    /// </summary>
    /// <typeparam name="TIntegrationEvent">Type of event to subscribe to</typeparam>
    /// <typeparam name="TEventHandler">Type of event handler</typeparam>
    /// <returns>Task representing asynchronous operation</returns>
    Task SubscribeAsync<TIntegrationEvent, TEventHandler>()
        where TIntegrationEvent : IntegrationEvent
        where TEventHandler : IIntegrationEventHandler<TIntegrationEvent>;

    /// <summary>
    /// Unsubscribes from integration events
    /// </summary>
    /// <typeparam name="TIntegrationEvent">Type of event</typeparam>
    /// <typeparam name="TEventHandler">Type of event handler</typeparam>
    /// <returns>Task representing asynchronous operation</returns>
    Task UnsubscribeAsync<TIntegrationEvent, TEventHandler>()
        where TIntegrationEvent : IntegrationEvent
        where TEventHandler : IIntegrationEventHandler<TIntegrationEvent>;
}

/// <summary>
/// Base interface for integration event handlers
/// Implement this interface for each event type you want to handle
/// </summary>
/// <typeparam name="TIntegrationEvent">Type of integration event to handle</typeparam>
public interface IIntegrationEventHandler<TIntegrationEvent>
    where TIntegrationEvent : IntegrationEvent
{
    /// <summary>
    /// Handles the integration event
    /// </summary>
    /// <param name="event">Event to handle</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing asynchronous operation</returns>
    Task Handle(TIntegrationEvent @event, CancellationToken cancellationToken = default);
}
