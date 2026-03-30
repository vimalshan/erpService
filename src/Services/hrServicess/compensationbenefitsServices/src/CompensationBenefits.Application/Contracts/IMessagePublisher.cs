namespace CompensationBenefits.Application.Contracts;

/// <summary>Abstraction for publishing messages to a message broker (e.g. RabbitMQ).</summary>
public interface IMessagePublisher
{
    Task PublishAsync<T>(string exchange, string routingKey, T message);
}
