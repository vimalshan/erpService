using MediatR;
using Masters.Application.DTOs;
using Masters.Application.Interfaces;

namespace Masters.Application.Queries;

public class GetLovMasterByIdQueryHandler : IRequestHandler<GetLovMasterByIdQuery, LovMasterDto?>
{
    private readonly ILovMasterRepository _repository;

    public GetLovMasterByIdQueryHandler(ILovMasterRepository repository)
    {
        _repository = repository;
    }

    public async Task<LovMasterDto?> Handle(GetLovMasterByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.LovId, cancellationToken);
        
        return entity == null 
            ? null 
            : new LovMasterDto(entity.LovId, entity.LovType.Value, entity.LovName);
    }
}

public class GetAllLovMastersQueryHandler : IRequestHandler<GetAllLovMastersQuery, IEnumerable<LovMasterDto>>
{
    private readonly ILovMasterRepository _repository;

    public GetAllLovMastersQueryHandler(ILovMasterRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<LovMasterDto>> Handle(GetAllLovMastersQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        
        return entities.Select(e => new LovMasterDto(e.LovId, e.LovType.Value, e.LovName));
    }
}

public class GetLovMastersByTypeQueryHandler : IRequestHandler<GetLovMastersByTypeQuery, IEnumerable<LovMasterDto>>
{
    private readonly ILovMasterRepository _repository;

    public GetLovMastersByTypeQueryHandler(ILovMasterRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<LovMasterDto>> Handle(GetLovMastersByTypeQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetByTypeAsync(request.LovType, cancellationToken);
        
        return entities.Select(e => new LovMasterDto(e.LovId, e.LovType.Value, e.LovName));
    }
}
