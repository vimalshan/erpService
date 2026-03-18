using LovService.Application.Interfaces;
using LovService.Domain.Exceptions;
using MediatR;
using DomainItemData = LovService.Domain.Entities.ItemData;

namespace LovService.Application.Commands.ItemData;

public class CreateItemDataCommandHandler(IUnitOfWork uow)
    : IRequestHandler<CreateItemDataCommand, int>
{
    public async Task<int> Handle(CreateItemDataCommand request, CancellationToken ct)
    {
        var item = DomainItemData.Create(request.CatName, request.ItemName, request.Make, request.Uom, request.Price);
        await uow.ItemData.AddAsync(item, ct);
        await uow.SaveChangesAsync(ct);
        return item.Id;
    }
}

public class UpdateItemDataCommandHandler(IUnitOfWork uow)
    : IRequestHandler<UpdateItemDataCommand, bool>
{
    public async Task<bool> Handle(UpdateItemDataCommand request, CancellationToken ct)
    {
        var item = await uow.ItemData.GetByIdAsync(request.Id, ct)
            ?? throw new LovNotFoundException(nameof(DomainItemData), request.Id);

        item.Update(request.CatName, request.ItemName, request.Make, request.Uom, request.Price);
        uow.ItemData.Update(item);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}

public class DeleteItemDataCommandHandler(IUnitOfWork uow)
    : IRequestHandler<DeleteItemDataCommand, bool>
{
    public async Task<bool> Handle(DeleteItemDataCommand request, CancellationToken ct)
    {
        var item = await uow.ItemData.GetByIdAsync(request.Id, ct)
            ?? throw new LovNotFoundException(nameof(DomainItemData), request.Id);

        uow.ItemData.Delete(item);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}
