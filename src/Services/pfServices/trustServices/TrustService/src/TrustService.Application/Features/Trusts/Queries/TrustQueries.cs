using AutoMapper;
using MediatR;
using TrustService.Application.Common.Interfaces;
using TrustService.Application.DTOs;

namespace TrustService.Application.Features.Trusts.Queries;

// --- Get Trust By Code ---
public record GetTrustByCodeQuery(string TrustCode) : IRequest<TrustMasterDto?>;

public class GetTrustByCodeQueryHandler : IRequestHandler<GetTrustByCodeQuery, TrustMasterDto?>
{
    private readonly ITrustRepository _repository;
    private readonly IMapper _mapper;

    public GetTrustByCodeQueryHandler(ITrustRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TrustMasterDto?> Handle(GetTrustByCodeQuery request, CancellationToken cancellationToken)
    {
        var trust = await _repository.GetByCodeAsync(request.TrustCode, cancellationToken);
        return trust is null ? null : _mapper.Map<TrustMasterDto>(trust);
    }
}

// --- Get All Trusts ---
public record GetAllTrustsQuery : IRequest<IReadOnlyList<TrustMasterDto>>;

public class GetAllTrustsQueryHandler : IRequestHandler<GetAllTrustsQuery, IReadOnlyList<TrustMasterDto>>
{
    private readonly ITrustRepository _repository;
    private readonly IMapper _mapper;

    public GetAllTrustsQueryHandler(ITrustRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TrustMasterDto>> Handle(GetAllTrustsQuery request, CancellationToken cancellationToken)
    {
        var trusts = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<TrustMasterDto>>(trusts);
    }
}

// --- Get Active Trusts ---
public record GetActiveTrustsQuery : IRequest<IReadOnlyList<TrustMasterDto>>;

public class GetActiveTrustsQueryHandler : IRequestHandler<GetActiveTrustsQuery, IReadOnlyList<TrustMasterDto>>
{
    private readonly ITrustRepository _repository;
    private readonly IMapper _mapper;

    public GetActiveTrustsQueryHandler(ITrustRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TrustMasterDto>> Handle(GetActiveTrustsQuery request, CancellationToken cancellationToken)
    {
        var trusts = await _repository.GetActiveAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<TrustMasterDto>>(trusts);
    }
}

// --- Get Trusts via Dapper (read-optimized) ---
public record GetTrustsByDapperQuery(string? StatusFilter = null) : IRequest<IReadOnlyList<TrustMasterDto>>;

public class GetTrustsByDapperQueryHandler : IRequestHandler<GetTrustsByDapperQuery, IReadOnlyList<TrustMasterDto>>
{
    private readonly IDapperQueryService _dapper;

    public GetTrustsByDapperQueryHandler(IDapperQueryService dapper)
    {
        _dapper = dapper;
    }

    public async Task<IReadOnlyList<TrustMasterDto>> Handle(GetTrustsByDapperQuery request, CancellationToken cancellationToken)
    {
        var sql = """
            SELECT TRUST_CODE AS TrustCode, TRUST_SHORT_NAME AS TrustShortName, TRUST_TYPE AS TrustType,
                   TRUST_START_DATE AS TrustStartDate, TRUST_CLOSURE_DATE AS TrustClosureDate,
                   TRUST_ID AS TrustId, ADDRESS_LINE1 AS AddressLine1, ADDRESS_LINE2 AS AddressLine2,
                   ADDRESS_LINE3 AS AddressLine3, CITY, STATE, PIN_CODE AS PinCode, COUNTRY,
                   PHONE_NO AS PhoneNo, FAX_NO AS FaxNo, EMAIL, TRUST_STATUS AS TrustStatus,
                   CREATED_DATE AS CreatedDate, UPDATED_DATE AS UpdatedDate,
                   REGISTRAR_NAME AS RegistrarName, REGISTRAR_PHONE AS RegistrarPhone
            FROM TRUST_MASTER
            WHERE (@StatusFilter IS NULL OR TRUST_STATUS = @StatusFilter)
            ORDER BY TRUST_CODE
            """;

        return await _dapper.QueryAsync<TrustMasterDto>(sql, new { request.StatusFilter }, cancellationToken);
    }
}
