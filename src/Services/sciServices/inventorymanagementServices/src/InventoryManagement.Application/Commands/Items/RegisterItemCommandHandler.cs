using AutoMapper;
using InventoryManagement.Application.DTOs;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Interfaces;
using MediatR;

namespace InventoryManagement.Application.Commands.Items;

public sealed class RegisterItemCommandHandler : IRequestHandler<RegisterItemCommand, ItemDto>
{
    private readonly IItemRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IMessagePublisher _publisher;

    public RegisterItemCommandHandler(
        IItemRepository repo, IUnitOfWork uow, IMapper mapper, IMessagePublisher publisher)
    {
        _repo = repo;
        _uow = uow;
        _mapper = mapper;
        _publisher = publisher;
    }

    public async Task<ItemDto> Handle(RegisterItemCommand request, CancellationToken ct)
    {
        var exists = await _repo.OracleCodeExistsAsync(request.OracleCode, ct);
        if (exists)
            throw new InvalidOperationException($"Item with Oracle code '{request.OracleCode}' already exists.");

        var entity = new ItemMaster
        {
            OracleCode = request.OracleCode,
            OracleItemId = request.OracleItemId,
            ItemName = request.ItemName,
            MainProductId = request.MainProductId,
            ItemType = request.ItemType,
            ItemUomId = request.ItemUomId,
            MainProductUomConvFactor = request.ConversionFactor,
            IsBulkSource = request.IsBulkSource ? "Y" : "N",
            IsBulkItem = request.IsBulkItem ? 'Y' : 'N',
            PackageTypeId = request.PackageTypeId,
            MaterialTaxClassId = request.MaterialTaxClassId,
            LeadTime = request.LeadTime
        };

        await _repo.AddAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);

        await _publisher.PublishAsync("inventory.item.registered", new
        {
            entity.SciItemId,
            entity.OracleCode,
            entity.ItemName
        }, ct);

        return _mapper.Map<ItemDto>(entity);
    }
}
