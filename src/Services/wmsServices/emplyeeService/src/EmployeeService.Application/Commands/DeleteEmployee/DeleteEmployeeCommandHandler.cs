using MediatR;
using EmployeeService.Application.Interfaces;
using EmployeeService.Domain.Interfaces;

namespace EmployeeService.Application.Commands.DeleteEmployee;

public sealed class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, bool>
{
    private readonly IEmployeeRepository _repository;
    private readonly IMessagePublisher _messagePublisher;

    public DeleteEmployeeCommandHandler(IEmployeeRepository repository, IMessagePublisher messagePublisher)
    {
        _repository = repository;
        _messagePublisher = messagePublisher;
    }

    public async Task<bool> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _repository.GetByIdAsync(request.EmployeeId, cancellationToken);
        if (employee is null)
            return false;

        employee.Deactivate();
        await _repository.UpdateAsync(employee, cancellationToken);

        await _messagePublisher.PublishAsync(
            "employee",
            "employee.deactivated",
            new { employee.EmployeeId, Event = "EmployeeDeactivated" },
            cancellationToken);

        return true;
    }
}
