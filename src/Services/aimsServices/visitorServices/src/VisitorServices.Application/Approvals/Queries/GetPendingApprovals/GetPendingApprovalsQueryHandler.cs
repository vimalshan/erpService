using MediatR;
using VisitorServices.Application.Common.Interfaces;
using VisitorServices.Application.DTOs;
using VisitorServices.Domain.Entities;

namespace VisitorServices.Application.Approvals.Queries.GetPendingApprovals;

public sealed class GetPendingApprovalsQueryHandler(IApprovalRequestRepository approvalRepository)
    : IRequestHandler<GetPendingApprovalsQuery, IEnumerable<ApprovalRequestDto>>
{
    public async Task<IEnumerable<ApprovalRequestDto>> Handle(
        GetPendingApprovalsQuery request, CancellationToken cancellationToken)
    {
        var approvals = await approvalRepository.GetPendingByApproverAsync(request.ApproverId, cancellationToken);
        return approvals.Select(a => new ApprovalRequestDto(
            a.Id, a.VisitorId, a.RequiredApproverId,
            (char)(int)a.ApprovalStatus, a.ApprovalDate, a.ApprovalRemarks,
            a.RequestedOn, a.RequestedBy));
    }
}
