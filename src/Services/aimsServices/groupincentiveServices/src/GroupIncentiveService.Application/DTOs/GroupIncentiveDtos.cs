namespace GroupIncentiveService.Application.DTOs;

public record GroupMasterDto(
    int GroupId,
    string GroupName,
    string? GroupDescription,
    DateTime GroupEffDate,
    DateTime? GroupClsDate,
    string GroupStatus,
    long GroupLastModifiedBy,
    DateTime GroupLastModifiedOn);

public record GroupEmployeeMapDto(
    long GrpEmpMapId,
    int GrpEmpMapGroupId,
    long GrpEmpMapEmpSysId,
    DateTime GrpEmpMapEffDate,
    DateTime? GrpEmpMapClsDate,
    string? GrpEmpMapRole,
    long GrpEmpMapLastModifiedBy,
    DateTime GrpEmpMapLastModifiedOn);

public record GroupIncentiveMainDto(
    long GrpIncId,
    int GrpIncGroupId,
    string? GroupName,
    int GrpIncIncMonth,
    int GrpIncIncYear,
    decimal GrpIncTotalAmount,
    string GrpIncAppStatus,
    decimal? GrpIncApprovedAmount,
    long? GrpIncApprover,
    DateTime? GrpIncApprovalDate,
    DateTime GrpIncEnteredOn,
    long GrpIncEnteredBy,
    IReadOnlyList<GroupIncentiveDetDto>? Details);

public record GroupIncentiveDetDto(
    long GrpIncDetId,
    long GrpIncDetMainId,
    long GrpIncDetEmpSysId,
    decimal GrpIncDetAllocPercentage,
    decimal GrpIncDetAllocAmount,
    decimal? GrpIncDetApprovedAmount,
    string GrpIncDetAppStatus);

public record GroupIncentiveBreakDto(
    int GrpIncBrkId,
    int GrpIncBrkGroupId,
    decimal GrpIncBrkAttPercentage,
    decimal GrpIncBrkIncPercentage,
    DateTime GrpIncBrkEffDate,
    DateTime? GrpIncBrkClsDate);

public record GroupIncentiveApprovalDto(
    long GrpIncAppId,
    long GrpIncAppMainId,
    long GrpIncAppApprover,
    string GrpIncAppStatus,
    string? GrpIncAppRemarks,
    DateTime GrpIncAppApprovalDate);

public record EmployeeIncentiveSummaryDto(
    long EmployeeId,
    int Month,
    int Year,
    decimal TotalAllocatedAmount,
    decimal TotalApprovedAmount);
