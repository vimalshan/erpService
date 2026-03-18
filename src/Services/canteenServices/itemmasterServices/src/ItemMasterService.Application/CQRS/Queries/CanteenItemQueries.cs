using MediatR;
using ItemMasterService.Application.DTOs;

namespace ItemMasterService.Application.CQRS.Queries;

public record GetCanteenItemByIdQuery(
    long CanteenUnitCode,
    long ItemCode) : IRequest<CanteenItemMasterDto?>;

public record GetAllCanteenItemsQuery(
    long CanteenUnitCode) : IRequest<IEnumerable<CanteenItemMasterDto>>;

public record GetItemPriceQuery(
    long CanteenUnitCode,
    long ItemCode) : IRequest<CanteenItemPriceMasterDto?>;

public record GetItemPriceHistoryQuery(
    long CanteenUnitCode,
    long ItemCode) : IRequest<IEnumerable<CanteenItemPriceMasterDto>>;

public record GetGradeItemPriceQuery(
    long CanteenUnitCode) : IRequest<CanteenGradeItemPriceDto?>;

public record GetAllGradeItemPricesQuery() : IRequest<IEnumerable<CanteenGradeItemPriceDto>>;
