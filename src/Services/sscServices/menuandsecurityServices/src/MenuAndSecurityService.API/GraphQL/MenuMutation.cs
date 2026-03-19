using MediatR;
using MenuAndSecurityService.Application.Commands.CreateMenu;
using MenuAndSecurityService.Application.Commands.DeleteMenu;
using MenuAndSecurityService.Application.Commands.GrantMenuAccess;
using MenuAndSecurityService.Application.Commands.RevokeMenuAccess;
using MenuAndSecurityService.Application.Commands.UpdateMenu;
using MenuAndSecurityService.Application.DTOs;

namespace MenuAndSecurityService.API.GraphQL;

public class MenuMutation
{
    public async Task<MenuMasterDto> CreateMenu([Service] IMediator mediator,
        long menuId, string menuName, string menuPageName, long? menuParentId, int menuDisplayOrder, long modifiedBy)
    {
        return await mediator.Send(new CreateMenuCommand(menuId, menuName, menuPageName, menuParentId, menuDisplayOrder, modifiedBy));
    }

    public async Task<MenuMasterDto> UpdateMenu([Service] IMediator mediator,
        long menuId, string menuName, string menuPageName, long? menuParentId, int menuDisplayOrder, long modifiedBy)
    {
        return await mediator.Send(new UpdateMenuCommand(menuId, menuName, menuPageName, menuParentId, menuDisplayOrder, modifiedBy));
    }

    public async Task<bool> DeleteMenu([Service] IMediator mediator, long menuId)
    {
        return await mediator.Send(new DeleteMenuCommand(menuId));
    }

    public async Task<RoleMenuAccessDto> GrantMenuAccess([Service] IMediator mediator,
        long menuAccessId, int menuId, long menuRoleId, long modifiedBy)
    {
        return await mediator.Send(new GrantMenuAccessCommand(menuAccessId, menuId, menuRoleId, modifiedBy));
    }

    public async Task<bool> RevokeMenuAccess([Service] IMediator mediator, long menuAccessId)
    {
        return await mediator.Send(new RevokeMenuAccessCommand(menuAccessId));
    }
}
