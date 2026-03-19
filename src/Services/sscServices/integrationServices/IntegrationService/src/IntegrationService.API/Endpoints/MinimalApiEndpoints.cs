using IntegrationService.Application.DTOs;
using IntegrationService.Application.OrganizationUnits.Commands;
using IntegrationService.Application.OrganizationUnits.Queries;
using IntegrationService.Application.PurchaseOrders.Commands;
using IntegrationService.Application.PurchaseOrders.Queries;
using IntegrationService.Application.Vendors.Commands;
using IntegrationService.Application.Vendors.Queries;
using IntegrationService.Infrastructure.Dapper;
using MediatR;

namespace IntegrationService.API.Endpoints;

public static class MinimalApiEndpoints
{
    public static WebApplication MapMinimalApiEndpoints(this WebApplication app)
    {
        var api = app.MapGroup("/api/v2").RequireAuthorization();

        // PO endpoints
        api.MapGet("/purchase-orders", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllPurchaseOrdersQuery(), ct)))
            .WithName("GetAllPurchaseOrdersV2")
            .WithTags("PurchaseOrders");

        api.MapGet("/purchase-orders/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetPurchaseOrderByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetPurchaseOrderByIdV2")
        .WithTags("PurchaseOrders");

        api.MapPost("/purchase-orders", async (CreatePurchaseOrderCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/purchase-orders/{result.PoSeqId}", result);
        })
        .WithName("CreatePurchaseOrderV2")
        .WithTags("PurchaseOrders");

        api.MapDelete("/purchase-orders/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new DeletePurchaseOrderCommand(id), ct);
            return Results.NoContent();
        })
        .WithName("DeletePurchaseOrderV2")
        .WithTags("PurchaseOrders");

        // Vendor endpoints
        api.MapGet("/vendors", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllVendorsQuery(), ct)))
            .WithName("GetAllVendorsV2")
            .WithTags("Vendors");

        api.MapGet("/vendors/{id:int}", async (int id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetVendorByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetVendorByIdV2")
        .WithTags("Vendors");

        api.MapPost("/vendors", async (CreateVendorCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/vendors/{result.VendorId}", result);
        })
        .WithName("CreateVendorV2")
        .WithTags("Vendors");

        // OU endpoints
        api.MapGet("/organization-units", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllOrganizationUnitsQuery(), ct)))
            .WithName("GetAllOrganizationUnitsV2")
            .WithTags("OrganizationUnits");

        api.MapGet("/organization-units/{id}", async (string id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetOrganizationUnitByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetOrganizationUnitByIdV2")
        .WithTags("OrganizationUnits");

        // Dapper read-optimized endpoints
        var dapperGroup = app.MapGroup("/api/v2/dapper").RequireAuthorization();

        dapperGroup.MapGet("/purchase-orders", async (IDapperQueryService dapperService, CancellationToken ct) =>
            Results.Ok(await dapperService.GetPurchaseOrdersAsync(ct)))
            .WithName("DapperGetAllPurchaseOrders")
            .WithTags("Dapper");

        dapperGroup.MapGet("/purchase-orders/{id:long}", async (long id, IDapperQueryService dapperService, CancellationToken ct) =>
        {
            var result = await dapperService.GetPurchaseOrderByIdAsync(id, ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("DapperGetPurchaseOrderById")
        .WithTags("Dapper");

        dapperGroup.MapGet("/vendors", async (IDapperQueryService dapperService, CancellationToken ct) =>
            Results.Ok(await dapperService.GetVendorsAsync(ct)))
            .WithName("DapperGetAllVendors")
            .WithTags("Dapper");

        dapperGroup.MapGet("/organization-units", async (IDapperQueryService dapperService, CancellationToken ct) =>
            Results.Ok(await dapperService.GetOrganizationUnitsAsync(ct)))
            .WithName("DapperGetAllOrganizationUnits")
            .WithTags("Dapper");

        return app;
    }
}
