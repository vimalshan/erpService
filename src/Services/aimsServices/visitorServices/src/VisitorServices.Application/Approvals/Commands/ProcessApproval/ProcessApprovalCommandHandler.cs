using MediatR;
using VisitorServices.Application.Common.Interfaces;
using VisitorServices.Application.DTOs;
using VisitorServices.Domain.Entities;

namespace VisitorServices.Application.Approvals.Commands.ProcessApproval;

public sealed class ProcessApprovalCommandHandler(
    IApprovalRequestRepository approvalRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ProcessApprovalCommand, ApprovalRequestDto>
{
    public async Task<ApprovalRequestDto> Handle(ProcessApprovalCommand request, CancellationToken cancellationToken)
    {
        var approval = await approvalRepository.GetByIdAsync(request.RequestId, cancellationToken)
            ?? throw new KeyNotFoundException($"Approval request {request.RequestId} not found.");

        if (request.IsApproved)
            approval.Approve(request.Remarks, request.ProcessedBy);
        else
            approval.Reject(request.Remarks, request.ProcessedBy);

        approvalRepository.Update(approval);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(approval);
    }

    private static ApprovalRequestDto MapToDto(VisitorApprovalRequest a) => new(
        a.Id, a.VisitorId, a.RequiredApproverId,
        ((char)(int)a.ApprovalStatus).ToString(), a.ApprovalDate, a.ApprovalRemarks,
        a.RequestedOn, a.RequestedBy);
}
