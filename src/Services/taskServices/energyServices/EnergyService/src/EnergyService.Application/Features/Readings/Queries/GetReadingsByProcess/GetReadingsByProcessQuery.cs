using EnergyService.Application.DTOs;
using MediatR;

namespace EnergyService.Application.Features.Readings.Queries.GetReadingsByProcess;

public record GetReadingsByProcessQuery(int ProcessId) : IRequest<IReadOnlyList<EcReadingDto>>;
