using MediatR;
using Microsoft.AspNetCore.Mvc;
using AdminService.Application.Commands.AdminMasters;
using AdminService.Application.Commands.UserMaps;
using AdminService.Application.Commands.FinUserMaps;
using AdminService.Application.Commands.AccessRights;
using AdminService.Application.Queries;

namespace AdminService.API.Endpoints;

public static class AdminMinimalApiEndpoints
{
    public static void MapAdminMinimalApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/minimal/admin")
            .WithTags("Admin Minimal API")
            .RequireAuthorization();

        // AdminMaster
        group.MapGet("/masters", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllAdminMastersQuery(), ct)));

        group.MapGet("/masters/{id}", async (string id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAdminMasterByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        group.MapPost("/masters", async ([FromBody] CreateAdminMasterCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/minimal/admin/masters/{result.AdminId}", result);
        });

        group.MapPut("/masters/{id}", async (string id, [FromBody] UpdateAdminMasterCommand command, IMediator mediator, CancellationToken ct) =>
        {
            if (id != command.AdminId) return Results.BadRequest("ID mismatch.");
            return Results.Ok(await mediator.Send(command, ct));
        });

        group.MapDelete("/masters/{id}", async (string id, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new DeleteAdminMasterCommand(id), ct);
            return Results.NoContent();
        });

        // UserMap
        group.MapGet("/usermaps", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllAdminUserMapsQuery(), ct)));

        group.MapPost("/usermaps", async ([FromBody] CreateAdminUserMapCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/minimal/admin/usermaps/{result.AdminMapId}", result);
        });

        // FinUserMap
        group.MapGet("/finusermaps", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllAdminFinUserMapsQuery(), ct)));

        group.MapPost("/finusermaps", async ([FromBody] CreateAdminFinUserMapCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/minimal/admin/finusermaps/{result.FinanceMapId}", result);
        });

        // AccessRights
        group.MapGet("/accessrights", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllAccessRightsQuery(), ct)));

        group.MapPost("/accessrights", async ([FromBody] CreateAccessRightsCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/minimal/admin/accessrights/{result.AdminRightsId}", result);
        });
    }
}
