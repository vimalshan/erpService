namespace TransactionService.API.GraphQL;

using MediatR;
using TransactionService.Application.DTOs;
using TransactionService.Application.ExternalServices;
using TransactionService.Application.Queries.GetBudget;
using TransactionService.Application.Queries.GetOrders;
using TransactionService.Application.Queries.GetRequests;

public sealed class Query
{
    // Requests
    public async Task<IEnumerable<RequestSummaryDto>> GetRequests(
        [Service] IMediator mediator,
        long? locationId = null,
        CancellationToken ct = default)
    {
        return await mediator.Send(new GetAllRequestsQuery(locationId), ct);
    }

    public async Task<RequestMainDto?> GetRequestById(
        long requestId,
        [Service] IMediator mediator,
        CancellationToken ct = default)
    {
        return await mediator.Send(new GetRequestByIdQuery(requestId), ct);
    }

    public async Task<IEnumerable<RequestSummaryDto>> GetRequestsByEmployee(
        long empSysId,
        [Service] IMediator mediator,
        CancellationToken ct = default)
    {
        return await mediator.Send(new GetRequestsByEmployeeQuery(empSysId), ct);
    }

    // Orders
    public async Task<IEnumerable<OrderSummaryDto>> GetOrders(
        [Service] IMediator mediator,
        long? locationId = null,
        CancellationToken ct = default)
    {
        return await mediator.Send(new GetAllOrdersQuery(locationId), ct);
    }

    public async Task<OrderMainDto?> GetOrderById(
        long orderMainId,
        [Service] IMediator mediator,
        CancellationToken ct = default)
    {
        return await mediator.Send(new GetOrderByIdQuery(orderMainId), ct);
    }

    public async Task<IEnumerable<OrderSummaryDto>> GetOrdersByVendor(
        long vendorId,
        [Service] IMediator mediator,
        CancellationToken ct = default)
    {
        return await mediator.Send(new GetOrdersByVendorQuery(vendorId), ct);
    }

    // Budgets
    public async Task<BudgetSummaryDto?> GetDeptBudget(
        long locationId, long deptId, long finYearId,
        [Service] IMediator mediator,
        CancellationToken ct = default)
    {
        return await mediator.Send(new GetDeptBudgetQuery(locationId, deptId, finYearId), ct);
    }

    public async Task<IEnumerable<DeptBudgetDto>> GetDeptBudgetsByLocation(
        long locationId, long finYearId,
        [Service] IMediator mediator,
        CancellationToken ct = default)
    {
        return await mediator.Send(new GetDeptBudgetsByLocationQuery(locationId, finYearId), ct);
    }

    // ── External Service Lookups ──

    public async Task<IReadOnlyList<VendorDto>> GetVendors(
        [Service] IVendorServiceClient client,
        string? status = null,
        CancellationToken ct = default)
    {
        char? statusChar = status?.Length > 0 ? status[0] : null;
        return await client.GetAllVendorsAsync(statusChar, ct);
    }

    public async Task<VendorDto?> GetVendorById(
        long vendorId,
        [Service] IVendorServiceClient client,
        CancellationToken ct = default)
    {
        return await client.GetVendorByIdAsync(vendorId, ct);
    }

    public async Task<IReadOnlyList<LocationAppMapDto>> GetLocations(
        [Service] ILocationServiceClient client,
        CancellationToken ct = default)
    {
        return await client.GetActiveLocationsAsync(ct);
    }

    public async Task<IReadOnlyList<StationeryItemDto>> GetStationeryItems(
        [Service] IStationeryServiceClient client,
        long? locationId = null,
        CancellationToken ct = default)
    {
        return locationId.HasValue
            ? await client.GetItemsByLocationAsync(locationId.Value, ct)
            : await client.GetAllItemsAsync(ct);
    }

    public async Task<IReadOnlyList<FinancialYearDto>> GetFinancialYears(
        [Service] IFinyearServiceClient client,
        CancellationToken ct = default)
    {
        return await client.GetAllFinancialYearsAsync(ct);
    }

    public async Task<FinancialYearDto?> GetCurrentFinancialYear(
        [Service] IFinyearServiceClient client,
        CancellationToken ct = default)
    {
        return await client.GetCurrentFinancialYearAsync(ct);
    }

    public async Task<IReadOnlyList<LovTypeDto>> GetLovTypes(
        [Service] ILovServiceClient client,
        CancellationToken ct = default)
    {
        return await client.GetAllLovTypesAsync(ct);
    }

    public async Task<IReadOnlyList<LovMasterDto>> GetLovMastersByType(
        long lovTypeId,
        [Service] ILovServiceClient client,
        CancellationToken ct = default)
    {
        return await client.GetLovMastersByTypeAsync(lovTypeId, ct);
    }

    public async Task<IReadOnlyList<ItemDataDto>> GetItemData(
        [Service] ILovServiceClient client,
        string? catName = null,
        string? itemName = null,
        CancellationToken ct = default)
    {
        return (catName is not null || itemName is not null)
            ? await client.SearchItemDataAsync(catName, itemName, ct)
            : await client.GetAllItemDataAsync(ct);
    }
}
