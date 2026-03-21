using AutoMapper;
using MasterDataService.Application.DTOs;
using MasterDataService.Domain.Interfaces;
using MediatR;

namespace MasterDataService.Application.Queries.TaxSlab;

public class GetAllTaxSlabsQueryHandler : IRequestHandler<GetAllTaxSlabsQuery, IReadOnlyList<TaxSlabDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllTaxSlabsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TaxSlabDto>> Handle(GetAllTaxSlabsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.TaxSlabs.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<TaxSlabDto>>(entities);
    }
}

public class GetActiveTaxSlabsQueryHandler : IRequestHandler<GetActiveTaxSlabsQuery, IReadOnlyList<TaxSlabDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetActiveTaxSlabsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TaxSlabDto>> Handle(GetActiveTaxSlabsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.TaxSlabs.GetActiveSlabsAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<TaxSlabDto>>(entities);
    }
}
