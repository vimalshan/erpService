using MediatR;
using ItemMasterService.Application.CQRS.Commands;
using ItemMasterService.Application.DTOs;
using ItemMasterService.Domain.Entities;
using ItemMasterService.Domain.Exceptions;
using ItemMasterService.Domain.Interfaces;

namespace ItemMasterService.Application.CQRS.Handlers;

public class CreateCanteenItemCommandHandler : IRequestHandler<CreateCanteenItemCommand, CanteenItemMasterDto>
{
    private readonly ICanteenItemRepository _repo;

    public CreateCanteenItemCommandHandler(ICanteenItemRepository repo) => _repo = repo;

    public async Task<CanteenItemMasterDto> Handle(CreateCanteenItemCommand request, CancellationToken ct)
    {
        if (await _repo.ExistsAsync(request.CanteenUnitCode, request.ItemCode, ct))
            throw new DuplicateItemException(request.CanteenUnitCode, request.ItemCode);

        var entity = CanteenItemMaster.Create(
            request.CanteenUnitCode,
            request.ItemCode,
            request.ItemDescription,
            request.ItemType,
            request.ItemReference,
            request.EnteredBy);

        await _repo.AddAsync(entity, ct);
        await _repo.SaveChangesAsync(ct);

        return MapToDto(entity);
    }

    internal static CanteenItemMasterDto MapToDto(CanteenItemMaster e) => new(
        e.CanteenUnitCode, e.ItemCode, e.ItemDescription, e.ItemType, e.ItemReference, e.EnteredOn, e.EnteredBy);
}

public class UpdateCanteenItemCommandHandler : IRequestHandler<UpdateCanteenItemCommand, CanteenItemMasterDto>
{
    private readonly ICanteenItemRepository _repo;

    public UpdateCanteenItemCommandHandler(ICanteenItemRepository repo) => _repo = repo;

    public async Task<CanteenItemMasterDto> Handle(UpdateCanteenItemCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.CanteenUnitCode, request.ItemCode, ct)
            ?? throw new ItemNotFoundException(request.CanteenUnitCode, request.ItemCode);

        entity.Update(request.ItemDescription, request.ItemType, request.ItemReference);
        _repo.Update(entity);
        await _repo.SaveChangesAsync(ct);

        return CreateCanteenItemCommandHandler.MapToDto(entity);
    }
}

public class DeleteCanteenItemCommandHandler : IRequestHandler<DeleteCanteenItemCommand, bool>
{
    private readonly ICanteenItemRepository _repo;

    public DeleteCanteenItemCommandHandler(ICanteenItemRepository repo) => _repo = repo;

    public async Task<bool> Handle(DeleteCanteenItemCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.CanteenUnitCode, request.ItemCode, ct)
            ?? throw new ItemNotFoundException(request.CanteenUnitCode, request.ItemCode);

        _repo.Delete(entity);
        await _repo.SaveChangesAsync(ct);
        return true;
    }
}

public class CreateItemPriceCommandHandler : IRequestHandler<CreateItemPriceCommand, CanteenItemPriceMasterDto>
{
    private readonly ICanteenItemPriceRepository _repo;

    public CreateItemPriceCommandHandler(ICanteenItemPriceRepository repo) => _repo = repo;

    public async Task<CanteenItemPriceMasterDto> Handle(CreateItemPriceCommand request, CancellationToken ct)
    {
        var entity = CanteenItemPriceMaster.Create(
            request.CanteenUnitCode,
            request.ItemCode,
            request.EmployeeContribution,
            request.EmployerContribution,
            request.EffectiveDate,
            request.EnteredBy);

        await _repo.AddAsync(entity, ct);
        await _repo.SaveChangesAsync(ct);
        return MapPriceDto(entity);
    }

    internal static CanteenItemPriceMasterDto MapPriceDto(CanteenItemPriceMaster e) => new(
        e.CanteenUnitCode, e.ItemCode, e.EmployeeContribution, e.EmployerContribution,
        e.EffectiveDate, e.ClosureDate, e.EnteredOn, e.EnteredBy);
}

public class CloseItemPriceCommandHandler : IRequestHandler<CloseItemPriceCommand, bool>
{
    private readonly ICanteenItemPriceRepository _repo;

    public CloseItemPriceCommandHandler(ICanteenItemPriceRepository repo) => _repo = repo;

    public async Task<bool> Handle(CloseItemPriceCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetActiveAsync(request.CanteenUnitCode, request.ItemCode, ct)
            ?? throw new DomainException($"No active price found for item {request.ItemCode}.");

        entity.Close(request.ClosureDate);
        _repo.Update(entity);
        await _repo.SaveChangesAsync(ct);
        return true;
    }
}

public class CreateGradeItemPriceCommandHandler : IRequestHandler<CreateGradeItemPriceCommand, CanteenGradeItemPriceDto>
{
    private readonly ICanteenGradeItemPriceRepository _repo;

    public CreateGradeItemPriceCommandHandler(ICanteenGradeItemPriceRepository repo) => _repo = repo;

    public async Task<CanteenGradeItemPriceDto> Handle(CreateGradeItemPriceCommand request, CancellationToken ct)
    {
        var entity = CanteenGradeItemPrice.Create(
            request.CanteenUnitCode, request.ItemCode, request.EmployeeContribution,
            request.EmployerContribution, request.EffectiveDate, request.ClosureDate,
            request.EnteredBy, request.GradeType);

        await _repo.AddAsync(entity, ct);
        await _repo.SaveChangesAsync(ct);
        return MapGradeDto(entity);
    }

    internal static CanteenGradeItemPriceDto MapGradeDto(CanteenGradeItemPrice e) => new(
        e.CanteenUnitCode, e.ItemCode, e.EmployeeContribution, e.EmployerContribution,
        e.EffectiveDate, e.ClosureDate, e.EnteredOn, e.EnteredBy, e.GradeType);
}

public class UpdateGradeItemPriceCommandHandler : IRequestHandler<UpdateGradeItemPriceCommand, CanteenGradeItemPriceDto>
{
    private readonly ICanteenGradeItemPriceRepository _repo;

    public UpdateGradeItemPriceCommandHandler(ICanteenGradeItemPriceRepository repo) => _repo = repo;

    public async Task<CanteenGradeItemPriceDto> Handle(UpdateGradeItemPriceCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByUnitCodeAsync(request.CanteenUnitCode, ct)
            ?? throw new DomainException($"Grade item price for unit {request.CanteenUnitCode} not found.");

        entity.Update(request.EmployeeContribution, request.EmployerContribution, request.ClosureDate);
        _repo.Update(entity);
        await _repo.SaveChangesAsync(ct);
        return CreateGradeItemPriceCommandHandler.MapGradeDto(entity);
    }
}
