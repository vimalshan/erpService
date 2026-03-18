namespace ApprovalService.Infrastructure.Messaging;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

/// <summary>
/// Base class for RabbitMQ message consumers
/// </summary>
public abstract class RabbitMqConsumerBase
{
    protected readonly IConnection Connection;
    protected readonly ILogger Logger;
    protected IModel? Channel;

    protected RabbitMqConsumerBase(IConnection connection, ILogger logger)
    {
        Connection = connection;
        Logger = logger;
    }

    public virtual void Start(string queueName, string routingKey, string exchange = "approval-service")
    {
        try
        {
            Channel = Connection.CreateModel();

            // Declare exchange
            Channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Topic, durable: true);

            // Declare queue
            Channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);

            // Bind queue to exchange
            Channel.QueueBind(queue: queueName, exchange: exchange, routingKey: routingKey);

            var consumer = new EventingBasicConsumer(Channel);
            consumer.Received += async (model, ea) => await OnMessageReceivedAsync(ea);

            Channel.BasicConsume(queue: queueName, autoAck: false, consumerTag: GetConsumerTag(), noLocal: false, exclusive: false, arguments: null, consumer: consumer);

            Logger.LogInformation("Consumer started for queue {QueueName}", queueName);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error starting consumer for queue {QueueName}", queueName);
            throw;
        }
    }

    protected abstract Task OnMessageReceivedAsync(BasicDeliverEventArgs ea);

    protected string DecodeMessage(byte[] body)
    {
        return Encoding.UTF8.GetString(body);
    }

    protected T? DeserializeMessage<T>(string message)
    {
        return JsonSerializer.Deserialize<T>(message);
    }

    protected virtual string GetConsumerTag()
    {
        return GetType().Name;
    }

    public virtual void Stop()
    {
        Channel?.Close();
        Logger.LogInformation("Consumer stopped");
    }

    public void Dispose()
    {
        Channel?.Dispose();
    }
}
