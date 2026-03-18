namespace Stationery.Application.DTOs;

public record RequestDto(
    long Id,
    long RequestedBy,
    DateTime RequestedOn,
    long? LocationId,
    string? UnitCode,
    IEnumerable<RequestSubDto> Details
);

public record RequestSubDto(
    long Id,
    long StationaryId,
    long DeptId,
    DateTime ExpectedDate,
    long RequestedQty,
    long? ApprovedQty,
    string Status,
    string? ApproverRemarks,
    DateTime? ApprovedOn,
    DateTime? ReceivedDate
);

public record RequestSummaryDto(
    long Id,
    long RequestedBy,
    DateTime RequestedOn,
    long? LocationId,
    string? UnitCode,
    int TotalItems,
    int PendingItems,
    int ApprovedItems
);
