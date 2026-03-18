using Dapper;
using Microsoft.Data.SqlClient;
using RiskService.Application.DTOs;
using MediatR;
using RiskService.Application.Queries.RiskType;

namespace RiskService.Infrastructure.Repositories;

public class DapperLookupQueryHandler(string connectionString)
    : IRequestHandler<GetAllRiskTypesQuery, IReadOnlyList<RiskTypeDto>>,
      IRequestHandler<GetAllRiskImpactsQuery, IReadOnlyList<RiskImpactDto>>,
      IRequestHandler<GetAllRiskProbabilitiesQuery, IReadOnlyList<RiskProbabilityDto>>,
      IRequestHandler<GetAllRiskRatingsQuery, IReadOnlyList<RiskRatingDto>>,
      IRequestHandler<GetAllRiskResponsesQuery, IReadOnlyList<RiskResponseDto>>
{
    public async Task<IReadOnlyList<RiskTypeDto>> Handle(GetAllRiskTypesQuery request, CancellationToken ct)
    {
        await using var conn = new SqlConnection(connectionString);
        var result = await conn.QueryAsync<RiskTypeDto>(
            "SELECT TYPE_ID as Id, TYPE_NAME as Name FROM RISKTYPE_MASTER");
        return result.ToList();
    }

    public async Task<IReadOnlyList<RiskImpactDto>> Handle(GetAllRiskImpactsQuery request, CancellationToken ct)
    {
        await using var conn = new SqlConnection(connectionString);
        var result = await conn.QueryAsync<RiskImpactDto>(
            "SELECT IMPACT_ID as Id, IMPACT_RANK as [Rank], IMPACT_NAME as Name FROM RISKIMPACT_MASTER ORDER BY IMPACT_RANK");
        return result.ToList();
    }

    public async Task<IReadOnlyList<RiskProbabilityDto>> Handle(GetAllRiskProbabilitiesQuery request, CancellationToken ct)
    {
        await using var conn = new SqlConnection(connectionString);
        var result = await conn.QueryAsync<RiskProbabilityDto>(
            "SELECT PROB_ID as Id, PROB_RANK as [Rank], PROB_NAME as Name, PROB_OCC as Occurrence FROM RISKPROB_MASTER ORDER BY PROB_RANK");
        return result.ToList();
    }

    public async Task<IReadOnlyList<RiskRatingDto>> Handle(GetAllRiskRatingsQuery request, CancellationToken ct)
    {
        await using var conn = new SqlConnection(connectionString);
        var result = await conn.QueryAsync<RiskRatingDto>(
            "SELECT RATING_ID as Id, RATING_RANK as [Rank], RATING_FROM as RatingFrom, RATING_TO as RatingTo, RATING_NAME as Name FROM RISKRATING_MASTER ORDER BY RATING_RANK");
        return result.ToList();
    }

    public async Task<IReadOnlyList<RiskResponseDto>> Handle(GetAllRiskResponsesQuery request, CancellationToken ct)
    {
        await using var conn = new SqlConnection(connectionString);
        var result = await conn.QueryAsync<RiskResponseDto>(
            "SELECT RESP_ID as Id, RESP_NAME as Name FROM RISKRESP_MASTER");
        return result.ToList();
    }
}
