using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AttendanceService.Infrastructure.EventBus.RabbitMQ;

public class EventBusRabbitMQ(RabbitMQConnection connection, ILogger<EventBusRabbitMQ> logger)
{
    public async Task PublishAsync<T>(T message, string exchange, string routingKey)
    {
        try
        {
            var channel = await connection.CreateChannelAsync();
            await channel.ExchangeDeclareAsync(exchange, ExchangeType.Topic, durable: true);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            var props = new BasicProperties { ContentType = "application/json", DeliveryMode = DeliveryModes.Persistent };

            await channel.BasicPublishAsync(exchange, routingKey, false, props, body);
            logger.LogInformation("Published {MessageType} to {Exchange}/{RoutingKey}", typeof(T).Name, exchange, routingKey);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "RabbitMQ unavailable — skipping publish of {MessageType} to {Exchange}/{RoutingKey}", typeof(T).Name, exchange, routingKey);
        }
    }

    public async Task SubscribeAsync(string queue, string exchange, string routingKey,
        Func<string, Task> onMessage)
    {
        var channel = await connection.CreateChannelAsync();
        await channel.ExchangeDeclareAsync(exchange, ExchangeType.Topic, durable: true);
        await channel.QueueDeclareAsync(queue, durable: true, exclusive: false, autoDelete: false);
        await channel.QueueBindAsync(queue, exchange, routingKey);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            var body = Encoding.UTF8.GetString(ea.Body.ToArray());
            try
            {
                await onMessage(body);
                await channel.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing message from {Queue}", queue);
                await channel.BasicNackAsync(ea.DeliveryTag, false, requeue: true);
            }
        };

        await channel.BasicConsumeAsync(queue, autoAck: false, consumer);
    }
}
