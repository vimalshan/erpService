using MediatR;
using MenuAndSecurityService.Application.Commands.CreateMenu;
using MenuAndSecurityService.Application.Commands.DeleteMenu;
using MenuAndSecurityService.Application.Commands.UpdateMenu;
using MenuAndSecurityService.Application.Queries.GetAllMenus;
using MenuAndSecurityService.Application.Queries.GetMenuById;
using MenuAndSecurityService.Application.Queries.GetMenusByRole;
using MenuAndSecurityService.Infrastructure.Dapper;

namespace MenuAndSecurityService.API.MinimalApis;

public static class MenuEndpoints
{
    public static void MapMenuEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/minimal/menus")
            .WithTags("Menus (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllMenusQuery());
            return Results.Ok(result);
        });

        group.MapGet("/{id:long}", async (long id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetMenuByIdQuery(id));
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        group.MapPost("/", async (CreateMenuCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Created($"/api/minimal/menus/{result.MenuId}", result);
        }).RequireAuthorization("AdminOnly");

        group.MapPut("/{id:long}", async (long id, UpdateMenuCommand command, IMediator mediator) =>
        {
            if (id != command.MenuId) return Results.BadRequest("Menu ID mismatch");
            var result = await mediator.Send(command);
            return Results.Ok(result);
        }).RequireAuthorization("AdminOnly");

        group.MapDelete("/{id:long}", async (long id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteMenuCommand(id));
            return result ? Results.NoContent() : Results.NotFound();
        }).RequireAuthorization("AdminOnly");

        // Dapper-based endpoints
        group.MapGet("/hierarchy", async (DapperMenuRepository dapperRepo) =>
        {
            var result = await dapperRepo.GetMenuHierarchyAsync();
            return Results.Ok(result);
        });

        group.MapGet("/role/{roleId:long}", async (long roleId, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetMenusByRoleQuery(roleId));
            return Results.Ok(result);
        });
    }
}
