using MasterDataService.Application.DTOs;
using MasterDataService.Application.Queries.Area;
using MasterDataService.Application.Queries.Coupon;
using MasterDataService.Application.Queries.GuestHouse;
using MasterDataService.Application.Queries.Route;
using MasterDataService.Application.Queries.TaxSlab;
using MediatR;

namespace MasterDataService.API.GraphQL;

public class MasterDataQuery
{
    [UseFiltering]
    [UseSorting]
    public async Task<IReadOnlyList<GuestHouseDto>> GetGuestHouses([Service] IMediator mediator)
        => await mediator.Send(new GetAllGuestHousesQuery());

    public async Task<GuestHouseDto?> GetGuestHouseById([Service] IMediator mediator, long id)
        => await mediator.Send(new GetGuestHouseByIdQuery(id));

    [UseFiltering]
    [UseSorting]
    public async Task<IReadOnlyList<AreaDto>> GetAreas([Service] IMediator mediator)
        => await mediator.Send(new GetAllAreasQuery());

    public async Task<AreaDto?> GetAreaById([Service] IMediator mediator, long id)
        => await mediator.Send(new GetAreaByIdQuery(id));

    [UseFiltering]
    [UseSorting]
    public async Task<IReadOnlyList<RouteDto>> GetRoutes([Service] IMediator mediator)
        => await mediator.Send(new GetAllRoutesQuery());

    public async Task<RouteDto?> GetRouteById([Service] IMediator mediator, long id)
        => await mediator.Send(new GetRouteByIdQuery(id));

    [UseFiltering]
    [UseSorting]
    public async Task<IReadOnlyList<CouponDto>> GetCoupons([Service] IMediator mediator)
        => await mediator.Send(new GetAllCouponsQuery());

    [UseFiltering]
    [UseSorting]
    public async Task<IReadOnlyList<TaxSlabDto>> GetTaxSlabs([Service] IMediator mediator)
        => await mediator.Send(new GetAllTaxSlabsQuery());

    public async Task<IReadOnlyList<TaxSlabDto>> GetActiveTaxSlabs([Service] IMediator mediator)
        => await mediator.Send(new GetActiveTaxSlabsQuery());
}
