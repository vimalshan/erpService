using AutoMapper;
using MediatR;
using ProductService.Application.DTOs;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.Queries.GetAllCategories;

public sealed class GetAllCategoriesQueryHandler(ICategoryRepository repo, IMapper mapper)
    : IRequestHandler<GetAllCategoriesQuery, IReadOnlyList<CategoryDto>>
{
    public async Task<IReadOnlyList<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken ct)
    {
        var categories = await repo.GetAllAsync(ct);
        return mapper.Map<IReadOnlyList<CategoryDto>>(categories);
    }
}
