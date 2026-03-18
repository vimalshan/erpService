namespace EmployeeService.Application.DTOs;

public record EmployeeTimeInfoDto(
    long TimeInfoId,
    long EmpSysId,
    char EmpAttFlag,
    long LastModifiedBy,
    DateTime LastModifiedOn
);

public record EmployeeApproverDto(
    int ApproverId,
    long EmpSysId,
    int Level,
    long ApproverSysId,
    DateTime EffDate,
    long LastModifiedBy,
    DateTime LastModifiedOn
);

public record EmployeeApprovalMailDto(
    int AppMailId,
    long EmpSysId,
    long AppMailSysId,
    DateTime EffDate,
    long? LastModifiedBy,
    DateTime? LastModifiedOn
);

public record EmployeeCalendarDto(
    long EmpCalId,
    long EmpSysId,
    int CalendarId,
    long? SwipeId,
    DateTime EffDate,
    DateTime? ClsDate,
    char? Status,
    int? Transfer,
    long? SettlementNo,
    long LastModifiedBy,
    DateTime LastModifiedOn
);

public record EmployeePatternDto(
    long EmpPatternId,
    long EmpSysId,
    int PatternMastId,
    DateTime EffDate,
    DateTime? ClsDate,
    int WeeklyOffDay,
    int? SubWeeklyDay,
    string? SubFrequency,
    long? LastModifiedBy,
    DateTime? LastModifiedOn
);

public record EmployeeShiftDto(
    long EmpShiftId,
    long EmpSysId,
    int TimeUnitId,
    char ShiftCode,
    int YearMonth,
    int Day,
    DateTime ShiftDate,
    long UpdatedBy,
    DateTime UpdatedOn
);

public record EmployeeShiftPatternDto(
    long EmpShiftId,
    long? EmpSysId,
    long? TimeUnitId,
    int? YearMonth,
    string? OrgPattern,
    string? NewPattern,
    long LastModifiedBy,
    DateTime LastModifiedOn
);
