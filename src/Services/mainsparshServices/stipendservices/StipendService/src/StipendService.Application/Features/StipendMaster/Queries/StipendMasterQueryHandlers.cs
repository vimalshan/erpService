using AutoMapper;
using MediatR;
using StipendService.Application.DTOs;
using StipendService.Domain.Interfaces;

namespace StipendService.Application.Features.StipendMaster.Queries;

public class GetStipendMasterByIdQueryHandler : IRequestHandler<GetStipendMasterByIdQuery, StipendMasterDto?>
{
    private readonly IStipendMasterRepository _repository;
    private readonly IMapper _mapper;

    public GetStipendMasterByIdQueryHandler(IStipendMasterRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<StipendMasterDto?> Handle(GetStipendMasterByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.StipendId, cancellationToken);
        return entity is null ? null : _mapper.Map<StipendMasterDto>(entity);
    }
}

public class GetAllStipendMastersQueryHandler : IRequestHandler<GetAllStipendMastersQuery, IEnumerable<StipendMasterDto>>
{
    private readonly IStipendMasterRepository _repository;
    private readonly IMapper _mapper;

    public GetAllStipendMastersQueryHandler(IStipendMasterRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StipendMasterDto>> Handle(GetAllStipendMastersQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<StipendMasterDto>>(entities);
    }
}

public class GetActiveStipendByCategoryQueryHandler : IRequestHandler<GetActiveStipendByCategoryQuery, StipendMasterDto?>
{
    private readonly IStipendMasterRepository _repository;
    private readonly IMapper _mapper;

    public GetActiveStipendByCategoryQueryHandler(IStipendMasterRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<StipendMasterDto?> Handle(GetActiveStipendByCategoryQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetActiveByCategory(request.ResearchCategoryId, request.SrfRankId, cancellationToken);
        return entity is null ? null : _mapper.Map<StipendMasterDto>(entity);
    }
}
