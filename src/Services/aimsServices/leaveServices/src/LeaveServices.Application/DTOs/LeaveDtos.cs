namespace LeaveServices.Application.DTOs;

public record LeaveMasterDto(
    long   LeaveId,
    string LeaveDescription,
    char   LeaveGenderSpecific,
    char   LeaveApplicableForAll,
    int    LeaveMaxDaysPL,
    char   LeaveEncashable,
    char   LeaveCarryForward);

public record LeaveDetailsDto(
    long     LeaveDetailId,
    long     LeaveEmpSysId,
    DateTime LeaveAppFrom,
    DateTime LeaveAppTo,
    string   LeaveAppType,
    long     LeaveId,
    string?  LeaveTypeName,
    string   LeaveAppStatus,
    string   LeaveAppStatusDisplay,
    decimal  LeaveAppliedDays,
    string?  LeaveReason,
    DateTime LeaveEnteredOn,
    long     LeaveEnteredBy);

public record LeaveCreditDto(
    long    CreditId,
    long    CreditEmpSysId,
    long    CreditLeaveId,
    string? LeaveTypeName,
    int     CreditYear,
    decimal CreditOpening,
    decimal CreditCredited,
    decimal CreditUtilized,
    decimal CreditClosing,
    decimal AvailableBalance);

public record LeaveApprovalDto(
    long     LeaveAprId,
    long     LeaveAprDetailId,
    string   LeaveAprApproveStatus,
    string?  LeaveAprRemarks,
    DateTime LeaveAprApprovedOn,
    long     LeaveAprApprovedBy);

public record LeaveBalanceDto(
    long    EmpSysId,
    long    LeaveId,
    string  LeaveTypeName,
    int     Year,
    decimal Balance);

public record CompOffDto(
    long     CompOffId,
    long     CompOffEmpSysId,
    DateTime CompOffCompOffDate,
    DateTime? CompOffUsedDate,
    string   CompOffStatus);
