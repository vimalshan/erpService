using MediatR;
using ReferenceService.Application.DTOs;
using ReferenceService.Application.Queries.LovType;
using ReferenceService.Domain.Interfaces;
using AutoMapper;

namespace ReferenceService.Application.Queries.LovType;

/// <summary>
/// Handler for GetAllLovTypesQuery.
/// </summary>
public class GetAllLovTypesQueryHandlerImpl : IRequestHandler<GetAllLovTypesQuery, PaginatedResponse<LovTypeDto>>
{
    private readonly ILovTypeRepository _repository;
    private readonly IMapper _mapper;
    
    public GetAllLovTypesQueryHandlerImpl(ILovTypeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<PaginatedResponse<LovTypeDto>> Handle(GetAllLovTypesQuery request, CancellationToken cancellationToken)
    {
        var allTypes = await _repository.GetAllWithValuesAsync(cancellationToken);
        
        var totalCount = allTypes.Count;
        var items = allTypes
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => _mapper.Map<LovTypeDto>(x))
            .ToList();
        
        return new PaginatedResponse<LovTypeDto>
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}

/// <summary>
/// Handler for GetLovTypeByIdQuery.
/// </summary>
public class GetLovTypeByIdQueryHandlerImpl : IRequestHandler<GetLovTypeByIdQuery, LovTypeDto?>
{
    private readonly ILovTypeRepository _repository;
    private readonly IMapper _mapper;
    
    public GetLovTypeByIdQueryHandlerImpl(ILovTypeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<LovTypeDto?> Handle(GetLovTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var lovType = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return lovType != null ? _mapper.Map<LovTypeDto>(lovType) : null;
    }
}

/// <summary>
/// Handler for GetLovTypeByNameQuery.
/// </summary>
public class GetLovTypeByNameQueryHandlerImpl : IRequestHandler<GetLovTypeByNameQuery, LovTypeDto?>
{
    private readonly ILovTypeRepository _repository;
    private readonly IMapper _mapper;
    
    public GetLovTypeByNameQueryHandlerImpl(ILovTypeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<LovTypeDto?> Handle(GetLovTypeByNameQuery request, CancellationToken cancellationToken)
    {
        var lovType = await _repository.GetByNameAsync(request.TypeName, cancellationToken);
        return lovType != null ? _mapper.Map<LovTypeDto>(lovType) : null;
    }
}

/// <summary>
/// Handler for GetLovValuesByTypeQuery.
/// </summary>
public class GetLovValuesByTypeQueryHandlerImpl : IRequestHandler<GetLovValuesByTypeQuery, List<LovValueDto>>
{
    private readonly ILovValueRepository _repository;
    private readonly IMapper _mapper;
    
    public GetLovValuesByTypeQueryHandlerImpl(ILovValueRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<List<LovValueDto>> Handle(GetLovValuesByTypeQuery request, CancellationToken cancellationToken)
    {
        var values = await _repository.GetByTypeIdAsync(request.TypeId, cancellationToken);
        return values.Select(x => _mapper.Map<LovValueDto>(x)).ToList();
    }
}
