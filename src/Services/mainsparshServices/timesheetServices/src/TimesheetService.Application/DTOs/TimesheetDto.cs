namespace TimesheetService.Application.DTOs;

public sealed record TimesheetDto(
    long TimesheetId,
    long EmployeeId,
    DateOnly TimesheetDate,
    DateOnly WorkDate,
    TimeOnly? StartTime,
    TimeOnly? EndTime,
    decimal? TotalHours,
    long? ProjectId,
    long? TaskId,
    string? WorkDescription,
    DateTime RecordedDate,
    string Status,
    string ApprovalStatus,
    long? ApprovedBy,
    DateTime? ApprovedOn,
    string? RejectionReason,
    long CreatedBy,
    DateTime CreatedOn,
    long? UpdatedBy,
    DateTime? UpdatedOn
);

public sealed record TimesheetSummaryDto(
    long TimesheetId,
    long EmployeeId,
    DateOnly WorkDate,
    decimal? TotalHours,
    string Status,
    string ApprovalStatus
);

public sealed record MonthlyHoursSummaryDto(
    long EmployeeId,
    int Year,
    int Month,
    decimal TotalHours
);
