using MasterDataService.Application.DTOs;
using MasterDataService.Application.Interfaces;
using MediatR;

namespace MasterDataService.Application.Features.LovMaster.Queries;

public class GetAllLovQueryHandler : IRequestHandler<GetAllLovQuery, IEnumerable<LovMasterDto>>
{
    private readonly ILovMasterRepository _repository;

    public GetAllLovQueryHandler(ILovMasterRepository repository) => _repository = repository;

    public async Task<IEnumerable<LovMasterDto>> Handle(GetAllLovQuery request, CancellationToken cancellationToken)
    {
        var entities = string.IsNullOrEmpty(request.Category)
            ? await _repository.GetAllAsync(cancellationToken)
            : await _repository.GetByCategoryAsync(request.Category, cancellationToken);

        return entities.Select(e => new LovMasterDto(
            e.LovId, e.LovCode, e.LovDescription, e.LovValue, e.LovCategory, e.LovStatus));
    }
}

public class GetLovByIdQueryHandler : IRequestHandler<GetLovByIdQuery, LovMasterDto?>
{
    private readonly ILovMasterRepository _repository;

    public GetLovByIdQueryHandler(ILovMasterRepository repository) => _repository = repository;

    public async Task<LovMasterDto?> Handle(GetLovByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.LovId, cancellationToken);
        if (entity is null) return null;
        return new LovMasterDto(entity.LovId, entity.LovCode, entity.LovDescription, entity.LovValue, entity.LovCategory, entity.LovStatus);
    }
}

public class GetLovByCategoryQueryHandler : IRequestHandler<GetLovByCategoryQuery, IEnumerable<LovMasterDto>>
{
    private readonly ILovMasterRepository _repository;

    public GetLovByCategoryQueryHandler(ILovMasterRepository repository) => _repository = repository;

    public async Task<IEnumerable<LovMasterDto>> Handle(GetLovByCategoryQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetByCategoryAsync(request.Category, cancellationToken);
        return entities.Select(e => new LovMasterDto(
            e.LovId, e.LovCode, e.LovDescription, e.LovValue, e.LovCategory, e.LovStatus));
    }
}
