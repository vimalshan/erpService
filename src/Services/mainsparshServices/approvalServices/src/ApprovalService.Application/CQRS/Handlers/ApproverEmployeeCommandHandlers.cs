namespace ApprovalService.Application.CQRS.Handlers;

using MediatR;
using ApprovalService.Application.CQRS.Commands;
using ApprovalService.Domain.Entities;
using ApprovalService.Domain.Interfaces;
using Microsoft.Extensions.Logging;

/// <summary>
/// Handler for creating approver employee command
/// </summary>
public class CreateApproverEmployeeHandler : IRequestHandler<CreateApproverEmployeeCommand, CreateApproverEmployeeCommandResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateApproverEmployeeHandler> _logger;

    public CreateApproverEmployeeHandler(IUnitOfWork unitOfWork, ILogger<CreateApproverEmployeeHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<CreateApproverEmployeeCommandResult> Handle(
        CreateApproverEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Verify approval master exists
            var approval = await _unitOfWork.ApprovalMasters.GetByIdAsync(request.ApprovalMasterId);
            if (approval == null)
            {
                throw new KeyNotFoundException($"Approval master with ID {request.ApprovalMasterId} not found");
            }

            var approver = ApproverEmployee.Create(
                request.ApprovalMasterId,
                request.EmployeeSysId,
                request.ApproverLevel,
                request.EffectiveFrom,
                request.EffectiveTo,
                request.UserId);

            await _unitOfWork.ApproverEmployees.AddAsync(approver);
            approval.AssignApprover(approver);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Created approver employee with ID {ApproverId}", approver.Id);

            return new CreateApproverEmployeeCommandResult { Id = approver.Id };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating approver employee");
            throw;
        }
    }
}

/// <summary>
/// Handler for updating approver employee command
/// </summary>
public class UpdateApproverEmployeeHandler : IRequestHandler<UpdateApproverEmployeeCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateApproverEmployeeHandler> _logger;

    public UpdateApproverEmployeeHandler(IUnitOfWork unitOfWork, ILogger<UpdateApproverEmployeeHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(
        UpdateApproverEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var approver = await _unitOfWork.ApproverEmployees.GetByIdAsync(request.Id);
            if (approver == null)
            {
                throw new KeyNotFoundException($"Approver employee with ID {request.Id} not found");
            }

            approver.Update(request.ApproverLevel, request.EffectiveTo, request.UserId);
            await _unitOfWork.ApproverEmployees.UpdateAsync(approver);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Updated approver employee with ID {ApproverId}", request.Id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating approver employee");
            throw;
        }
    }
}

/// <summary>
/// Handler for deactivating approver employee command
/// </summary>
public class DeactivateApproverEmployeeHandler : IRequestHandler<DeactivateApproverEmployeeCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeactivateApproverEmployeeHandler> _logger;

    public DeactivateApproverEmployeeHandler(IUnitOfWork unitOfWork, ILogger<DeactivateApproverEmployeeHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(
        DeactivateApproverEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var approver = await _unitOfWork.ApproverEmployees.GetByIdAsync(request.Id);
            if (approver == null)
            {
                throw new KeyNotFoundException($"Approver employee with ID {request.Id} not found");
            }

            approver.Deactivate(request.UserId);
            await _unitOfWork.ApproverEmployees.UpdateAsync(approver);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Deactivated approver employee with ID {ApproverId}", request.Id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deactivating approver employee");
            throw;
        }
    }
}

/// <summary>
/// Handler for activating approver employee command
/// </summary>
public class ActivateApproverEmployeeHandler : IRequestHandler<ActivateApproverEmployeeCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ActivateApproverEmployeeHandler> _logger;

    public ActivateApproverEmployeeHandler(IUnitOfWork unitOfWork, ILogger<ActivateApproverEmployeeHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(
        ActivateApproverEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var approver = await _unitOfWork.ApproverEmployees.GetByIdAsync(request.Id);
            if (approver == null)
            {
                throw new KeyNotFoundException($"Approver employee with ID {request.Id} not found");
            }

            approver.Activate(request.UserId);
            await _unitOfWork.ApproverEmployees.UpdateAsync(approver);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Activated approver employee with ID {ApproverId}", request.Id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error activating approver employee");
            throw;
        }
    }
}
