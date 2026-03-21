using EnergyService.Application.DTOs;
using MediatR;

namespace EnergyService.Application.Features.Processes.Commands.UpdateProcess;

public record UpdateProcessCommand(
    int EcProcessId,
    string EcProcessDesc,
    string EcUnitCode,
    string EcCloseFlag,
    int ModifiedBy) : IRequest<EcProcessDto>;
