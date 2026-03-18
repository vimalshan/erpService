using MediatR;
using OrganizationStructureService.Application.DTOs;

namespace OrganizationStructureService.Application.Commands;

// Business Commands
public record CreateBusinessCommand(
    decimal BusinessId,
    string BusinessName,
    string BusinessShortName,
    string BusinessCode,
    decimal CompanyId,
    string CompanyCode,
    decimal UpdatedBy) : IRequest<BusinessDto>;

public record UpdateBusinessCommand(
    decimal BusinessId,
    string BusinessName,
    string BusinessShortName,
    decimal UpdatedBy) : IRequest<BusinessDto>;

public record DeactivateBusinessCommand(decimal BusinessId, decimal UpdatedBy) : IRequest<bool>;

// Unit Commands
public record CreateUnitCommand(
    decimal UnitId,
    string UnitName,
    string UnitShortName,
    string UnitCode,
    decimal BusinessId,
    string BusinessCode,
    decimal OrgId,
    string ReportFlag,
    decimal UpdatedBy) : IRequest<UnitDto>;

public record UpdateUnitCommand(
    decimal UnitId,
    string UnitName,
    string UnitShortName,
    decimal UpdatedBy) : IRequest<UnitDto>;

public record DeactivateUnitCommand(decimal UnitId, decimal UpdatedBy) : IRequest<bool>;

// Department Commands
public record CreateDepartmentCommand(
    decimal DepartmentId,
    string DepartmentName,
    string? DepartmentCode,
    decimal UpdatedBy) : IRequest<DepartmentDto>;

public record UpdateDepartmentCommand(
    decimal DepartmentId,
    string DepartmentName,
    string? DepartmentCode,
    decimal UpdatedBy) : IRequest<DepartmentDto>;

// Division Commands
public record CreateDivisionCommand(
    decimal DivisionId,
    string DivisionName,
    string DivisionCode,
    decimal UpdatedBy) : IRequest<DivisionDto>;

// Grade Commands
public record CreateGradeCommand(
    decimal GradeId,
    string GradeName,
    string? GradeCode,
    string? GradeDesignation,
    string? CategoryCode,
    string? ManagementCategoryCode,
    decimal? Priority) : IRequest<GradeDto>;

public record UpdateGradeCommand(
    decimal GradeId,
    string GradeName,
    string? GradeDesignation,
    decimal? Priority) : IRequest<GradeDto>;

// Position Commands
public record CreatePositionCommand(
    decimal PositionId,
    string UnitCode,
    decimal GradeId,
    string? PositionName,
    string Designation,
    DateTime EffectiveDate,
    string ReferenceCode,
    decimal EnteredBy,
    decimal? Ctc) : IRequest<PositionDto>;

public record ClosePositionCommand(decimal PositionId, DateTime CloseDate, decimal ModifiedBy) : IRequest<bool>;
public record DeletePositionCommand(decimal PositionId, decimal ModifiedBy) : IRequest<bool>;

// Site Commands
public record CreateSiteCommand(
    decimal SiteId,
    string SiteName,
    string SiteShortName,
    decimal CityCode,
    decimal CategoryCode) : IRequest<SiteDto>;
