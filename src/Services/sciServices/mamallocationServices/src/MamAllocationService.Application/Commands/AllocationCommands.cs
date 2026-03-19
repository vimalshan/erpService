using MamAllocationService.Application.DTOs;
using MediatR;

namespace MamAllocationService.Application.Commands;

public record CreateAllocationDetailCommand(AllocationDetailDto Allocation) : IRequest<AllocationDetailDto>;

public record UpdateAllocationDetailCommand(DateTime AllDate, int AllRm, AllocationDetailDto Allocation) : IRequest<AllocationDetailDto>;

public record DeleteAllocationDetailCommand(DateTime AllDate, int AllRm) : IRequest<bool>;

public record CreateArrivalDetailCommand(ArrivalDetailDto Arrival) : IRequest<ArrivalDetailDto>;

public record CreateConsumptionDetailCommand(ConsumptionDetailDto Consumption) : IRequest<ConsumptionDetailDto>;

public record CreateDispatchDetailCommand(DispatchDetailDto Dispatch) : IRequest<DispatchDetailDto>;

public record CreateAllocationProdDetailCommand(AllocationProdDetailDto ProdDetail) : IRequest<AllocationProdDetailDto>;

public record CreateAllocationFgCommand(AllocationFgDto AllocationFg) : IRequest<AllocationFgDto>;

public record CreateFgAllocationCommand(FgAllocationDto FgAllocation) : IRequest<FgAllocationDto>;

public record CreateProductAllocationCommand(ProductAllocationDto ProductAllocation) : IRequest<ProductAllocationDto>;
