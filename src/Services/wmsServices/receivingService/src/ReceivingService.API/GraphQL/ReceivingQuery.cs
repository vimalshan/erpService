using MediatR;
using ReceivingService.Application.DTOs;
using ReceivingService.Application.Queries.GetReceivingById;
using ReceivingService.Application.Queries.GetAllReceivings;

namespace ReceivingService.API.GraphQL;

/// <summary>Hot Chocolate GraphQL query type for Receiving.</summary>
public sealed class ReceivingQuery
{
    public async Task<ReceivingDto> GetReceivingByIdAsync(
        int id,
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(new GetReceivingByIdQuery(id), ct);

    public async Task<IEnumerable<ReceivingDto>> GetReceivingsAsync(
        int page,
        int pageSize,
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(new GetAllReceivingsQuery(page, pageSize), ct);
}
