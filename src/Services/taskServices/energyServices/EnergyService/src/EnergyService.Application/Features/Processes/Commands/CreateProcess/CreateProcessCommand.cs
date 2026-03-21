using EnergyService.Application.DTOs;
using MediatR;

namespace EnergyService.Application.Features.Processes.Commands.CreateProcess;

public record CreateProcessCommand(
    int EcProcessId,
    string EcProcessDesc,
    string EcUnitCode,
    string EcCloseFlag,
    int ModifiedBy) : IRequest<EcProcessDto>;
