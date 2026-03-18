namespace MedicalVisit.Application.Common.Interfaces;

public interface IEventPublisher
{
    void Publish<T>(string exchange, string routingKey, T message);
}
