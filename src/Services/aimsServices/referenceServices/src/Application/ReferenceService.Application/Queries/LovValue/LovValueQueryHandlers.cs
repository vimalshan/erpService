using MediatR;
using ReferenceService.Application.DTOs;
using ReferenceService.Application.Queries.LovValue;
using ReferenceService.Domain.Interfaces;
using AutoMapper;

namespace ReferenceService.Application.Queries.LovValue;

/// <summary>
/// Handler for GetLovValueByIdQuery.
/// </summary>
public class GetLovValueByIdQueryHandlerImpl : IRequestHandler<GetLovValueByIdQuery, LovValueDto?>
{
    private readonly ILovValueRepository _repository;
    private readonly IMapper _mapper;
    
    public GetLovValueByIdQueryHandlerImpl(ILovValueRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<LovValueDto?> Handle(GetLovValueByIdQuery request, CancellationToken cancellationToken)
    {
        var lovValue = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return lovValue != null ? _mapper.Map<LovValueDto>(lovValue) : null;
    }
}

/// <summary>
/// Handler for GetLovValueByCodeQuery.
/// </summary>
public class GetLovValueByCodeQueryHandlerImpl : IRequestHandler<GetLovValueByCodeQuery, LovValueDto?>
{
    private readonly ILovValueRepository _repository;
    private readonly IMapper _mapper;
    
    public GetLovValueByCodeQueryHandlerImpl(ILovValueRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<LovValueDto?> Handle(GetLovValueByCodeQuery request, CancellationToken cancellationToken)
    {
        var lovValue = await _repository.GetByCodeAsync(request.Code, cancellationToken);
        return lovValue != null ? _mapper.Map<LovValueDto>(lovValue) : null;
    }
}
