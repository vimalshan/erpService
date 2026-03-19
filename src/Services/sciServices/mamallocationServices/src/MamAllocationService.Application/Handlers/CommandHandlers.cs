using AutoMapper;
using MamAllocationService.Application.Commands;
using MamAllocationService.Application.DTOs;
using MamAllocationService.Application.Interfaces;
using MamAllocationService.Domain.Entities;
using MamAllocationService.Domain.Events;
using MamAllocationService.Domain.Exceptions;
using MamAllocationService.Domain.Interfaces;
using MediatR;

namespace MamAllocationService.Application.Handlers;

public class CreateAllocationDetailHandler(IUnitOfWork uow, IMapper mapper, IMessagePublisher publisher)
    : IRequestHandler<CreateAllocationDetailCommand, AllocationDetailDto>
{
    public async Task<AllocationDetailDto> Handle(CreateAllocationDetailCommand request, CancellationToken ct)
    {
        var entity = mapper.Map<AllocationDetail>(request.Allocation);
        entity.AddDomainEvent(new AllocationCreatedEvent(entity.AllDate, entity.AllRm));
        await uow.AllocationDetails.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        await publisher.PublishAsync("allocation.created", new { entity.AllDate, entity.AllRm }, ct);
        return mapper.Map<AllocationDetailDto>(entity);
    }
}

public class UpdateAllocationDetailHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<UpdateAllocationDetailCommand, AllocationDetailDto>
{
    public async Task<AllocationDetailDto> Handle(UpdateAllocationDetailCommand request, CancellationToken ct)
    {
        var entity = await uow.AllocationDetails.GetByIdAsync(request.AllDate, request.AllRm, ct)
            ?? throw new AllocationNotFoundException(request.AllDate, request.AllRm);

        mapper.Map(request.Allocation, entity);
        entity.AddDomainEvent(new AllocationUpdatedEvent(entity.AllDate, entity.AllRm, "FullUpdate", 0));
        uow.AllocationDetails.Update(entity);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<AllocationDetailDto>(entity);
    }
}

public class DeleteAllocationDetailHandler(IUnitOfWork uow)
    : IRequestHandler<DeleteAllocationDetailCommand, bool>
{
    public async Task<bool> Handle(DeleteAllocationDetailCommand request, CancellationToken ct)
    {
        var entity = await uow.AllocationDetails.GetByIdAsync(request.AllDate, request.AllRm, ct);
        if (entity is null) return false;

        entity.AddDomainEvent(new AllocationDeletedEvent(entity.AllDate, entity.AllRm));
        uow.AllocationDetails.Delete(entity);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}

public class CreateArrivalDetailHandler(IUnitOfWork uow, IMapper mapper, IMessagePublisher publisher)
    : IRequestHandler<CreateArrivalDetailCommand, ArrivalDetailDto>
{
    public async Task<ArrivalDetailDto> Handle(CreateArrivalDetailCommand request, CancellationToken ct)
    {
        var entity = mapper.Map<ArrivalDetail>(request.Arrival);
        entity.AddDomainEvent(new ArrivalRecordedEvent(entity.ArrivalNo, entity.ArrivalItem, entity.ArrivalQty));
        await uow.ArrivalDetails.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        await publisher.PublishAsync("arrival.created", new { entity.ArrivalNo, entity.ArrivalItem }, ct);
        return mapper.Map<ArrivalDetailDto>(entity);
    }
}

public class CreateConsumptionDetailHandler(IUnitOfWork uow, IMapper mapper, IMessagePublisher publisher)
    : IRequestHandler<CreateConsumptionDetailCommand, ConsumptionDetailDto>
{
    public async Task<ConsumptionDetailDto> Handle(CreateConsumptionDetailCommand request, CancellationToken ct)
    {
        var entity = mapper.Map<ConsumptionDetail>(request.Consumption);
        entity.AddDomainEvent(new ConsumptionRecordedEvent(entity.ConsumptionNo, entity.ConsumptionRm, entity.ConsumptionQty));
        await uow.ConsumptionDetails.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        await publisher.PublishAsync("consumption.created", new { entity.ConsumptionNo, entity.ConsumptionRm }, ct);
        return mapper.Map<ConsumptionDetailDto>(entity);
    }
}

public class CreateDispatchDetailHandler(IUnitOfWork uow, IMapper mapper, IMessagePublisher publisher)
    : IRequestHandler<CreateDispatchDetailCommand, DispatchDetailDto>
{
    public async Task<DispatchDetailDto> Handle(CreateDispatchDetailCommand request, CancellationToken ct)
    {
        var entity = mapper.Map<DispatchDetail>(request.Dispatch);
        entity.AddDomainEvent(new DispatchRecordedEvent(entity.DispatchNo, entity.DispatchFg, entity.DispatchQty));
        await uow.DispatchDetails.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        await publisher.PublishAsync("dispatch.created", new { entity.DispatchNo, entity.DispatchFg }, ct);
        return mapper.Map<DispatchDetailDto>(entity);
    }
}

public class CreateAllocationProdDetailHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<CreateAllocationProdDetailCommand, AllocationProdDetailDto>
{
    public async Task<AllocationProdDetailDto> Handle(CreateAllocationProdDetailCommand request, CancellationToken ct)
    {
        var entity = mapper.Map<AllocationProdDetail>(request.ProdDetail);
        await uow.AllocationProdDetails.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<AllocationProdDetailDto>(entity);
    }
}

public class CreateAllocationFgHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<CreateAllocationFgCommand, AllocationFgDto>
{
    public async Task<AllocationFgDto> Handle(CreateAllocationFgCommand request, CancellationToken ct)
    {
        var entity = mapper.Map<AllocationFg>(request.AllocationFg);
        await uow.AllocationFgs.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<AllocationFgDto>(entity);
    }
}

public class CreateFgAllocationHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<CreateFgAllocationCommand, FgAllocationDto>
{
    public async Task<FgAllocationDto> Handle(CreateFgAllocationCommand request, CancellationToken ct)
    {
        var entity = mapper.Map<FgAllocation>(request.FgAllocation);
        await uow.FgAllocations.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<FgAllocationDto>(entity);
    }
}

public class CreateProductAllocationHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<CreateProductAllocationCommand, ProductAllocationDto>
{
    public async Task<ProductAllocationDto> Handle(CreateProductAllocationCommand request, CancellationToken ct)
    {
        var entity = mapper.Map<ProductAllocation>(request.ProductAllocation);
        await uow.ProductAllocations.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<ProductAllocationDto>(entity);
    }
}
