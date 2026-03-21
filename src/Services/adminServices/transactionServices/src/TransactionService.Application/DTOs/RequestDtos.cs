namespace TransactionService.Application.DTOs;

public sealed record RequestMainDto(
    long RequestId,
    long RequestedBy,
    DateTime RequestedOn,
    long? LocationId,
    string? UnitCode,
    List<RequestSubDto> Details);

public sealed record RequestSubDto(
    long RequestSubId,
    long RequestId,
    long StationaryId,
    long DeptId,
    DateTime ExpectedDate,
    long? UserSysId,
    long RequestedQty,
    long? IndentedQty,
    long? ApprovedQty,
    long? ApproverSysId,
    string? ApproverRemarks,
    DateTime? ReceivedDate,
    string Status,
    long UpdatedBy,
    DateTime UpdatedOn,
    DateTime? ApprovedOn);

public sealed record RequestSummaryDto(
    long RequestId,
    long RequestedBy,
    DateTime RequestedOn,
    long? LocationId,
    int TotalItems,
    int PendingItems,
    int ApprovedItems);
