using MediatR;
using Microsoft.AspNetCore.Authorization;
using SciTransactional.Application.Commands.CloseNorm;
using SciTransactional.Application.Commands.CreateAdvanceLicense;
using SciTransactional.Application.Commands.CreateAutoMail;
using SciTransactional.Application.Commands.CreateDirectEntry;
using SciTransactional.Application.Commands.CreateNavigation;
using SciTransactional.Application.Commands.CreateNorm;
using SciTransactional.Application.Commands.CreateOrderMap;
using SciTransactional.Application.Commands.UpdateAdvanceLicense;
using SciTransactional.Application.Commands.UpdateNavigation;
using SciTransactional.Application.DTOs;
using SciTransactional.Application.Queries.GetAdvanceLicenseById;
using SciTransactional.Application.Queries.GetAllAdvanceLicenses;
using SciTransactional.Application.Queries.GetAllNavigations;
using SciTransactional.Application.Queries.GetAllNorms;
using SciTransactional.Application.Queries.GetAutoMailStatus;
using SciTransactional.Application.Queries.GetDirectEntries;
using SciTransactional.Application.Queries.GetNavigationById;
using SciTransactional.Application.Queries.GetNormById;
using SciTransactional.Application.Queries.GetOrderMaps;

namespace SciTransactional.API.MinimalApis;

public static class TransactionalEndpoints
{
    public static void MapTransactionalEndpoints(this IEndpointRouteBuilder app)
    {
        // ── Navigations ──
        var nav = app.MapGroup("/api/v2/navigations")
            .WithTags("Navigations-MinimalApi")
            .RequireAuthorization();

        nav.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllNavigationsQuery(), ct);
            return Results.Ok(result);
        }).Produces<IReadOnlyList<NavigationDto>>();

        nav.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetNavigationByIdQuery(id), ct);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        }).Produces<NavigationDto>().Produces(StatusCodes.Status404NotFound);

        nav.MapPost("/", async (CreateNavigationCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/navigations/{id}", id);
        }).Produces<long>(StatusCodes.Status201Created);

        nav.MapPut("/{id:long}/status", async (long id, UpdateNavigationCommand command, IMediator mediator, CancellationToken ct) =>
        {
            if (id != command.RequestNum)
                return Results.BadRequest("ID mismatch.");
            await mediator.Send(command, ct);
            return Results.NoContent();
        });

        // ── Norms ──
        var norms = app.MapGroup("/api/v2/norms")
            .WithTags("Norms-MinimalApi")
            .RequireAuthorization();

        norms.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllNormsQuery(), ct);
            return Results.Ok(result);
        }).Produces<IReadOnlyList<NormsMainDto>>();

        norms.MapGet("/{normNo:long}", async (long normNo, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetNormByIdQuery(normNo), ct);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        }).Produces<NormsMainDto>().Produces(StatusCodes.Status404NotFound);

        norms.MapPost("/", async (CreateNormCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/norms/{id}", id);
        }).Produces<long>(StatusCodes.Status201Created);

        norms.MapPost("/{normNo:long}/close", async (long normNo, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new CloseNormCommand(normNo), ct);
            return Results.NoContent();
        });

        // ── Advance Licenses ──
        var licenses = app.MapGroup("/api/v2/licenses")
            .WithTags("Licenses-MinimalApi")
            .RequireAuthorization();

        licenses.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllAdvanceLicensesQuery(), ct);
            return Results.Ok(result);
        }).Produces<IReadOnlyList<AdvanceLicenseDto>>();

        licenses.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAdvanceLicenseByIdQuery(id), ct);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        }).Produces<AdvanceLicenseDto>().Produces(StatusCodes.Status404NotFound);

        licenses.MapPost("/", async (CreateAdvanceLicenseCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/licenses/{id}", id);
        }).Produces<long>(StatusCodes.Status201Created);

        licenses.MapPut("/{id:long}", async (long id, UpdateAdvanceLicenseCommand command, IMediator mediator, CancellationToken ct) =>
        {
            if (id != command.LicenseId)
                return Results.BadRequest("ID mismatch.");
            await mediator.Send(command, ct);
            return Results.NoContent();
        });

        // ── Auto Mail ──
        var mail = app.MapGroup("/api/v2/automail")
            .WithTags("AutoMail-MinimalApi")
            .RequireAuthorization();

        mail.MapGet("/status", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAutoMailStatusQuery(), ct);
            return Results.Ok(result);
        }).Produces<IReadOnlyList<AutoMailStatusDto>>();

        mail.MapPost("/status", async (CreateAutoMailStatusCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/automail/status/{id}", id);
        }).Produces<int>(StatusCodes.Status201Created);

        // ── Order Maps ──
        var maps = app.MapGroup("/api/v2/ordermaps")
            .WithTags("OrderMaps-MinimalApi")
            .RequireAuthorization();

        maps.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetOrderMapsQuery(), ct);
            return Results.Ok(result);
        }).Produces<IReadOnlyList<OrderMapDto>>();

        maps.MapPost("/", async (CreateOrderMapCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/ordermaps/{id}", id);
        }).Produces<int>(StatusCodes.Status201Created);

        // ── Direct Entries ──
        var entries = app.MapGroup("/api/v2/directentries")
            .WithTags("DirectEntries-MinimalApi")
            .RequireAuthorization();

        entries.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetDirectEntriesQuery(), ct);
            return Results.Ok(result);
        }).Produces<IReadOnlyList<DirectEntryDto>>();

        entries.MapPost("/", async (CreateDirectEntryCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/directentries/{id}", id);
        }).Produces<long>(StatusCodes.Status201Created);
    }
}
