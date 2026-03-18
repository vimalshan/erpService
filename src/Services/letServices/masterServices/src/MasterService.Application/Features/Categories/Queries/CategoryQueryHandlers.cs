using AutoMapper;
using MediatR;
using MasterService.Application.DTOs;
using MasterService.Domain.Interfaces;

namespace MasterService.Application.Features.Categories.Queries;

public sealed class GetCategoriesQueryHandler(ICategoryRepository repository, IMapper mapper)
    : IRequestHandler<GetCategoriesQuery, IEnumerable<CategoryDto>>
{
    public async Task<IEnumerable<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var list = await repository.GetAllAsync(cancellationToken);
        return mapper.Map<IEnumerable<CategoryDto>>(list);
    }
}

public sealed class GetCategoryByCodeQueryHandler(ICategoryRepository repository, IMapper mapper)
    : IRequestHandler<GetCategoryByCodeQuery, CategoryDto?>
{
    public async Task<CategoryDto?> Handle(GetCategoryByCodeQuery request, CancellationToken cancellationToken)
    {
        var cat = await repository.GetByCodeAsync(request.CategoryCode, cancellationToken);
        return cat is null ? null : mapper.Map<CategoryDto>(cat);
    }
}
