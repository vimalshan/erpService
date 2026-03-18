namespace AttendanceService.Application.DTOs;

public record AttendancePercentageDto(
    long EmpSysId,
    DateTime MonthStart,
    DateTime MonthEnd,
    int PresentDays,
    int WorkingDays,
    decimal Percentage);

public record LopDto(
    long LopId,
    long EmpSysId,
    long BatchId,
    decimal LopDays,
    string LopType,
    DateTime LastModifiedOn);

public record GraceAdjustDto(
    long GraceId,
    long EmpSysId,
    DateTime Date,
    int Minutes,
    DateTime LastModifiedOn);

public record LeaveAdjustDto(
    long LeaveAdjId,
    long EmpSysId,
    DateTime Date,
    string LeaveType,
    DateTime LastModifiedOn);

public record NightShiftDto(
    long NightId,
    long EmpSysId,
    DateTime NightDate,
    string NightType,
    DateTime LastModifiedOn);
