using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace UserService.Infrastructure.Messaging;

/// <summary>
/// RabbitMQ Message Publisher
/// </summary>
public class RabbitMqPublisher
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqPublisher(string hostName, int port, string userName, string password)
    {
        var factory = new ConnectionFactory
        {
            HostName = hostName,
            Port = port,
            UserName = userName,
            Password = password,
            DispatchConsumersAsync = true
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public void PublishMessage(string exchangeName, string routingKey, string message)
    {
        try
        {
            _channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, durable: true);

            var body = System.Text.Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(
                exchange: exchangeName,
                routingKey: routingKey,
                basicProperties: null,
                body: body);

            Log.Information("Message published to {ExchangeName}/{RoutingKey}", exchangeName, routingKey);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error publishing message to {ExchangeName}/{RoutingKey}", exchangeName, routingKey);
            throw;
        }
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}

/// <summary>
/// RabbitMQ Message Consumer base class
/// </summary>
public abstract class RabbitMqConsumer
{
    protected readonly IConnection Connection;
    protected readonly IModel Channel;
    protected string QueueName { get; }

    protected RabbitMqConsumer(string hostName, int port, string userName, string password, string queueName)
    {
        QueueName = queueName;

        var factory = new ConnectionFactory
        {
            HostName = hostName,
            Port = port,
            UserName = userName,
            Password = password,
            DispatchConsumersAsync = true
        };

        Connection = factory.CreateConnection();
        Channel = Connection.CreateModel();
    }

    public virtual void StartConsuming(string exchangeName, string routingKey)
    {
        try
        {
            Channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, durable: true);
            Channel.QueueDeclare(QueueName, durable: true, exclusive: false);
            Channel.QueueBind(QueueName, exchangeName, routingKey);

            var consumer = new AsyncEventingBasicConsumer(Channel);
            consumer.Received += async (model, eventArgs) =>
            {
                try
                {
                    var message = System.Text.Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                    await HandleMessage(message);
                    Channel.BasicAck(eventArgs.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error processing message from {QueueName}", QueueName);
                    Channel.BasicNack(eventArgs.DeliveryTag, false, true);
                }
            };

            Channel.BasicConsume(QueueName, autoAck: false, consumer: consumer);
            Log.Information("Started consuming from queue {QueueName}", QueueName);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error starting consumer for queue {QueueName}", QueueName);
            throw;
        }
    }

    protected abstract Task HandleMessage(string message);

    public virtual void Dispose()
    {
        Channel?.Dispose();
        Connection?.Dispose();
    }
}

/// <summary>
/// User Domain Event Consumer
/// </summary>
public class UserDomainEventConsumer : RabbitMqConsumer
{
    public UserDomainEventConsumer(
        string hostName,
        int port,
        string userName,
        string password)
        : base(hostName, port, userName, password, "user-domain-events-queue")
    {
    }

    protected override async Task HandleMessage(string message)
    {
        Log.Information("Processing user domain event: {Message}", message);
        // Parse and process the domain event
        // This could trigger additional business logic, notifications, etc.
        await Task.Delay(100); // Simulate processing
        Log.Information("User domain event processed successfully");
    }
}
