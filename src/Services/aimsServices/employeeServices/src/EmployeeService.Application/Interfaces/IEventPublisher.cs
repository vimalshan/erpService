namespace EmployeeService.Application.Interfaces;

/// <summary>Abstraction over message broker publishing — keeps Application layer decoupled from Infrastructure.</summary>
public interface IEventPublisher
{
    Task PublishAsync<T>(T message, string topic, CancellationToken ct = default);
}
