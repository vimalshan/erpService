namespace EligibilityService.Application.DTOs;

public record EligibilityMasterDto(
    long CanteenUnit,
    string ShiftCode,
    decimal ItemCode,
    int? EligibleLimit,
    long? EnteredUser,
    DateTime? EnteredOn,
    string? TimeOfficeUnit);

public record EligibilityMasterHistoryDto(
    long CanteenUnit,
    string ShiftCode,
    decimal ItemCode,
    int? EligibleLimit,
    decimal? ModifiedUser,
    DateTime? ModifiedOn);

public record ShiftMappingDto(
    long CompanyCode,
    string ShiftCode,
    string BeforeShiftCode,
    string AfterShiftCode);

public record DaywiseEligibilityDto(
    long SerialNumber,
    long CompanyCode,
    long EmployeeSysId,
    DateTime? AttendanceDate,
    long? ProcessNumber,
    string? ShiftCode,
    long? ItemCode,
    int? ShiftQuantity,
    int? BeforeShiftQty,
    int? AfterShiftQty,
    long? EnteredUser,
    DateTime? EnteredOn,
    string? FlexField1,
    string? GradeType);

public record EligibilityCheckResultDto(bool IsEligible, int? EligibleLimit);
