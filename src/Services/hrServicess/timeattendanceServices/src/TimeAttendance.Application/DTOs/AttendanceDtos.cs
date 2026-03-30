namespace TimeAttendance.Application.DTOs;

public record AbsenteeismDetailDto(
    long Id,
    long UnitId,
    int Year,
    int Month,
    long TotalManDays,
    long AbsentManDays,
    long PresentManDays,
    decimal AbsenteeismRate,
    string GradeCategory,
    long FunctionId,
    long AgeId,
    long ExperienceId,
    string Gender,
    long InternalExperienceId,
    long TotalExperienceId,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? LastModifiedAt,
    string? LastModifiedBy
);

public record AbsenteeismMisDto(
    long Id,
    int? UnitId,
    int? CompanyId,
    long? DepartmentId,
    long? SystemId,
    string? Grade,
    decimal? PlannedLeave,
    decimal? PaidDays,
    decimal? WeeklyOff,
    decimal? LeaveWithoutPay,
    decimal? NumberOfPresentHours,
    decimal? CompensatoryOff,
    decimal? BankLeave,
    decimal? AnnualPaidLeave,
    decimal? PenaltyLeave,
    decimal? ShiftSwap,
    decimal? OnDuty,
    string? Month,
    decimal? LogSystemId,
    decimal? LeaveWithoutPayPercentage,
    DateTime CreatedAt
);

public record AbsenteeismSummaryDto(
    long UnitId,
    int Year,
    int Month,
    long TotalManDays,
    long TotalAbsentDays,
    decimal OverallAbsenteeismRate
);

public record PaginatedResult<T>(
    IEnumerable<T> Items,
    int TotalCount,
    int PageNumber,
    int PageSize
)
{
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}
