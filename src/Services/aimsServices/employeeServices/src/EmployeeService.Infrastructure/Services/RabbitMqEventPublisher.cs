using EmployeeService.Application.Interfaces;
using EmployeeService.Infrastructure.Messaging;
using Microsoft.Extensions.Configuration;

namespace EmployeeService.Infrastructure.Services;

/// <summary>Bridges the Application IEventPublisher abstraction to RabbitMQ.</summary>
public sealed class RabbitMqEventPublisher : IEventPublisher
{
    private readonly RabbitMqPublisher _publisher;
    private readonly string _exchangeName;

    public RabbitMqEventPublisher(RabbitMqPublisher publisher, IConfiguration config)
    {
        _publisher = publisher;
        _exchangeName = config["RabbitMQ:ExchangeName"] ?? "employee.events";
    }

    public Task PublishAsync<T>(T message, string topic, CancellationToken ct = default) =>
        _publisher.PublishAsync(message, _exchangeName, topic, ct);
}
