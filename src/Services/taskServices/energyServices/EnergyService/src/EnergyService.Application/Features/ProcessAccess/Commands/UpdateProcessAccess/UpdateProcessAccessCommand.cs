using EnergyService.Application.DTOs;
using MediatR;

namespace EnergyService.Application.Features.ProcessAccess.Commands.UpdateProcessAccess;

public record UpdateProcessAccessCommand(
    int ProcessId,
    int EmployeeSysId,
    DateTime StartDate,
    DateTime? CloseDate,
    int ModifiedBy) : IRequest<EcProcessAccessDto>;
