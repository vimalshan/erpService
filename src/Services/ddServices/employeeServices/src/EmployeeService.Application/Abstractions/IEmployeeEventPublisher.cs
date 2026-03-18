using EmployeeService.Shared.Messaging;

namespace EmployeeService.Application.Abstractions;

public interface IEmployeeEventPublisher
{
    Task PublishAsync(EmployeeEventMessage message, CancellationToken cancellationToken = default);
}