using MediatR;
using MenuAndSecurityService.Application.DTOs;

namespace MenuAndSecurityService.Application.Commands.UpdateMenu;

public sealed record UpdateMenuCommand(
    long MenuId,
    string MenuName,
    string MenuPageName,
    long? MenuParentId,
    int MenuDisplayOrder,
    long ModifiedBy
) : IRequest<MenuMasterDto>;
