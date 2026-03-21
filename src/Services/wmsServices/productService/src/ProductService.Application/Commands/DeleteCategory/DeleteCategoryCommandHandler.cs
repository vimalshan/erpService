using MediatR;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.Commands.DeleteCategory;

public sealed class DeleteCategoryCommandHandler(ICategoryRepository repo)
    : IRequestHandler<DeleteCategoryCommand, bool>
{
    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken ct)
    {
        var category = await repo.GetByIdAsync(request.CategoryId, ct);
        if (category is null) return false;

        await repo.DeleteAsync(category, ct);
        return true;
    }
}
