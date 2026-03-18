namespace ItemMasterService.Application.DTOs;

public record CanteenItemMasterDto(
    long CanteenUnitCode,
    long ItemCode,
    string? ItemDescription,
    string? ItemType,
    string? ItemReference,
    DateTime? EnteredOn,
    string? EnteredBy);

public record CanteenItemPriceMasterDto(
    long CanteenUnitCode,
    long ItemCode,
    decimal? EmployeeContribution,
    decimal? EmployerContribution,
    DateTime EffectiveDate,
    DateTime? ClosureDate,
    DateTime? EnteredOn,
    string? EnteredBy);

public record CanteenGradeItemPriceDto(
    long CanteenUnitCode,
    long? ItemCode,
    decimal? EmployeeContribution,
    decimal? EmployerContribution,
    DateTime? EffectiveDate,
    DateTime ClosureDate,
    DateTime? EnteredOn,
    string EnteredBy,
    string GradeType);
