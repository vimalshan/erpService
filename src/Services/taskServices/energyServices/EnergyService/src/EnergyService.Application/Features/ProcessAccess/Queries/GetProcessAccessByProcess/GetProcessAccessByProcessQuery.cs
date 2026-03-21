using EnergyService.Application.DTOs;
using MediatR;

namespace EnergyService.Application.Features.ProcessAccess.Queries.GetProcessAccessByProcess;

public record GetProcessAccessByProcessQuery(int ProcessId) : IRequest<IReadOnlyList<EcProcessAccessDto>>;
