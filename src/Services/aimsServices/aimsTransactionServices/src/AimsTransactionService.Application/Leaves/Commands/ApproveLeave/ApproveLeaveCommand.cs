using MediatR;

namespace AimsTransactionService.Application.Leaves.Commands.ApproveLeave;

public sealed record ApproveLeaveCommand(
    long LeaveDetailId,
    bool IsApproved,
    string? Remarks,
    long ProcessedBy) : IRequest;
