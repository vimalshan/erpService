using EmployeeService.Application.Interfaces;
using EmployeeService.Infrastructure.Messaging;

namespace EmployeeService.Infrastructure.Services;

/// <summary>Bridges the Application IEventPublisher abstraction to RabbitMQ.</summary>
public sealed class RabbitMqEventPublisher : IEventPublisher
{
    private readonly RabbitMqPublisher _publisher;

    public RabbitMqEventPublisher(RabbitMqPublisher publisher) => _publisher = publisher;

    public Task PublishAsync<T>(T message, string topic, CancellationToken ct = default) =>
        _publisher.PublishAsync(message, "employee.events", topic, ct);
}
