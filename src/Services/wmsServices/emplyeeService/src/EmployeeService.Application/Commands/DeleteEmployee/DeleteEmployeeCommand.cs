using MediatR;

namespace EmployeeService.Application.Commands.DeleteEmployee;

public sealed record DeleteEmployeeCommand(int EmployeeId) : IRequest<bool>;
