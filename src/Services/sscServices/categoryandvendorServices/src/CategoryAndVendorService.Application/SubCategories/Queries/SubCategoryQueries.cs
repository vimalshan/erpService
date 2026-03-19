using AutoMapper;
using CategoryAndVendorService.Application.DTOs;
using CategoryAndVendorService.Domain.Interfaces;
using MediatR;

namespace CategoryAndVendorService.Application.SubCategories.Queries;

public record GetAllSubCategoriesQuery : IRequest<IReadOnlyList<SubCategoryDto>>;
public record GetSubCategoryByIdQuery(long SubCatId) : IRequest<SubCategoryDto?>;
public record GetSubCategoriesByMainCategoryQuery(long MainCatId) : IRequest<IReadOnlyList<SubCategoryDto>>;

public class GetAllSubCategoriesQueryHandler : IRequestHandler<GetAllSubCategoriesQuery, IReadOnlyList<SubCategoryDto>>
{
    private readonly ISubCategoryRepository _repo;
    private readonly IMapper _mapper;
    public GetAllSubCategoriesQueryHandler(ISubCategoryRepository repo, IMapper mapper)
    { _repo = repo; _mapper = mapper; }

    public async Task<IReadOnlyList<SubCategoryDto>> Handle(GetAllSubCategoriesQuery request, CancellationToken ct)
    {
        var items = await _repo.GetAllAsync(ct);
        return items.Select(_mapper.Map<SubCategoryDto>).ToList();
    }
}

public class GetSubCategoryByIdQueryHandler : IRequestHandler<GetSubCategoryByIdQuery, SubCategoryDto?>
{
    private readonly ISubCategoryRepository _repo;
    private readonly IMapper _mapper;
    public GetSubCategoryByIdQueryHandler(ISubCategoryRepository repo, IMapper mapper)
    { _repo = repo; _mapper = mapper; }

    public async Task<SubCategoryDto?> Handle(GetSubCategoryByIdQuery request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.SubCatId, ct);
        return entity is null ? null : _mapper.Map<SubCategoryDto>(entity);
    }
}

public class GetSubCategoriesByMainCategoryQueryHandler : IRequestHandler<GetSubCategoriesByMainCategoryQuery, IReadOnlyList<SubCategoryDto>>
{
    private readonly ISubCategoryRepository _repo;
    private readonly IMapper _mapper;
    public GetSubCategoriesByMainCategoryQueryHandler(ISubCategoryRepository repo, IMapper mapper)
    { _repo = repo; _mapper = mapper; }

    public async Task<IReadOnlyList<SubCategoryDto>> Handle(GetSubCategoriesByMainCategoryQuery request, CancellationToken ct)
    {
        var items = await _repo.GetByMainCategoryIdAsync(request.MainCatId, ct);
        return items.Select(_mapper.Map<SubCategoryDto>).ToList();
    }
}
