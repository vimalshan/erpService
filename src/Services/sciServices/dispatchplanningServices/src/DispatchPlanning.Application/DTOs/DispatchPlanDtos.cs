namespace DispatchPlanning.Application.DTOs;

public record DispatchPlanHeaderDto(
    int DispatchPlanHeaderId,
    string PlanType,
    DateTime PlanMonth,
    string? PlanMPlus1,
    string? PlanMPlus2,
    string? PlanMPlus3,
    string? PlanMPlus4,
    DateTime EntryDate,
    int CompanyUnitId,
    int SciUserIdModified,
    DateTime ModifiedDate
);

public record DispatchPlanItemDto(
    int DispatchPlanHeaderId,
    int BreakupItemId,
    long? TargetWeek1,
    long? TargetWeek2,
    long? TargetWeek3,
    long? TargetWeek4,
    long? TargetWeek5,
    long? TargetMPlus1,
    long? TargetMPlus2,
    long? TargetMPlus3,
    long? TargetMPlus4
);

public record DispatchPlanSubGroupTargetDto(
    int DispatchPlanHeaderId,
    int SubGroupId,
    long? TargetWeek1,
    long? TargetWeek2,
    long? TargetWeek3,
    long? TargetWeek4,
    long? TargetWeek5,
    long? TargetMPlus1,
    long? TargetMPlus2,
    long? TargetMPlus3,
    long? TargetMPlus4
);

public record DispatchPlanDetailDto(
    DispatchPlanHeaderDto Header,
    IReadOnlyList<DispatchPlanItemDto> Items,
    IReadOnlyList<DispatchPlanSubGroupTargetDto> SubGroupTargets
);

public record MainGroupDto(
    int MainGroupId,
    string MainGroupName,
    string GroupType,
    string ProductSummary,
    string TotalDisplayName,
    int MgDisplayOrder,
    int CompanyUnitId
);

public record SubGroupDto(
    int SubGroupId,
    int MainGroupId,
    string SubGroupName,
    int? ProductId,
    int? SgDisplayOrder,
    string CaptureTotalDirectly
);

public record BreakupItemDto(
    int BreakupItemId,
    int SubGroupId,
    int ProductId,
    string BreakupItemDesc,
    int UnitId,
    int MainProductUnitsConFactor,
    int BiDisplayOrder,
    DateTime EffectiveDate,
    string? ClosureDate,
    decimal? PackageId
);
