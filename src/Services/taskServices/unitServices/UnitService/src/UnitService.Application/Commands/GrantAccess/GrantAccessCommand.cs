using MediatR;

namespace UnitService.Application.Commands.GrantAccess;

public record GrantAccessCommand(
    string UnitCode,
    int EmployeeSysId,
    string AccessType,
    string Module,
    int ModifiedBy) : IRequest<int>;
