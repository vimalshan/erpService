using AutoMapper;
using MediatR;
using SupplierService.Application.DTOs;
using SupplierService.Domain.Repositories;

namespace SupplierService.Application.Features.Suppliers.Queries;

public class GetSuppliersPagedQueryHandler : IRequestHandler<GetSuppliersPagedQuery, PagedResultDto<SupplierDto>>
{
    private readonly ISupplierRepository _repository;
    private readonly IMapper _mapper;

    public GetSuppliersPagedQueryHandler(ISupplierRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PagedResultDto<SupplierDto>> Handle(GetSuppliersPagedQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _repository.GetPagedAsync(request.Page, request.PageSize, request.Search, cancellationToken);

        return new PagedResultDto<SupplierDto>
        {
            Items = _mapper.Map<IReadOnlyList<SupplierDto>>(items),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
