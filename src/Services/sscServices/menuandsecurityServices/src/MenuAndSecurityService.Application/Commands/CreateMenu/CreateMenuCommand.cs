using MediatR;
using MenuAndSecurityService.Application.DTOs;

namespace MenuAndSecurityService.Application.Commands.CreateMenu;

public sealed record CreateMenuCommand(
    long MenuId,
    string MenuName,
    string MenuPageName,
    long? MenuParentId,
    int MenuDisplayOrder,
    long ModifiedBy
) : IRequest<MenuMasterDto>;
