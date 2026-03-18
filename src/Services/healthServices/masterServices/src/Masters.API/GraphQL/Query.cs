using MediatR;
using Masters.Application.Queries;
using Masters.Application.DTOs;

namespace Masters.API.GraphQL;

public class Query
{
    public async Task<IEnumerable<LovTypeMasterDto>> GetLovTypeMasters(
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var query = new GetAllLovTypeMastersQuery();
        return await mediator.Send(query, cancellationToken);
    }

    public async Task<LovTypeMasterDto?> GetLovTypeMasterById(
        string lovTypeCode,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var query = new GetLovTypeMasterByIdQuery(lovTypeCode);
        return await mediator.Send(query, cancellationToken);
    }

    public async Task<IEnumerable<LovMasterDto>> GetLovMasters(
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var query = new GetAllLovMastersQuery();
        return await mediator.Send(query, cancellationToken);
    }

    public async Task<LovMasterDto?> GetLovMasterById(
        long lovId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var query = new GetLovMasterByIdQuery(lovId);
        return await mediator.Send(query, cancellationToken);
    }

    public async Task<IEnumerable<LovMasterDto>> GetLovMastersByType(
        string lovType,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var query = new GetLovMastersByTypeQuery(lovType);
        return await mediator.Send(query, cancellationToken);
    }
}
