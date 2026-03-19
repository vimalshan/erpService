using MamAllocationService.Application.Commands;
using MamAllocationService.Application.DTOs;
using MamAllocationService.Application.Queries;
using MediatR;

namespace MamAllocationService.Api.GraphQL;

public class AllocationQuery
{
    public async Task<IEnumerable<AllocationDetailDto>> GetAllocations([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllAllocationsQuery(), ct);

    public async Task<AllocationDetailDto?> GetAllocationById(DateTime date, int rmCode, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllocationByIdQuery(date, rmCode), ct);

    public async Task<IEnumerable<AllocationDetailDto>> GetAllocationsByDate(DateTime date, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllocationsByDateQuery(date), ct);

    public async Task<AllocationSummaryDto?> GetAllocationSummary(DateTime date, int rmCode, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllocationSummaryQuery(date, rmCode), ct);

    public async Task<IEnumerable<ArrivalDetailDto>> GetArrivals([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllArrivalsQuery(), ct);

    public async Task<IEnumerable<ConsumptionDetailDto>> GetConsumptions([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllConsumptionsQuery(), ct);

    public async Task<IEnumerable<DispatchDetailDto>> GetDispatches([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllDispatchesQuery(), ct);

    public async Task<IEnumerable<FgAllocationDto>> GetFgAllocations([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllFgAllocationsQuery(), ct);

    public async Task<IEnumerable<ProductAllocationDto>> GetProductAllocations([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllProductAllocationsQuery(), ct);
}

public class AllocationMutation
{
    public async Task<AllocationDetailDto> CreateAllocation(AllocationDetailDto input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new CreateAllocationDetailCommand(input), ct);

    public async Task<AllocationDetailDto> UpdateAllocation(DateTime date, int rmCode, AllocationDetailDto input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new UpdateAllocationDetailCommand(date, rmCode, input), ct);

    public async Task<bool> DeleteAllocation(DateTime date, int rmCode, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new DeleteAllocationDetailCommand(date, rmCode), ct);

    public async Task<ArrivalDetailDto> CreateArrival(ArrivalDetailDto input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new CreateArrivalDetailCommand(input), ct);

    public async Task<ConsumptionDetailDto> CreateConsumption(ConsumptionDetailDto input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new CreateConsumptionDetailCommand(input), ct);

    public async Task<DispatchDetailDto> CreateDispatch(DispatchDetailDto input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new CreateDispatchDetailCommand(input), ct);
}
