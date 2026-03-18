using MediatR;
using Masters.Application.DTOs;
using Masters.Application.Interfaces;

namespace Masters.Application.Queries;

public class GetLovTypeMasterByIdQueryHandler : IRequestHandler<GetLovTypeMasterByIdQuery, LovTypeMasterDto?>
{
    private readonly ILovTypeMasterRepository _repository;

    public GetLovTypeMasterByIdQueryHandler(ILovTypeMasterRepository repository)
    {
        _repository = repository;
    }

    public async Task<LovTypeMasterDto?> Handle(GetLovTypeMasterByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.LovTypeCode, cancellationToken);
        
        return entity == null 
            ? null 
            : new LovTypeMasterDto(entity.LovTypeCode.Value, entity.LovTypeName);
    }
}

public class GetAllLovTypeMastersQueryHandler : IRequestHandler<GetAllLovTypeMastersQuery, IEnumerable<LovTypeMasterDto>>
{
    private readonly ILovTypeMasterRepository _repository;

    public GetAllLovTypeMastersQueryHandler(ILovTypeMasterRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<LovTypeMasterDto>> Handle(GetAllLovTypeMastersQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        
        return entities.Select(e => new LovTypeMasterDto(e.LovTypeCode.Value, e.LovTypeName));
    }
}
