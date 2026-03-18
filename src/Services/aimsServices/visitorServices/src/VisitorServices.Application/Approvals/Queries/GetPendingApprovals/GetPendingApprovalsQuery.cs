using MediatR;
using VisitorServices.Application.DTOs;

namespace VisitorServices.Application.Approvals.Queries.GetPendingApprovals;

public sealed record GetPendingApprovalsQuery(long ApproverId) : IRequest<IEnumerable<ApprovalRequestDto>>;
