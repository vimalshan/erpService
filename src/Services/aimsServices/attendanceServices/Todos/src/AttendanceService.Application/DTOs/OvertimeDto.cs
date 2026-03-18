namespace AttendanceService.Application.DTOs;

public record OvertimeDto(
    long OtId,
    long EmpSysId,
    DateTime OtDate,
    decimal Hours,
    string OtType,
    string Approved,
    DateTime LastModifiedOn);

public record CreateOvertimeRequest(
    long EmpSysId,
    DateTime OtDate,
    decimal Hours,
    string OtType,
    long CreatedBy);
