using MasterDataService.Application.DTOs;
using MasterDataService.Application.Interfaces;
using MasterDataService.Domain.Interfaces;
using MediatR;

namespace MasterDataService.Application.Features.LovMaster.Commands;

public class CreateLovCommandHandler : IRequestHandler<CreateLovCommand, LovMasterDto>
{
    private readonly ILovMasterRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateLovCommandHandler(ILovMasterRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<LovMasterDto> Handle(CreateLovCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.LovMaster
        {
            LovCode = request.LovCode,
            LovDescription = request.LovDescription,
            LovValue = request.LovValue,
            LovCategory = request.LovCategory,
            LovStatus = "A"
        };
        entity.LovId = await _repository.GetNextIdAsync(cancellationToken);
        await _repository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new LovMasterDto(entity.LovId, entity.LovCode, entity.LovDescription, entity.LovValue, entity.LovCategory, entity.LovStatus);
    }
}

public class ActivateLovCommandHandler : IRequestHandler<ActivateLovCommand, bool>
{
    private readonly ILovMasterRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ActivateLovCommandHandler(ILovMasterRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(ActivateLovCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.LovId, cancellationToken);
        if (entity is null) return false;
        entity.Activate();
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class DeactivateLovCommandHandler : IRequestHandler<DeactivateLovCommand, bool>
{
    private readonly ILovMasterRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateLovCommandHandler(ILovMasterRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeactivateLovCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.LovId, cancellationToken);
        if (entity is null) return false;
        entity.Deactivate();
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
