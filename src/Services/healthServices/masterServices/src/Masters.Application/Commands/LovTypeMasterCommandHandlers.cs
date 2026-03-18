using MediatR;
using Masters.Application.DTOs;
using Masters.Application.Interfaces;
using Masters.Domain.Entities;
using Masters.Domain.ValueObjects;
using Masters.Domain.Events;

namespace Masters.Application.Commands;

public class CreateLovTypeMasterCommandHandler : IRequestHandler<CreateLovTypeMasterCommand, LovTypeMasterDto>
{
    private readonly ILovTypeMasterRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateLovTypeMasterCommandHandler(
        ILovTypeMasterRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<LovTypeMasterDto> Handle(CreateLovTypeMasterCommand request, CancellationToken cancellationToken)
    {
        var lovTypeCode = LovTypeCode.Create(request.LovTypeCode);
        
        if (await _repository.ExistsAsync(lovTypeCode.Value, cancellationToken))
            throw new InvalidOperationException($"LOV Type Code '{request.LovTypeCode}' already exists.");

        var entity = new LovTypeMaster(lovTypeCode, request.LovTypeName);
        entity.AddDomainEvent(new LovTypeMasterCreatedEvent(lovTypeCode.Value, request.LovTypeName));
        
        await _repository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new LovTypeMasterDto(entity.LovTypeCode.Value, entity.LovTypeName);
    }
}

public class UpdateLovTypeMasterCommandHandler : IRequestHandler<UpdateLovTypeMasterCommand, LovTypeMasterDto>
{
    private readonly ILovTypeMasterRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateLovTypeMasterCommandHandler(
        ILovTypeMasterRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<LovTypeMasterDto> Handle(UpdateLovTypeMasterCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.LovTypeCode, cancellationToken)
            ?? throw new KeyNotFoundException($"LOV Type Code '{request.LovTypeCode}' not found.");

        entity.SetLovTypeName(request.LovTypeName);
        entity.AddDomainEvent(new LovTypeMasterUpdatedEvent(entity.LovTypeCode.Value, entity.LovTypeName));

        await _repository.UpdateAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new LovTypeMasterDto(entity.LovTypeCode.Value, entity.LovTypeName);
    }
}

public class DeleteLovTypeMasterCommandHandler : IRequestHandler<DeleteLovTypeMasterCommand, bool>
{
    private readonly ILovTypeMasterRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteLovTypeMasterCommandHandler(
        ILovTypeMasterRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteLovTypeMasterCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.LovTypeCode, cancellationToken);
        
        if (entity == null)
            return false;

        entity.AddDomainEvent(new LovTypeMasterDeletedEvent(entity.LovTypeCode.Value));

        await _repository.DeleteAsync(request.LovTypeCode, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
