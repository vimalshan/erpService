using MediatR;
using SSCTransactional.Application.DTOs;

namespace SSCTransactional.Application.Commands.Approval;

public record CreateApprovalCommand(long DocId, long ApproverUserId, string Status, DateTime ApprovalDate, string? Remarks = null) : IRequest<DocumentApprovalDto>;
public record UpdateApprovalStatusCommand(long ApprovalId, string Status, string? Remarks = null) : IRequest<DocumentApprovalDto>;
