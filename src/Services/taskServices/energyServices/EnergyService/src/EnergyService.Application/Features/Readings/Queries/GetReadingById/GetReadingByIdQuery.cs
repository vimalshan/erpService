using EnergyService.Application.DTOs;
using MediatR;

namespace EnergyService.Application.Features.Readings.Queries.GetReadingById;

public record GetReadingByIdQuery(int Id) : IRequest<EcReadingDto?>;
