using MasterDataService.Application.Commands.Area;
using MasterDataService.Application.Commands.GuestHouse;
using MasterDataService.Application.Commands.Route;
using MasterDataService.Application.DTOs;
using MediatR;

namespace MasterDataService.API.GraphQL;

public class MasterDataMutation
{
    public async Task<GuestHouseDto> CreateGuestHouse(
        [Service] IMediator mediator,
        long adminCode, string guestHouseName, long dailyAmount)
        => await mediator.Send(new CreateGuestHouseCommand(adminCode, guestHouseName, dailyAmount));

    public async Task<GuestHouseDto> UpdateGuestHouse(
        [Service] IMediator mediator,
        long id, string guestHouseName, long dailyAmount)
        => await mediator.Send(new UpdateGuestHouseCommand(id, guestHouseName, dailyAmount));

    public async Task<bool> DeleteGuestHouse([Service] IMediator mediator, long id)
        => await mediator.Send(new DeleteGuestHouseCommand(id));

    public async Task<AreaDto> CreateArea(
        [Service] IMediator mediator,
        int areaId, string areaName)
        => await mediator.Send(new CreateAreaCommand(areaId, areaName));

    public async Task<AreaDto> UpdateArea(
        [Service] IMediator mediator,
        long id, string areaName)
        => await mediator.Send(new UpdateAreaCommand(id, areaName));

    public async Task<bool> DeleteArea([Service] IMediator mediator, long id)
        => await mediator.Send(new DeleteAreaCommand(id));

    public async Task<RouteDto> CreateRoute(
        [Service] IMediator mediator,
        int routeId, string routeName)
        => await mediator.Send(new CreateRouteCommand(routeId, routeName));

    public async Task<RouteDto> UpdateRoute(
        [Service] IMediator mediator,
        long id, string routeName)
        => await mediator.Send(new UpdateRouteCommand(id, routeName));

    public async Task<bool> DeleteRoute([Service] IMediator mediator, long id)
        => await mediator.Send(new DeleteRouteCommand(id));
}
