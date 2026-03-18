using MediatR;
using SettlementService.Application.DTOs;
using SettlementService.Application.Queries.GetSettlement;
using SettlementService.Application.Queries.GetSettlements;
using SettlementService.Application.Queries.GetSettlementsByMember;

namespace SettlementService.API.GraphQL;

public class Query
{
    public async Task<IEnumerable<SettlementDto>> GetSettlements([Service] IMediator mediator, CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetSettlementsQuery(), cancellationToken);
    }

    public async Task<SettlementDto?> GetSettlement([Service] IMediator mediator, long settlementNumber, CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetSettlementQuery(settlementNumber), cancellationToken);
    }

    public async Task<IEnumerable<SettlementDto>> GetSettlementsByMember([Service] IMediator mediator, long memberNo, CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetSettlementsByMemberQuery(memberNo), cancellationToken);
    }
}
