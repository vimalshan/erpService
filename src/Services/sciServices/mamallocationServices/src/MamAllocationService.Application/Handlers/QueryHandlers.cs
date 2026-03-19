using AutoMapper;
using MamAllocationService.Application.DTOs;
using MamAllocationService.Application.Queries;
using MamAllocationService.Domain.Interfaces;
using MediatR;

namespace MamAllocationService.Application.Handlers;

public class GetAllocationByIdHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetAllocationByIdQuery, AllocationDetailDto?>
{
    public async Task<AllocationDetailDto?> Handle(GetAllocationByIdQuery request, CancellationToken ct)
    {
        var entity = await uow.AllocationDetails.GetByIdAsync(request.AllocationDate, request.RawMaterialCode, ct);
        return entity is null ? null : mapper.Map<AllocationDetailDto>(entity);
    }
}

public class GetAllocationsByDateHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetAllocationsByDateQuery, IEnumerable<AllocationDetailDto>>
{
    public async Task<IEnumerable<AllocationDetailDto>> Handle(GetAllocationsByDateQuery request, CancellationToken ct)
    {
        var entities = await uow.AllocationDetails.GetByDateAsync(request.AllocationDate, ct);
        return mapper.Map<IEnumerable<AllocationDetailDto>>(entities);
    }
}

public class GetAllAllocationsHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetAllAllocationsQuery, IEnumerable<AllocationDetailDto>>
{
    public async Task<IEnumerable<AllocationDetailDto>> Handle(GetAllAllocationsQuery request, CancellationToken ct)
    {
        var entities = await uow.AllocationDetails.GetAllAsync(ct);
        return mapper.Map<IEnumerable<AllocationDetailDto>>(entities);
    }
}

public class GetAllocationSummaryHandler(IAllocationSummaryDapperQuery dapperQuery)
    : IRequestHandler<GetAllocationSummaryQuery, AllocationSummaryDto?>
{
    public async Task<AllocationSummaryDto?> Handle(GetAllocationSummaryQuery request, CancellationToken ct)
    {
        return await dapperQuery.ExecuteAsync(request.AllocationDate, request.RawMaterialCode, ct);
    }
}

public class GetAllocationProdDetailsHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetAllocationProdDetailsQuery, IEnumerable<AllocationProdDetailDto>>
{
    public async Task<IEnumerable<AllocationProdDetailDto>> Handle(GetAllocationProdDetailsQuery request, CancellationToken ct)
    {
        var entities = await uow.AllocationProdDetails.GetByDateAsync(request.AllocationDate, ct);
        return mapper.Map<IEnumerable<AllocationProdDetailDto>>(entities);
    }
}

public class GetAllocationFgsHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetAllocationFgsQuery, IEnumerable<AllocationFgDto>>
{
    public async Task<IEnumerable<AllocationFgDto>> Handle(GetAllocationFgsQuery request, CancellationToken ct)
    {
        var entities = await uow.AllocationFgs.GetByDateAsync(request.AllocationDate, ct);
        return mapper.Map<IEnumerable<AllocationFgDto>>(entities);
    }
}

public class GetArrivalsByItemHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetArrivalsByItemQuery, IEnumerable<ArrivalDetailDto>>
{
    public async Task<IEnumerable<ArrivalDetailDto>> Handle(GetArrivalsByItemQuery request, CancellationToken ct)
    {
        var entities = await uow.ArrivalDetails.GetByItemAsync(request.ArrivalItem, ct);
        return mapper.Map<IEnumerable<ArrivalDetailDto>>(entities);
    }
}

public class GetAllArrivalsHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetAllArrivalsQuery, IEnumerable<ArrivalDetailDto>>
{
    public async Task<IEnumerable<ArrivalDetailDto>> Handle(GetAllArrivalsQuery request, CancellationToken ct)
    {
        var entities = await uow.ArrivalDetails.GetAllAsync(ct);
        return mapper.Map<IEnumerable<ArrivalDetailDto>>(entities);
    }
}

public class GetConsumptionsByRmHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetConsumptionsByRmQuery, IEnumerable<ConsumptionDetailDto>>
{
    public async Task<IEnumerable<ConsumptionDetailDto>> Handle(GetConsumptionsByRmQuery request, CancellationToken ct)
    {
        var entities = await uow.ConsumptionDetails.GetByRmAsync(request.ConsumptionRm, ct);
        return mapper.Map<IEnumerable<ConsumptionDetailDto>>(entities);
    }
}

public class GetAllConsumptionsHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetAllConsumptionsQuery, IEnumerable<ConsumptionDetailDto>>
{
    public async Task<IEnumerable<ConsumptionDetailDto>> Handle(GetAllConsumptionsQuery request, CancellationToken ct)
    {
        var entities = await uow.ConsumptionDetails.GetAllAsync(ct);
        return mapper.Map<IEnumerable<ConsumptionDetailDto>>(entities);
    }
}

public class GetDispatchesByFgHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetDispatchesByFgQuery, IEnumerable<DispatchDetailDto>>
{
    public async Task<IEnumerable<DispatchDetailDto>> Handle(GetDispatchesByFgQuery request, CancellationToken ct)
    {
        var entities = await uow.DispatchDetails.GetByFgAsync(request.DispatchFg, ct);
        return mapper.Map<IEnumerable<DispatchDetailDto>>(entities);
    }
}

public class GetAllDispatchesHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetAllDispatchesQuery, IEnumerable<DispatchDetailDto>>
{
    public async Task<IEnumerable<DispatchDetailDto>> Handle(GetAllDispatchesQuery request, CancellationToken ct)
    {
        var entities = await uow.DispatchDetails.GetAllAsync(ct);
        return mapper.Map<IEnumerable<DispatchDetailDto>>(entities);
    }
}

public class GetAllFgAllocationsHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetAllFgAllocationsQuery, IEnumerable<FgAllocationDto>>
{
    public async Task<IEnumerable<FgAllocationDto>> Handle(GetAllFgAllocationsQuery request, CancellationToken ct)
    {
        var entities = await uow.FgAllocations.GetAllAsync(ct);
        return mapper.Map<IEnumerable<FgAllocationDto>>(entities);
    }
}

public class GetAllProductAllocationsHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetAllProductAllocationsQuery, IEnumerable<ProductAllocationDto>>
{
    public async Task<IEnumerable<ProductAllocationDto>> Handle(GetAllProductAllocationsQuery request, CancellationToken ct)
    {
        var entities = await uow.ProductAllocations.GetAllAsync(ct);
        return mapper.Map<IEnumerable<ProductAllocationDto>>(entities);
    }
}
