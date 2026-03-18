using MediatR;
using RequestServices.Application.DTOs;
using RequestServices.Application.Queries.GetPendingRequests;
using RequestServices.Application.Queries.GetRequestById;

namespace RequestServices.API.GraphQL.Queries;

public class RequestQuery
{
    [GraphQLDescription("Retrieve a training request by its ID.")]
    public async Task<RequestMainDto> GetRequest(
        [Service] IMediator mediator,
        long requestId,
        CancellationToken ct)
        => await mediator.Send(new GetRequestByIdQuery(requestId), ct);

    [GraphQLDescription("Retrieve pending training requests for a supervisor.")]
    public async Task<IEnumerable<PendingRequestDto>> GetPendingRequests(
        [Service] IMediator mediator,
        string supervisorUser,
        CancellationToken ct)
        => await mediator.Send(new GetPendingRequestsQuery(supervisorUser), ct);
}
