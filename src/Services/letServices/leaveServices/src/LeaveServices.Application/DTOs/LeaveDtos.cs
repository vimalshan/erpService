namespace LeaveServices.Application.DTOs;

public record LeaveRequestDto(
    long ReqNum,
    int FinyearSrlno,
    string EmpUserId,
    string? SupUserId,
    DateTime? ReqDate,
    List<LeaveRequestDetailDto> Details);

public record LeaveRequestDetailDto(
    long LsReqNum,
    int LsSrlNum,
    DateTime? LsModDat,
    string? LsModUser,
    char? LsPrefModdev,
    string? LsActTaken,
    int? LsCrsId,
    string? LsRevType,
    string? LsLetsubCode);

public record LeaveEncashmentDto(
    long EncashmentId,
    long EmpSysId,
    string LeaveType,
    int EncashmentDays,
    decimal EncashmentAmount,
    DateOnly RequestDate,
    char EncashmentStatus,
    string StatusDescription,
    DateTime CreatedOn);

public record LossOfPayDto(
    long LopId,
    long EmpSysId,
    int LopDays,
    DateOnly LopMonth,
    string? LopRemarks,
    DateTime CreatedOn);

public record LeaveCounterDto(string TypeCode, long? CurrentCount, string? Description);
