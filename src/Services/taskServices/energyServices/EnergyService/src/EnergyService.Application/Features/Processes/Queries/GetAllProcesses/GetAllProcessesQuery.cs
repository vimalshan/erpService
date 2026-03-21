using EnergyService.Application.DTOs;
using MediatR;

namespace EnergyService.Application.Features.Processes.Queries.GetAllProcesses;

public record GetAllProcessesQuery : IRequest<IReadOnlyList<EcProcessDto>>;
