using AutoMapper;
using MediatR;
using SupplierService.Application.DTOs;
using SupplierService.Domain.Repositories;

namespace SupplierService.Application.Features.Suppliers.Queries;

public class GetAllSuppliersQueryHandler : IRequestHandler<GetAllSuppliersQuery, IReadOnlyList<SupplierDto>>
{
    private readonly ISupplierRepository _repository;
    private readonly IMapper _mapper;

    public GetAllSuppliersQueryHandler(ISupplierRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<SupplierDto>> Handle(GetAllSuppliersQuery request, CancellationToken cancellationToken)
    {
        var suppliers = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<SupplierDto>>(suppliers);
    }
}
