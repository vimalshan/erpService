using MediatR;
using Masters.Application.DTOs;
using Masters.Application.Interfaces;
using Masters.Domain.Entities;
using Masters.Domain.ValueObjects;
using Masters.Domain.Events;

namespace Masters.Application.Commands;

public class CreateLovMasterCommandHandler : IRequestHandler<CreateLovMasterCommand, LovMasterDto>
{
    private readonly ILovMasterRepository _repository;
    private readonly ILovTypeMasterRepository _lovTypeMasterRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateLovMasterCommandHandler(
        ILovMasterRepository repository,
        ILovTypeMasterRepository lovTypeMasterRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _lovTypeMasterRepository = lovTypeMasterRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<LovMasterDto> Handle(CreateLovMasterCommand request, CancellationToken cancellationToken)
    {
        var lovTypeCode = LovTypeCode.Create(request.LovType);
        
        if (!await _lovTypeMasterRepository.ExistsAsync(lovTypeCode.Value, cancellationToken))
            throw new InvalidOperationException($"LOV Type Code '{request.LovType}' does not exist.");

        if (await _repository.ExistsAsync(request.LovId, cancellationToken))
            throw new InvalidOperationException($"LOV ID '{request.LovId}' already exists.");

        var entity = new LovMaster(lovTypeCode, request.LovId, request.LovName);
        entity.AddDomainEvent(new LovMasterCreatedEvent(request.LovId, lovTypeCode.Value, request.LovName));
        
        await _repository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new LovMasterDto(entity.LovId, entity.LovType.Value, entity.LovName);
    }
}

public class UpdateLovMasterCommandHandler : IRequestHandler<UpdateLovMasterCommand, LovMasterDto>
{
    private readonly ILovMasterRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateLovMasterCommandHandler(
        ILovMasterRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<LovMasterDto> Handle(UpdateLovMasterCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.LovId, cancellationToken)
            ?? throw new KeyNotFoundException($"LOV ID '{request.LovId}' not found.");

        entity.SetLovName(request.LovName);
        entity.AddDomainEvent(new LovMasterUpdatedEvent(entity.LovId, entity.LovName));

        await _repository.UpdateAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new LovMasterDto(entity.LovId, entity.LovType.Value, entity.LovName);
    }
}

public class DeleteLovMasterCommandHandler : IRequestHandler<DeleteLovMasterCommand, bool>
{
    private readonly ILovMasterRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteLovMasterCommandHandler(
        ILovMasterRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteLovMasterCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.LovId, cancellationToken);
        
        if (entity == null)
            return false;

        entity.AddDomainEvent(new LovMasterDeletedEvent(entity.LovId));

        await _repository.DeleteAsync(request.LovId, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
