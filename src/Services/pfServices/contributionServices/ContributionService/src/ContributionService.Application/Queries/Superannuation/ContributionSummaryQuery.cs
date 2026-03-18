using AutoMapper;
using ContributionService.Application.DTOs;
using ContributionService.Application.Interfaces;
using MediatR;

namespace ContributionService.Application.Queries.Superannuation;

public record GetContributionSummaryQuery(DateTime StartDate, DateTime EndDate)
    : IRequest<IReadOnlyList<ContributionSummaryDto>>;

public class GetContributionSummaryHandler(IDapperQueryService dapper, IMapper mapper)
    : IRequestHandler<GetContributionSummaryQuery, IReadOnlyList<ContributionSummaryDto>>
{
    public async Task<IReadOnlyList<ContributionSummaryDto>> Handle(GetContributionSummaryQuery request, CancellationToken ct)
    {
        const string sql = """
            SELECT 
                cm.CONTRIBUTION_BATCH_NO AS ContributionBatchNo,
                cm.CONTRIBUTION_TRUST_CODE AS TrustCode,
                cm.CONTRIBUTION_PAYUNIT_CODE AS PayunitCode,
                cm.CONTRIBUTION_STATUS AS Status,
                COUNT(DISTINCT cd.CONTRIBUTION_MEMBER_NO) AS MemberCount,
                ISNULL(SUM(cd.CONTRIBUTION_EE_AMOUNT), 0) AS TotalEeContribution,
                ISNULL(SUM(cd.CONTRIBUTION_ER_AMOUNT), 0) AS TotalErContribution,
                ISNULL(SUM(cd.CONTRIBUTION_EE_AMOUNT) + SUM(cd.CONTRIBUTION_ER_AMOUNT), 0) AS TotalContribution
            FROM CONTRIBUTION_MAIN cm
            LEFT JOIN CONTRIBUTION_DETAILS cd ON cm.CONTRIBUTION_BATCH_NO = cd.CONTRIBUTION_BATCH_NO
            WHERE cm.CONTRIBUTION_PAY_MONTHSTART >= @StartDate
              AND cm.CONTRIBUTION_PAY_MONTHEND <= @EndDate
            GROUP BY cm.CONTRIBUTION_BATCH_NO, cm.CONTRIBUTION_TRUST_CODE,
                     cm.CONTRIBUTION_PAYUNIT_CODE, cm.CONTRIBUTION_STATUS
            ORDER BY cm.CONTRIBUTION_BATCH_NO DESC
            """;

        var results = await dapper.QueryAsync<ContributionSummaryDto>(
            sql, new { request.StartDate, request.EndDate }, ct);
        return results;
    }
}
