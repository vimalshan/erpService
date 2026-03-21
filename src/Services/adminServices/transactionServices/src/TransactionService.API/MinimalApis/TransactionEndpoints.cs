namespace TransactionService.API.MinimalApis;

using MediatR;
using TransactionService.Application.Commands.ApproveRequest;
using TransactionService.Application.Commands.CreateOrder;
using TransactionService.Application.Commands.ReceiveOrder;
using TransactionService.Application.Commands.SubmitRequest;
using TransactionService.Application.ExternalServices;
using TransactionService.Application.Queries.GetBudget;
using TransactionService.Application.Queries.GetOrders;
using TransactionService.Application.Queries.GetRequests;

public static class TransactionEndpoints
{
    public static IEndpointRouteBuilder MapTransactionMinimalApis(this IEndpointRouteBuilder app)
    {
        // Requests v2
        var requests = app.MapGroup("/api/v2/requests")
            .RequireAuthorization()
            .WithTags("Requests (Minimal API)");

        requests.MapGet("/", async (IMediator mediator, long? locationId, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllRequestsQuery(locationId), ct);
            return Results.Ok(result);
        }).WithName("GetRequestsV2").WithSummary("Get all requests");

        requests.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetRequestByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetRequestByIdV2").WithSummary("Get request by ID");

        requests.MapPost("/", async (SubmitRequestCommand cmd, IMediator mediator, CancellationToken ct) =>
        {
            var id = await mediator.Send(cmd, ct);
            return Results.Created($"/api/v2/requests/{id}", id);
        }).WithName("SubmitRequestV2").WithSummary("Submit a new stationery request");

        requests.MapPut("/{requestSubId:long}/approve", async (
            long requestSubId, ApproveRequestCommand cmd, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(cmd, ct);
            return result ? Results.NoContent() : Results.NotFound();
        }).WithName("ApproveRequestV2").WithSummary("Approve a request sub item");

        // Orders v2
        var orders = app.MapGroup("/api/v2/orders")
            .RequireAuthorization()
            .WithTags("Orders (Minimal API)");

        orders.MapGet("/", async (IMediator mediator, long? locationId, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllOrdersQuery(locationId), ct);
            return Results.Ok(result);
        }).WithName("GetOrdersV2").WithSummary("Get all orders");

        orders.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetOrderByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetOrderByIdV2").WithSummary("Get order by ID");

        orders.MapPost("/", async (CreateOrderCommand cmd, IMediator mediator, CancellationToken ct) =>
        {
            var id = await mediator.Send(cmd, ct);
            return Results.Created($"/api/v2/orders/{id}", id);
        }).WithName("CreateOrderV2").WithSummary("Create a purchase order");

        orders.MapPut("/{orderSubId:long}/receive", async (
            long orderSubId, ReceiveOrderCommand cmd, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(cmd, ct);
            return result ? Results.NoContent() : Results.NotFound();
        }).WithName("ReceiveOrderV2").WithSummary("Record order receipt");

        // Budgets v2
        var budgets = app.MapGroup("/api/v2/budgets")
            .RequireAuthorization()
            .WithTags("Budgets (Minimal API)");

        budgets.MapGet("/department", async (
            long locationId, long deptId, long finYearId,
            IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetDeptBudgetQuery(locationId, deptId, finYearId), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetDeptBudgetV2").WithSummary("Get department budget summary");

        // ── External Service Lookups (v2) ──
        var lookups = app.MapGroup("/api/v2/lookups")
            .RequireAuthorization()
            .WithTags("Lookups (External Services)");

        lookups.MapGet("/vendors", async (IVendorServiceClient client, char? status, CancellationToken ct) =>
        {
            var vendors = await client.GetAllVendorsAsync(status, ct);
            return Results.Ok(vendors);
        }).WithName("LookupVendorsV2").WithSummary("Get vendors from VendorService");

        lookups.MapGet("/vendors/{vendorId:long}", async (long vendorId, IVendorServiceClient client, CancellationToken ct) =>
        {
            var vendor = await client.GetVendorByIdAsync(vendorId, ct);
            return vendor is null ? Results.NotFound() : Results.Ok(vendor);
        }).WithName("LookupVendorByIdV2").WithSummary("Get vendor by ID from VendorService");

        lookups.MapGet("/locations", async (ILocationServiceClient client, CancellationToken ct) =>
        {
            var locations = await client.GetActiveLocationsAsync(ct);
            return Results.Ok(locations);
        }).WithName("LookupLocationsV2").WithSummary("Get active locations from LocationService");

        lookups.MapGet("/stationery-items", async (IStationeryServiceClient client, long? locationId, CancellationToken ct) =>
        {
            var items = locationId.HasValue
                ? await client.GetItemsByLocationAsync(locationId.Value, ct)
                : await client.GetAllItemsAsync(ct);
            return Results.Ok(items);
        }).WithName("LookupStationeryItemsV2").WithSummary("Get stationery items from StationeryService");

        lookups.MapGet("/financial-years", async (IFinyearServiceClient client, CancellationToken ct) =>
        {
            var years = await client.GetAllFinancialYearsAsync(ct);
            return Results.Ok(years);
        }).WithName("LookupFinancialYearsV2").WithSummary("Get financial years from FinyearService");

        lookups.MapGet("/current-finyear", async (IFinyearServiceClient client, CancellationToken ct) =>
        {
            var fy = await client.GetCurrentFinancialYearAsync(ct);
            return fy is null ? Results.NotFound() : Results.Ok(fy);
        }).WithName("LookupCurrentFinyearV2").WithSummary("Get current financial year from FinyearService");

        lookups.MapGet("/lov-types", async (ILovServiceClient client, CancellationToken ct) =>
        {
            var types = await client.GetAllLovTypesAsync(ct);
            return Results.Ok(types);
        }).WithName("LookupLovTypesV2").WithSummary("Get LOV types from LovService");

        lookups.MapGet("/lov-masters/{lovTypeId:long}", async (long lovTypeId, ILovServiceClient client, CancellationToken ct) =>
        {
            var masters = await client.GetLovMastersByTypeAsync(lovTypeId, ct);
            return Results.Ok(masters);
        }).WithName("LookupLovMastersV2").WithSummary("Get LOV masters by type from LovService");

        lookups.MapGet("/item-data", async (ILovServiceClient client, string? catName, string? itemName, CancellationToken ct) =>
        {
            var items = (catName is not null || itemName is not null)
                ? await client.SearchItemDataAsync(catName, itemName, ct)
                : await client.GetAllItemDataAsync(ct);
            return Results.Ok(items);
        }).WithName("LookupItemDataV2").WithSummary("Search item data from LovService");

        return app;
    }
}
