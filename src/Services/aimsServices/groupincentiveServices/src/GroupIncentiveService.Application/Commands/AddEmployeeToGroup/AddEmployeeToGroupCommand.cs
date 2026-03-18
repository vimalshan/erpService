using MediatR;

namespace GroupIncentiveService.Application.Commands.AddEmployeeToGroup;

public record AddEmployeeToGroupCommand(
    int GroupId,
    long EmployeeId,
    DateTime EffDate,
    string? Role,
    long CreatedBy) : IRequest<long>;
