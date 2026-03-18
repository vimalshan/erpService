using MediatR;
using ReferenceService.Application.Commands.LovValue;
using ReferenceService.Domain.Entities;
using ReferenceService.Domain.Interfaces;

namespace ReferenceService.Application.Commands.LovValue;

/// <summary>
/// Handler for CreateLovValueCommand.
/// </summary>
public class CreateLovValueCommandHandlerImpl : IRequestHandler<CreateLovValueCommand, CreateLovValueResponse>
{
    private readonly ILovValueRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateLovValueCommandHandlerImpl(ILovValueRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<CreateLovValueResponse> Handle(CreateLovValueCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if value already exists
            var existing = await _repository.GetByCodeAsync(request.Code, cancellationToken);
            if (existing != null)
                return new CreateLovValueResponse(0, request.Code, false, "LOV Value already exists for this type");
            
            var lovValue = Domain.Entities.LovValue.Create(
                GenerateId(),
                request.TypeId,
                request.Code,
                request.Description,
                request.LongDescription,
                request.Sequence,
                request.ModifiedBy
            );
            
            await _repository.AddAsync(lovValue, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return new CreateLovValueResponse(lovValue.Id, lovValue.Code, true, "LOV Value created successfully");
        }
        catch (Exception ex)
        {
            return new CreateLovValueResponse(0, request.Code, false, $"Error: {ex.Message}");
        }
    }
    
    private int GenerateId()
    {
        return (int)(DateTime.UtcNow.Ticks % int.MaxValue);
    }
}

/// <summary>
/// Handler for UpdateLovValueCommand.
/// </summary>
public class UpdateLovValueCommandHandlerImpl : IRequestHandler<UpdateLovValueCommand, UpdateLovValueResponse>
{
    private readonly ILovValueRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdateLovValueCommandHandlerImpl(ILovValueRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<UpdateLovValueResponse> Handle(UpdateLovValueCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var lovValue = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (lovValue == null)
                return new UpdateLovValueResponse(false, "LOV Value not found");
            
            lovValue.Update(request.Description, request.LongDescription, request.Sequence, request.ModifiedBy);
            await _repository.UpdateAsync(lovValue, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return new UpdateLovValueResponse(true, "LOV Value updated successfully");
        }
        catch (Exception ex)
        {
            return new UpdateLovValueResponse(false, $"Error: {ex.Message}");
        }
    }
}

/// <summary>
/// Handler for DeactivateLovValueCommand.
/// </summary>
public class DeactivateLovValueCommandHandlerImpl : IRequestHandler<DeactivateLovValueCommand, DeactivateLovValueResponse>
{
    private readonly ILovValueRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    
    public DeactivateLovValueCommandHandlerImpl(ILovValueRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<DeactivateLovValueResponse> Handle(DeactivateLovValueCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var lovValue = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (lovValue == null)
                return new DeactivateLovValueResponse(false, "LOV Value not found");
            
            lovValue.Deactivate(request.ModifiedBy);
            await _repository.UpdateAsync(lovValue, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return new DeactivateLovValueResponse(true, "LOV Value deactivated successfully");
        }
        catch (Exception ex)
        {
            return new DeactivateLovValueResponse(false, $"Error: {ex.Message}");
        }
    }
}
