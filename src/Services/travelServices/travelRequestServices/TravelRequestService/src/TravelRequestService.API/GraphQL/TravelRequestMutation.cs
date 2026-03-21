using MediatR;
using TravelRequestService.Application.Commands;
using TravelRequestService.Application.DTOs;

namespace TravelRequestService.API.GraphQL;

public class TravelRequestMutation
{
    public async Task<TravelRequestDto> CreateTravelRequest(
        CreateTravelRequestCommand input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(input, cancellationToken);
    }

    public async Task<bool> ApproveTravelRequest(
        ApproveTravelRequestCommand input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(input, cancellationToken);
    }

    public async Task<bool> RejectTravelRequest(
        RejectTravelRequestCommand input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(input, cancellationToken);
    }

    public async Task<bool> CancelTravelRequest(
        CancelTravelRequestCommand input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(input, cancellationToken);
    }

    public async Task<TravelAdvanceDto> AddTravelAdvance(
        AddTravelAdvanceCommand input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(input, cancellationToken);
    }
}
