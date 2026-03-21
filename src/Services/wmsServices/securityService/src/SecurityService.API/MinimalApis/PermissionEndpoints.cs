using MediatR;
using SecurityService.Application.DTOs;
using SecurityService.Application.Queries;

namespace SecurityService.API.MinimalApis;

public static class PermissionEndpoints
{
    public static void MapPermissionEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/minimal/permissions")
            .WithTags("Permissions (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllPermissionsQuery());
            return Results.Ok(result);
        }).WithName("GetAllPermissionsMinimal");

        group.MapGet("/{id}", async (int id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetPermissionByIdQuery(id));
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetPermissionByIdMinimal");

        group.MapGet("/module/{module}", async (string module, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetPermissionsByModuleQuery(module));
            return Results.Ok(result);
        }).WithName("GetPermissionsByModuleMinimal");
    }
}
