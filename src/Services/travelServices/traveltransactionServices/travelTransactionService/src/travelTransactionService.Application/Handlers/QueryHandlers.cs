using AutoMapper;
using MediatR;
using travelTransactionService.Application.DTOs;
using travelTransactionService.Application.Interfaces;
using travelTransactionService.Domain.Interfaces;

namespace travelTransactionService.Application.Handlers;

public class GetAllVendorsQueryHandler : IRequestHandler<Queries.GetAllVendorsQuery, IReadOnlyList<VendorMasterDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllVendorsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<VendorMasterDto>> Handle(Queries.GetAllVendorsQuery request, CancellationToken cancellationToken)
    {
        var vendors = await _unitOfWork.Vendors.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<VendorMasterDto>>(vendors);
    }
}

public class GetVendorByIdQueryHandler : IRequestHandler<Queries.GetVendorByIdQuery, VendorMasterDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetVendorByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<VendorMasterDto?> Handle(Queries.GetVendorByIdQuery request, CancellationToken cancellationToken)
    {
        var vendor = await _unitOfWork.Vendors.GetByIdAsync(request.VendorId, cancellationToken);
        return vendor is not null ? _mapper.Map<VendorMasterDto>(vendor) : null;
    }
}

public class GetVendorsByCategoryQueryHandler : IRequestHandler<Queries.GetVendorsByCategoryQuery, IReadOnlyList<VendorMasterDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetVendorsByCategoryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<VendorMasterDto>> Handle(Queries.GetVendorsByCategoryQuery request, CancellationToken cancellationToken)
    {
        var vendors = await _unitOfWork.Vendors.GetByCategoryAsync(request.CategoryType, cancellationToken);
        return _mapper.Map<IReadOnlyList<VendorMasterDto>>(vendors);
    }
}

public class GetAllTaxMastersQueryHandler : IRequestHandler<Queries.GetAllTaxMastersQuery, IReadOnlyList<TaxMasterDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllTaxMastersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TaxMasterDto>> Handle(Queries.GetAllTaxMastersQuery request, CancellationToken cancellationToken)
    {
        var taxes = await _unitOfWork.TaxMasters.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<TaxMasterDto>>(taxes);
    }
}

public class GetTaxMasterByTypeQueryHandler : IRequestHandler<Queries.GetTaxMasterByTypeQuery, TaxMasterDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTaxMasterByTypeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TaxMasterDto?> Handle(Queries.GetTaxMasterByTypeQuery request, CancellationToken cancellationToken)
    {
        var tax = await _unitOfWork.TaxMasters.GetByTypeAsync(request.TaxType, cancellationToken);
        return tax is not null ? _mapper.Map<TaxMasterDto>(tax) : null;
    }
}

public class GetTaxMastersByVendorQueryHandler : IRequestHandler<Queries.GetTaxMastersByVendorQuery, IReadOnlyList<TaxMasterDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTaxMastersByVendorQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TaxMasterDto>> Handle(Queries.GetTaxMastersByVendorQuery request, CancellationToken cancellationToken)
    {
        var taxes = await _unitOfWork.TaxMasters.GetByVendorAsync(request.VendorId, cancellationToken);
        return _mapper.Map<IReadOnlyList<TaxMasterDto>>(taxes);
    }
}

public class GetAllJaiInterfaceLinesQueryHandler : IRequestHandler<Queries.GetAllJaiInterfaceLinesQuery, IReadOnlyList<JaiInterfaceLineDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllJaiInterfaceLinesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<JaiInterfaceLineDto>> Handle(Queries.GetAllJaiInterfaceLinesQuery request, CancellationToken cancellationToken)
    {
        var lines = await _unitOfWork.JaiInterfaceLines.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<JaiInterfaceLineDto>>(lines);
    }
}

public class GetJaiInterfaceLineByIdQueryHandler : IRequestHandler<Queries.GetJaiInterfaceLineByIdQuery, JaiInterfaceLineDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetJaiInterfaceLineByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<JaiInterfaceLineDto?> Handle(Queries.GetJaiInterfaceLineByIdQuery request, CancellationToken cancellationToken)
    {
        var line = await _unitOfWork.JaiInterfaceLines.GetByIdAsync(request.InterfaceLineId, cancellationToken);
        return line is not null ? _mapper.Map<JaiInterfaceLineDto>(line) : null;
    }
}

public class GetJaiInterfaceLinesByBatchQueryHandler : IRequestHandler<Queries.GetJaiInterfaceLinesByBatchQuery, IReadOnlyList<JaiInterfaceLineDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetJaiInterfaceLinesByBatchQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<JaiInterfaceLineDto>> Handle(Queries.GetJaiInterfaceLinesByBatchQuery request, CancellationToken cancellationToken)
    {
        var lines = await _unitOfWork.JaiInterfaceLines.GetByBatchIdAsync(request.BatchId, cancellationToken);
        return _mapper.Map<IReadOnlyList<JaiInterfaceLineDto>>(lines);
    }
}

// Dapper-based query handlers
public class GetAllAccountMastersQueryHandler : IRequestHandler<Queries.GetAllAccountMastersQuery, IReadOnlyList<AccountMasterDto>>
{
    private readonly IDapperQueryService _dapper;

    public GetAllAccountMastersQueryHandler(IDapperQueryService dapper) => _dapper = dapper;

    public async Task<IReadOnlyList<AccountMasterDto>> Handle(Queries.GetAllAccountMastersQuery request, CancellationToken cancellationToken)
        => await _dapper.GetAllAccountMastersAsync(cancellationToken);
}

public class GetAllGlCodeCombinationsQueryHandler : IRequestHandler<Queries.GetAllGlCodeCombinationsQuery, IReadOnlyList<GlCodeCombinationDto>>
{
    private readonly IDapperQueryService _dapper;

    public GetAllGlCodeCombinationsQueryHandler(IDapperQueryService dapper) => _dapper = dapper;

    public async Task<IReadOnlyList<GlCodeCombinationDto>> Handle(Queries.GetAllGlCodeCombinationsQuery request, CancellationToken cancellationToken)
        => await _dapper.GetAllGlCodeCombinationsAsync(cancellationToken);
}

public class GetAllJvInterfacesQueryHandler : IRequestHandler<Queries.GetAllJvInterfacesQuery, IReadOnlyList<JvInterfaceDto>>
{
    private readonly IDapperQueryService _dapper;

    public GetAllJvInterfacesQueryHandler(IDapperQueryService dapper) => _dapper = dapper;

    public async Task<IReadOnlyList<JvInterfaceDto>> Handle(Queries.GetAllJvInterfacesQuery request, CancellationToken cancellationToken)
        => await _dapper.GetAllJvInterfacesAsync(cancellationToken);
}

public class GetAllJvMissingCombiCodesQueryHandler : IRequestHandler<Queries.GetAllJvMissingCombiCodesQuery, IReadOnlyList<JvMissingCombiCodeDto>>
{
    private readonly IDapperQueryService _dapper;

    public GetAllJvMissingCombiCodesQueryHandler(IDapperQueryService dapper) => _dapper = dapper;

    public async Task<IReadOnlyList<JvMissingCombiCodeDto>> Handle(Queries.GetAllJvMissingCombiCodesQuery request, CancellationToken cancellationToken)
        => await _dapper.GetAllJvMissingCombiCodesAsync(cancellationToken);
}

public class GetAllBatchSubBreakupsQueryHandler : IRequestHandler<Queries.GetAllBatchSubBreakupsQuery, IReadOnlyList<BatchSubBreakupDto>>
{
    private readonly IDapperQueryService _dapper;

    public GetAllBatchSubBreakupsQueryHandler(IDapperQueryService dapper) => _dapper = dapper;

    public async Task<IReadOnlyList<BatchSubBreakupDto>> Handle(Queries.GetAllBatchSubBreakupsQuery request, CancellationToken cancellationToken)
        => await _dapper.GetAllBatchSubBreakupsAsync(cancellationToken);
}

public class GetAllTravelApParamsQueryHandler : IRequestHandler<Queries.GetAllTravelApParamsQuery, IReadOnlyList<TravelApParamsDto>>
{
    private readonly IDapperQueryService _dapper;

    public GetAllTravelApParamsQueryHandler(IDapperQueryService dapper) => _dapper = dapper;

    public async Task<IReadOnlyList<TravelApParamsDto>> Handle(Queries.GetAllTravelApParamsQuery request, CancellationToken cancellationToken)
        => await _dapper.GetAllTravelApParamsAsync(cancellationToken);
}

public class GetTravelApParamsByIdQueryHandler : IRequestHandler<Queries.GetTravelApParamsByIdQuery, TravelApParamsDto?>
{
    private readonly IDapperQueryService _dapper;

    public GetTravelApParamsByIdQueryHandler(IDapperQueryService dapper) => _dapper = dapper;

    public async Task<TravelApParamsDto?> Handle(Queries.GetTravelApParamsByIdQuery request, CancellationToken cancellationToken)
        => await _dapper.GetTravelApParamsByIdAsync(request.ApUnitId, cancellationToken);
}

public class GetAllSourceHistoryQueryHandler : IRequestHandler<Queries.GetAllSourceHistoryQuery, IReadOnlyList<SourceHistoryDto>>
{
    private readonly IDapperQueryService _dapper;

    public GetAllSourceHistoryQueryHandler(IDapperQueryService dapper) => _dapper = dapper;

    public async Task<IReadOnlyList<SourceHistoryDto>> Handle(Queries.GetAllSourceHistoryQuery request, CancellationToken cancellationToken)
        => await _dapper.GetAllSourceHistoryAsync(cancellationToken);
}
