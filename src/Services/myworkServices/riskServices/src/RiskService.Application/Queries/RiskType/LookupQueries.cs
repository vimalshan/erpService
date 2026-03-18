using MediatR;
using RiskService.Application.DTOs;

namespace RiskService.Application.Queries.RiskType;

public record GetAllRiskTypesQuery : IRequest<IReadOnlyList<RiskTypeDto>>;
public record GetAllRiskImpactsQuery : IRequest<IReadOnlyList<RiskImpactDto>>;
public record GetAllRiskProbabilitiesQuery : IRequest<IReadOnlyList<RiskProbabilityDto>>;
public record GetAllRiskRatingsQuery : IRequest<IReadOnlyList<RiskRatingDto>>;
public record GetAllRiskResponsesQuery : IRequest<IReadOnlyList<RiskResponseDto>>;
