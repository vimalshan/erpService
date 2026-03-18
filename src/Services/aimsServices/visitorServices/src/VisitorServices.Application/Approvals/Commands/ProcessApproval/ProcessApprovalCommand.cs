using MediatR;
using VisitorServices.Application.DTOs;

namespace VisitorServices.Application.Approvals.Commands.ProcessApproval;

public sealed record ProcessApprovalCommand(
    long RequestId,
    bool IsApproved,
    string? Remarks,
    long ProcessedBy) : IRequest<ApprovalRequestDto>;
