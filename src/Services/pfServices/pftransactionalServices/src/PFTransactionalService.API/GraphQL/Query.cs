using MediatR;
using PFTransactionalService.Application.DTOs;
using PFTransactionalService.Application.Queries.GetAccumulation;
using PFTransactionalService.Application.Queries.GetAccumulations;
using PFTransactionalService.Application.Queries.GetSettlements;

namespace PFTransactionalService.API.GraphQL;

public class Query
{
    public async Task<IEnumerable<PFAccumulationDto>> GetAccumulations([Service] IMediator mediator, CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetAccumulationsQuery(), cancellationToken);
    }

    public async Task<PFAccumulationDto?> GetAccumulation([Service] IMediator mediator, long empSysId, CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetAccumulationQuery(empSysId), cancellationToken);
    }

    public async Task<IEnumerable<PFSettlementDto>> GetPFSettlements([Service] IMediator mediator, CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetPFSettlementsQuery(), cancellationToken);
    }

    public async Task<IEnumerable<PFSettlementDto>> GetPFSettlementsByEmployee([Service] IMediator mediator, long empSysId, CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetPFSettlementsByEmpQuery(empSysId), cancellationToken);
    }
}
