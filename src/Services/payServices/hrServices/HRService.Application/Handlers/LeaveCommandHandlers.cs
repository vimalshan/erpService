using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using HRService.Application.Commands;
using HRService.Infrastructure.Repositories;
using HRService.Domain.Entities;

namespace HRService.Application.Handlers;

/// <summary>
/// Command handlers for leave operations
/// </summary>
public class RequestLeaveCommandHandler : IRequestHandler<RequestLeaveCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RequestLeaveCommandHandler> _logger;

    public RequestLeaveCommandHandler(IUnitOfWork unitOfWork, ILogger<RequestLeaveCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Guid> Handle(RequestLeaveCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing leave request for employee: {EmployeeId}", request.EmployeeId);

        try
        {
            // Create leave request
            var leave = EmployeeLeave.Create(
                request.EmployeeId,
                request.LeaveTypeId,
                request.StartDate,
                request.EndDate,
                request.Reason
            );

            await _unitOfWork.LeaveRepository.AddAsync(leave, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Leave request created with ID: {LeaveId}", leave.Id);
            return leave.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error requesting leave");
            throw;
        }
    }
}

public class ApproveLeaveCommandHandler : IRequestHandler<ApproveLeaveCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ApproveLeaveCommandHandler> _logger;

    public ApproveLeaveCommandHandler(IUnitOfWork unitOfWork, ILogger<ApproveLeaveCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(ApproveLeaveCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var leave = await _unitOfWork.LeaveRepository.GetByIdAsync(request.LeaveId, cancellationToken);
            if (leave == null)
                throw new InvalidOperationException($"Leave {request.LeaveId} not found");

            leave.Approve(request.ApprovedBy);

            await _unitOfWork.LeaveRepository.UpdateAsync(leave, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Leave {LeaveId} approved", request.LeaveId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving leave");
            return false;
        }
    }
}

public class RejectLeaveCommandHandler : IRequestHandler<RejectLeaveCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RejectLeaveCommandHandler> _logger;

    public RejectLeaveCommandHandler(IUnitOfWork unitOfWork, ILogger<RejectLeaveCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(RejectLeaveCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var leave = await _unitOfWork.LeaveRepository.GetByIdAsync(request.LeaveId, cancellationToken);
            if (leave == null)
                return false;

            leave.Reject();

            await _unitOfWork.LeaveRepository.UpdateAsync(leave, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Leave {LeaveId} rejected", request.LeaveId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rejecting leave");
            return false;
        }
    }
}

public class CancelLeaveCommandHandler : IRequestHandler<CancelLeaveCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public CancelLeaveCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(CancelLeaveCommand request, CancellationToken cancellationToken)
    {
        var leave = await _unitOfWork.LeaveRepository.GetByIdAsync(request.LeaveId, cancellationToken);
        if (leave == null)
            return false;

        leave.Cancel();
        await _unitOfWork.LeaveRepository.UpdateAsync(leave, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
