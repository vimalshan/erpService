namespace AttendanceService.Application.DTOs;

public record SwipePunchDto(
    long SwipeId,
    long EmpSysId,
    DateTime PunchTime,
    string GateNo,
    string PunchStatus,
    string? PullStatus,
    string? Verified,
    DateTime? LastModifiedOn);

public record RecordSwipePunchRequest(
    long EmpSysId,
    DateTime PunchTime,
    string GateNo,
    string PunchStatus);
