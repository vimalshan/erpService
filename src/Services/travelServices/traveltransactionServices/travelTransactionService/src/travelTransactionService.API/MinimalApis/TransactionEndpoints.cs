using MediatR;
using travelTransactionService.Application.Commands;
using travelTransactionService.Application.DTOs;
using travelTransactionService.Application.Queries;

namespace travelTransactionService.API.MinimalApis;

public static class TransactionEndpoints
{
    public static IEndpointRouteBuilder MapTransactionEndpoints(this IEndpointRouteBuilder app)
    {
        var vendors = app.MapGroup("/api/v2/vendors")
            .WithTags("Vendors-MinimalApi")
            .RequireAuthorization();

        vendors.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllVendorsQuery(), ct);
            return Results.Ok(result);
        })
        .WithName("GetAllVendorsV2")
        .Produces<IReadOnlyList<VendorMasterDto>>();

        vendors.MapGet("/{vendorId}", async (long vendorId, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetVendorByIdQuery(vendorId), ct);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        })
        .WithName("GetVendorByIdV2")
        .Produces<VendorMasterDto>()
        .Produces(StatusCodes.Status404NotFound);

        vendors.MapPost("/", async (CreateVendorCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/vendors/{result.VendorId}", result);
        })
        .WithName("CreateVendorV2")
        .Produces<VendorMasterDto>(StatusCodes.Status201Created);

        vendors.MapPut("/{vendorId}", async (long vendorId, UpdateVendorCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var updatedCommand = command with { VendorId = vendorId };
            var result = await mediator.Send(updatedCommand, ct);
            return Results.Ok(new { Success = result });
        })
        .WithName("UpdateVendorV2");

        vendors.MapDelete("/{vendorId}", async (long vendorId, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new DeleteVendorCommand(vendorId), ct);
            return Results.Ok(new { Success = result });
        })
        .WithName("DeleteVendorV2");

        var taxMasters = app.MapGroup("/api/v2/tax-masters")
            .WithTags("TaxMasters-MinimalApi")
            .RequireAuthorization();

        taxMasters.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllTaxMastersQuery(), ct);
            return Results.Ok(result);
        })
        .WithName("GetAllTaxMastersV2")
        .Produces<IReadOnlyList<TaxMasterDto>>();

        taxMasters.MapGet("/{taxType}", async (string taxType, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetTaxMasterByTypeQuery(taxType), ct);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        })
        .WithName("GetTaxMasterByTypeV2")
        .Produces<TaxMasterDto>()
        .Produces(StatusCodes.Status404NotFound);

        taxMasters.MapPost("/", async (CreateTaxMasterCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/tax-masters/{result.TaxType}", result);
        })
        .WithName("CreateTaxMasterV2")
        .Produces<TaxMasterDto>(StatusCodes.Status201Created);

        var lookups = app.MapGroup("/api/v2/transaction-lookups")
            .WithTags("TransactionLookups-MinimalApi")
            .RequireAuthorization();

        lookups.MapGet("/account-masters", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllAccountMastersQuery(), ct);
            return Results.Ok(result);
        })
        .WithName("GetAccountMastersV2");

        lookups.MapGet("/gl-code-combinations", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllGlCodeCombinationsQuery(), ct);
            return Results.Ok(result);
        })
        .WithName("GetGlCodeCombinationsV2");

        lookups.MapGet("/source-history", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllSourceHistoryQuery(), ct);
            return Results.Ok(result);
        })
        .WithName("GetSourceHistoryV2");

        return app;
    }
}
