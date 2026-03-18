using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace PayrollServices.Infrastructure.Messaging;

/// <summary>
/// RabbitMQ message consumer base class
/// </summary>
public abstract class RabbitMqConsumerBase : IDisposable
{
    protected readonly IConnection Connection;
    protected readonly IModel Channel;
    protected readonly string QueueName;

    protected RabbitMqConsumerBase(IConnection connection, string queueName, string exchangeName, string routingKey)
    {
        Connection = connection;
        QueueName = queueName;
        Channel = connection.CreateModel();

        // Declare exchange and queue
        Channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, durable: true);
        Channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);
        Channel.QueueBind(queueName, exchangeName, routingKey);
    }

    public abstract Task ProcessMessageAsync(string message);

    public void StartConsuming()
    {
        var consumer = new AsyncEventingBasicConsumer(Channel);
        consumer.Received += async (model, ea) =>
        {
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
            await ProcessMessageAsync(message);
            Channel.BasicAck(ea.DeliveryTag, false);
        };

        Channel.BasicConsume(QueueName, false, consumer);
    }

    public void Dispose()
    {
        Channel?.Dispose();
    }
}
