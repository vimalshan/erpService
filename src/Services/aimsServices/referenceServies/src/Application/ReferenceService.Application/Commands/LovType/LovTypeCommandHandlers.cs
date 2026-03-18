using MediatR;
using ReferenceService.Application.Commands.LovType;
using ReferenceService.Domain.Entities;
using ReferenceService.Domain.Interfaces;

namespace ReferenceService.Application.Commands.LovType;

/// <summary>
/// Handler for CreateLovTypeCommand.
/// </summary>
public class CreateLovTypeCommandHandlerImpl : IRequestHandler<CreateLovTypeCommand, CreateLovTypeResponse>
{
    private readonly ILovTypeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateLovTypeCommandHandlerImpl(ILovTypeRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<CreateLovTypeResponse> Handle(CreateLovTypeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if type already exists
            var existing = await _repository.GetByNameAsync(request.TypeName, cancellationToken);
            if (existing != null)
                return new CreateLovTypeResponse(0, request.TypeName, false, "LOV Type already exists");
            
            // Create new entity
            var lovType = Domain.Entities.LovType.Create(
                GenerateId(),
                request.TypeName,
                request.Description,
                request.Sequence,
                request.ModifiedBy
            );
            
            await _repository.AddAsync(lovType, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return new CreateLovTypeResponse(lovType.Id, lovType.TypeName, true, "LOV Type created successfully");
        }
        catch (Exception ex)
        {
            return new CreateLovTypeResponse(0, request.TypeName, false, $"Error: {ex.Message}");
        }
    }
    
    private int GenerateId()
    {
        return (int)(DateTime.UtcNow.Ticks % int.MaxValue);
    }
}

/// <summary>
/// Handler for UpdateLovTypeCommand.
/// </summary>
public class UpdateLovTypeCommandHandlerImpl : IRequestHandler<UpdateLovTypeCommand, UpdateLovTypeResponse>
{
    private readonly ILovTypeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdateLovTypeCommandHandlerImpl(ILovTypeRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<UpdateLovTypeResponse> Handle(UpdateLovTypeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var lovType = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (lovType == null)
                return new UpdateLovTypeResponse(false, "LOV Type not found");
            
            lovType.Update(request.TypeName, request.Description, request.Sequence, request.ModifiedBy);
            await _repository.UpdateAsync(lovType, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return new UpdateLovTypeResponse(true, "LOV Type updated successfully");
        }
        catch (Exception ex)
        {
            return new UpdateLovTypeResponse(false, $"Error: {ex.Message}");
        }
    }
}

/// <summary>
/// Handler for DeactivateLovTypeCommand.
/// </summary>
public class DeactivateLovTypeCommandHandlerImpl : IRequestHandler<DeactivateLovTypeCommand, DeactivateLovTypeResponse>
{
    private readonly ILovTypeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    
    public DeactivateLovTypeCommandHandlerImpl(ILovTypeRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<DeactivateLovTypeResponse> Handle(DeactivateLovTypeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var lovType = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (lovType == null)
                return new DeactivateLovTypeResponse(false, "LOV Type not found");
            
            lovType.Deactivate(request.ModifiedBy);
            await _repository.UpdateAsync(lovType, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return new DeactivateLovTypeResponse(true, "LOV Type deactivated successfully");
        }
        catch (Exception ex)
        {
            return new DeactivateLovTypeResponse(false, $"Error: {ex.Message}");
        }
    }
}
