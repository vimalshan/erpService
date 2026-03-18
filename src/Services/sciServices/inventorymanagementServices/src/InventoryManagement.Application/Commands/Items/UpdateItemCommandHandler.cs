using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Interfaces;
using MediatR;

namespace InventoryManagement.Application.Commands.Items;

public sealed class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand>
{
    private readonly IItemRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateItemCommandHandler(IItemRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task Handle(UpdateItemCommand request, CancellationToken ct)
    {
        var item = await _repo.GetByIdAsync(request.SciItemId, ct)
            ?? throw new NotFoundException($"Item {request.SciItemId} not found.");

        item.ItemName = request.ItemName;
        item.ItemType = request.ItemType;
        item.ItemUomId = request.ItemUomId;
        item.LeadTime = request.LeadTime;
        item.ModifiedBy = request.ModifiedBy;
        item.ModifiedDate = DateTime.UtcNow.ToString("O");

        await _repo.UpdateAsync(item, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
