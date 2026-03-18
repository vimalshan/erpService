using MediatR;
using ItemMasterService.Application.DTOs;

namespace ItemMasterService.Application.CQRS.Commands;

// ---- CanteenItemMaster Commands ----

public record CreateCanteenItemCommand(
    long CanteenUnitCode,
    long ItemCode,
    string? ItemDescription,
    string? ItemType,
    string? ItemReference,
    string EnteredBy) : IRequest<CanteenItemMasterDto>;

public record UpdateCanteenItemCommand(
    long CanteenUnitCode,
    long ItemCode,
    string? ItemDescription,
    string? ItemType,
    string? ItemReference) : IRequest<CanteenItemMasterDto>;

public record DeleteCanteenItemCommand(
    long CanteenUnitCode,
    long ItemCode) : IRequest<bool>;

// ---- ItemPrice Commands ----

public record CreateItemPriceCommand(
    long CanteenUnitCode,
    long ItemCode,
    decimal? EmployeeContribution,
    decimal? EmployerContribution,
    DateTime EffectiveDate,
    string EnteredBy) : IRequest<CanteenItemPriceMasterDto>;

public record CloseItemPriceCommand(
    long CanteenUnitCode,
    long ItemCode,
    DateTime ClosureDate) : IRequest<bool>;

// ---- GradeItemPrice Commands ----

public record CreateGradeItemPriceCommand(
    long CanteenUnitCode,
    long? ItemCode,
    decimal? EmployeeContribution,
    decimal? EmployerContribution,
    DateTime? EffectiveDate,
    DateTime ClosureDate,
    string EnteredBy,
    string GradeType) : IRequest<CanteenGradeItemPriceDto>;

public record UpdateGradeItemPriceCommand(
    long CanteenUnitCode,
    decimal? EmployeeContribution,
    decimal? EmployerContribution,
    DateTime ClosureDate) : IRequest<CanteenGradeItemPriceDto>;
