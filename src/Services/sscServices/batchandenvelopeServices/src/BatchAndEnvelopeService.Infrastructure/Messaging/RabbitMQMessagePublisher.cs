using BatchAndEnvelopeService.Application.Interfaces;
using Microsoft.Extensions.Options;
using BatchAndEnvelopeService.Infrastructure.Settings;

namespace BatchAndEnvelopeService.Infrastructure.Messaging;

public class RabbitMQMessagePublisher : IMessagePublisher
{
    private readonly RabbitMQPublisher _publisher;

    public RabbitMQMessagePublisher(RabbitMQPublisher publisher)
        => _publisher = publisher;

    public Task PublishAsync<T>(string exchange, string routingKey, T message) where T : class
        => _publisher.PublishAsync(exchange, routingKey, message);
}
