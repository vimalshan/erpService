namespace VisitorServices.Application.DTOs;

public sealed record ApprovalRequestDto(
    long RequestId,
    long VisitorId,
    long RequiredApproverId,
    char ApprovalStatus,
    DateTime? ApprovalDate,
    string? ApprovalRemarks,
    DateTime RequestedOn,
    long RequestedBy);
