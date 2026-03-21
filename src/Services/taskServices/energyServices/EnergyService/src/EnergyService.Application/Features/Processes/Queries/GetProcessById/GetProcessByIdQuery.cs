using EnergyService.Application.DTOs;
using MediatR;

namespace EnergyService.Application.Features.Processes.Queries.GetProcessById;

public record GetProcessByIdQuery(int Id) : IRequest<EcProcessDto?>;
