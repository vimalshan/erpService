using MamAllocationService.Application.DTOs;
using MediatR;

namespace MamAllocationService.Application.Queries;

public record GetAllocationByIdQuery(DateTime AllocationDate, int RawMaterialCode) : IRequest<AllocationDetailDto?>;

public record GetAllocationsByDateQuery(DateTime AllocationDate) : IRequest<IEnumerable<AllocationDetailDto>>;

public record GetAllAllocationsQuery : IRequest<IEnumerable<AllocationDetailDto>>;

public record GetAllocationSummaryQuery(DateTime AllocationDate, int RawMaterialCode) : IRequest<AllocationSummaryDto?>;

public record GetAllocationProdDetailsQuery(DateTime AllocationDate) : IRequest<IEnumerable<AllocationProdDetailDto>>;

public record GetAllocationFgsQuery(DateTime AllocationDate) : IRequest<IEnumerable<AllocationFgDto>>;

public record GetArrivalsByItemQuery(int ArrivalItem) : IRequest<IEnumerable<ArrivalDetailDto>>;

public record GetAllArrivalsQuery : IRequest<IEnumerable<ArrivalDetailDto>>;

public record GetConsumptionsByRmQuery(int ConsumptionRm) : IRequest<IEnumerable<ConsumptionDetailDto>>;

public record GetAllConsumptionsQuery : IRequest<IEnumerable<ConsumptionDetailDto>>;

public record GetDispatchesByFgQuery(int DispatchFg) : IRequest<IEnumerable<DispatchDetailDto>>;

public record GetAllDispatchesQuery : IRequest<IEnumerable<DispatchDetailDto>>;

public record GetAllFgAllocationsQuery : IRequest<IEnumerable<FgAllocationDto>>;

public record GetAllProductAllocationsQuery : IRequest<IEnumerable<ProductAllocationDto>>;
