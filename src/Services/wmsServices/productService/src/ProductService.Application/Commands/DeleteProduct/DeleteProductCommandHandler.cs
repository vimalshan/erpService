using MediatR;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.Commands.DeleteProduct;

public sealed class DeleteProductCommandHandler(IProductRepository repo)
    : IRequestHandler<DeleteProductCommand, bool>
{
    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken ct)
    {
        var product = await repo.GetByIdAsync(request.ProductId, ct);
        if (product is null) return false;

        product.Deactivate();
        await repo.UpdateAsync(product, ct);
        return true;
    }
}
