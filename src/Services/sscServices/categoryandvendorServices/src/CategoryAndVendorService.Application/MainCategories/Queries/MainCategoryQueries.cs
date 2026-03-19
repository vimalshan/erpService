using AutoMapper;
using CategoryAndVendorService.Application.DTOs;
using CategoryAndVendorService.Domain.Interfaces;
using MediatR;

namespace CategoryAndVendorService.Application.MainCategories.Queries;

public record GetAllMainCategoriesQuery : IRequest<IReadOnlyList<MainCategoryDto>>;
public record GetMainCategoryByIdQuery(long MainCatId) : IRequest<MainCategoryDto?>;

public class GetAllMainCategoriesQueryHandler : IRequestHandler<GetAllMainCategoriesQuery, IReadOnlyList<MainCategoryDto>>
{
    private readonly IMainCategoryRepository _repo;
    private readonly IMapper _mapper;

    public GetAllMainCategoriesQueryHandler(IMainCategoryRepository repo, IMapper mapper)
    { _repo = repo; _mapper = mapper; }

    public async Task<IReadOnlyList<MainCategoryDto>> Handle(GetAllMainCategoriesQuery request, CancellationToken ct)
    {
        var items = await _repo.GetAllAsync(ct);
        return items.Select(_mapper.Map<MainCategoryDto>).ToList();
    }
}

public class GetMainCategoryByIdQueryHandler : IRequestHandler<GetMainCategoryByIdQuery, MainCategoryDto?>
{
    private readonly IMainCategoryRepository _repo;
    private readonly IMapper _mapper;

    public GetMainCategoryByIdQueryHandler(IMainCategoryRepository repo, IMapper mapper)
    { _repo = repo; _mapper = mapper; }

    public async Task<MainCategoryDto?> Handle(GetMainCategoryByIdQuery request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.MainCatId, ct);
        return entity is null ? null : _mapper.Map<MainCategoryDto>(entity);
    }
}
