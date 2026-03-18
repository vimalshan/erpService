using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Interfaces;
using MediatR;

namespace InventoryManagement.Application.Commands.Products;

public sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly IProductRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateProductCommandHandler(IProductRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task Handle(UpdateProductCommand request, CancellationToken ct)
    {
        var product = await _repo.GetByIdAsync(request.ProductId, ct)
            ?? throw new NotFoundException($"Product {request.ProductId} not found.");

        product.ProductName = request.ProductName;
        product.ProductDescription = request.ProductDescription;
        product.UnitId = request.UnitId;
        product.ModifiedBy = request.ModifiedBy;
        product.ModifiedDate = DateTime.UtcNow.ToString("O");

        await _repo.UpdateAsync(product, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
