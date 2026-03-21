using AutoMapper;
using MediatR;
using ProductService.Application.DTOs;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.Queries.GetCategoryById;

public sealed class GetCategoryByIdQueryHandler(ICategoryRepository repo, IMapper mapper)
    : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
{
    public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken ct)
    {
        var category = await repo.GetByIdAsync(request.CategoryId, ct);
        return category is null ? null : mapper.Map<CategoryDto>(category);
    }
}
