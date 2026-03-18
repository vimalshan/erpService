namespace AccessService.Infrastructure.MessageBrokers.RabbitMQ
{
    /// <summary>
    /// Interface for publishing messages to RabbitMQ
    /// </summary>
    public interface IRabbitMQPublisher
    {
        Task PublishAsync(string exchangeName, string routingKey, string message, Dictionary<string, object> headers = null);
        Task PublishAsync(string queueName, string message, Dictionary<string, object> headers = null);
    }
}
