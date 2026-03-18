using DemandManagement.Application.DTOs;
using DemandManagement.Application.Queries;
using MediatR;

namespace DemandManagement.API.GraphQL;

public class Query
{
    public async Task<IEnumerable<DemandDto>> GetDemands([Service] IMediator mediator) =>
        await mediator.Send(new GetAllDemandsQuery());

    public async Task<DemandDto?> GetDemandById(long id, [Service] IMediator mediator) =>
        await mediator.Send(new GetDemandByIdQuery(id));
}
