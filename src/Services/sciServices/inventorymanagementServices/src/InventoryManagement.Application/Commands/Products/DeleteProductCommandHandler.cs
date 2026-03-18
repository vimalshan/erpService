using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Interfaces;
using MediatR;

namespace InventoryManagement.Application.Commands.Products;

public sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IProductRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteProductCommandHandler(IProductRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken ct)
    {
        var exists = await _repo.ExistsAsync(request.ProductId, ct);
        if (!exists) throw new NotFoundException($"Product {request.ProductId} not found.");
        await _repo.DeleteAsync(request.ProductId, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
