namespace ApprovalService.Application.CQRS.Handlers;

using MediatR;
using ApprovalService.Application.CQRS.Commands;
using ApprovalService.Domain.Entities;
using ApprovalService.Domain.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;

/// <summary>
/// Handler for creating approval master command
/// </summary>
public class CreateApprovalMasterHandler : IRequestHandler<CreateApprovalMasterCommand, CreateApprovalMasterCommandResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateApprovalMasterHandler> _logger;

    public CreateApprovalMasterHandler(IUnitOfWork unitOfWork, ILogger<CreateApprovalMasterHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<CreateApprovalMasterCommandResult> Handle(
        CreateApprovalMasterCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Check if code already exists
            var existing = await _unitOfWork.ApprovalMasters.GetByCodeAsync(request.Code);
            if (existing != null)
            {
                throw new InvalidOperationException($"Approval master with code '{request.Code}' already exists");
            }

            var approval = ApprovalMaster.Create(
                request.Code,
                request.Name,
                request.Module,
                request.Level,
                request.UserId);

            await _unitOfWork.ApprovalMasters.AddAsync(approval);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Created approval master with ID {ApprovalId}", approval.Id);

            return new CreateApprovalMasterCommandResult
            {
                Id = approval.Id,
                Code = approval.Code
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating approval master");
            throw;
        }
    }
}

/// <summary>
/// Handler for updating approval master command
/// </summary>
public class UpdateApprovalMasterHandler : IRequestHandler<UpdateApprovalMasterCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateApprovalMasterHandler> _logger;

    public UpdateApprovalMasterHandler(IUnitOfWork unitOfWork, ILogger<UpdateApprovalMasterHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(
        UpdateApprovalMasterCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var approval = await _unitOfWork.ApprovalMasters.GetByIdAsync(request.Id);
            if (approval == null)
            {
                throw new KeyNotFoundException($"Approval master with ID {request.Id} not found");
            }

            approval.Update(request.Name, request.Level, request.UserId);
            await _unitOfWork.ApprovalMasters.UpdateAsync(approval);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Updated approval master with ID {ApprovalId}", request.Id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating approval master");
            throw;
        }
    }
}

/// <summary>
/// Handler for deactivating approval master command
/// </summary>
public class DeactivateApprovalMasterHandler : IRequestHandler<DeactivateApprovalMasterCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeactivateApprovalMasterHandler> _logger;

    public DeactivateApprovalMasterHandler(IUnitOfWork unitOfWork, ILogger<DeactivateApprovalMasterHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(
        DeactivateApprovalMasterCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var approval = await _unitOfWork.ApprovalMasters.GetByIdAsync(request.Id);
            if (approval == null)
            {
                throw new KeyNotFoundException($"Approval master with ID {request.Id} not found");
            }

            approval.Deactivate(request.UserId);
            await _unitOfWork.ApprovalMasters.UpdateAsync(approval);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Deactivated approval master with ID {ApprovalId}", request.Id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deactivating approval master");
            throw;
        }
    }
}

/// <summary>
/// Handler for activating approval master command
/// </summary>
public class ActivateApprovalMasterHandler : IRequestHandler<ActivateApprovalMasterCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ActivateApprovalMasterHandler> _logger;

    public ActivateApprovalMasterHandler(IUnitOfWork unitOfWork, ILogger<ActivateApprovalMasterHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(
        ActivateApprovalMasterCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var approval = await _unitOfWork.ApprovalMasters.GetByIdAsync(request.Id);
            if (approval == null)
            {
                throw new KeyNotFoundException($"Approval master with ID {request.Id} not found");
            }

            approval.Activate(request.UserId);
            await _unitOfWork.ApprovalMasters.UpdateAsync(approval);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Activated approval master with ID {ApprovalId}", request.Id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error activating approval master");
            throw;
        }
    }
}
