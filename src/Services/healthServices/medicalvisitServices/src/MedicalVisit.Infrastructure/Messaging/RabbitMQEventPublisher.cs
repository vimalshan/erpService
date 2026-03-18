using MedicalVisit.Application.Common.Interfaces;

namespace MedicalVisit.Infrastructure.Messaging;

public class RabbitMQEventPublisher : IEventPublisher
{
    private readonly RabbitMQPublisher _publisher;

    public RabbitMQEventPublisher(RabbitMQPublisher publisher)
    {
        _publisher = publisher;
    }

    public void Publish<T>(string exchange, string routingKey, T message)
    {
        _publisher.Publish(exchange, routingKey, message);
    }
}
