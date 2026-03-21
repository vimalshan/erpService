using MediatR;
using TravelRequestService.Application.DTOs;
using TravelRequestService.Application.Queries;

namespace TravelRequestService.API.GraphQL;

public class TravelRequestQuery
{
    public async Task<IReadOnlyList<TravelRequestDto>> GetTravelRequests(
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetAllTravelRequestsQuery(), cancellationToken);
    }

    public async Task<TravelRequestDto?> GetTravelRequestById(
        long planNumber,
        string companyCode,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetTravelRequestByIdQuery(planNumber, companyCode), cancellationToken);
    }

    public async Task<IReadOnlyList<TravelRequestDto>> GetTravelRequestsByUser(
        long userNumber,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetTravelRequestsByUserQuery(userNumber), cancellationToken);
    }

    public async Task<IReadOnlyList<DashTourPlanDto>> GetDashTourPlans(
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetDashTourPlanQuery(), cancellationToken);
    }
}
