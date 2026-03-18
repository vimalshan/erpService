namespace OrganizationStructureService.Application.DTOs;

public record BusinessDto(
    decimal BusinessId,
    string BusinessName,
    string BusinessShortName,
    string BusinessCode,
    decimal BusinessCompanyId,
    string BusinessCompanyCode,
    string LiveFlag,
    DateTime? UpdatedOn,
    decimal? UpdatedBy);

public record UnitDto(
    decimal UnitId,
    string UnitName,
    string UnitShortName,
    string UnitCode,
    decimal UnitBusinessId,
    string UnitBusinessCode,
    string LiveFlag,
    decimal OrgId,
    string? ReportFlag,
    DateTime? UpdatedOn,
    decimal? UpdatedBy);

public record DepartmentDto(
    decimal DepartmentId,
    string? DepartmentName,
    string? DepartmentCode,
    string? LiveFlag,
    DateTime? UpdatedOn,
    decimal? UpdatedBy);

public record DivisionDto(
    decimal DivisionId,
    string? DivisionCode,
    string? DivisionName,
    string? LiveFlag,
    DateTime? UpdatedOn,
    decimal? UpdatedBy);

public record GradeDto(
    decimal GradeId,
    string? GradeCode,
    string? GradeName,
    string? GradeDesignation,
    string? GradeCategoryCode,
    string? LiveFlag,
    string? ManagementCategoryCode,
    decimal? Priority);

public record PositionDto(
    decimal PositionId,
    string PosUnitCode,
    decimal PosGradeId,
    string? PositionName,
    string PositionDesignation,
    DateTime PosEffectiveDate,
    DateTime? PosClosedDate,
    string ReferenceCode,
    string? DeletedFlag,
    decimal? Ctc);

public record SiteDto(
    decimal SiteId,
    string? SiteName,
    string? SiteShortName,
    string? AddressLine1,
    string? AddressLine2,
    string? AddressPin,
    decimal SiteCityCode,
    decimal SiteCategoryCode,
    string? Phone1,
    string? LiveFlag);

public record LocationDto(
    decimal LocationCode,
    string? LocationName,
    decimal LocationRegionCode);

public record GradeListItemDto(
    decimal GradeId,
    string? GradeName,
    string? GradeCode,
    string? GradeDesignation,
    string? LiveFlag);
