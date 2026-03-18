using MediatR;
using DevelopmentService.Application.DTOs;

namespace DevelopmentService.Application.Queries.GetCompetencyIndicators;

public record GetCompetencyIndicatorsQuery(long? CompNum, string? Band) : IRequest<IEnumerable<CompetencyIndDto>>;
