namespace TransactionService.Application.ExternalServices;

/// <summary>
/// Client for VendorService (Port 5003) — vendor lookup for purchase orders.
/// </summary>
public interface IVendorServiceClient
{
    Task<VendorDto?> GetVendorByIdAsync(long vendorId, CancellationToken ct = default);
    Task<IReadOnlyList<VendorDto>> GetAllVendorsAsync(char? status = null, CancellationToken ct = default);
}

/// <summary>
/// Client for LocationService (Port 5002) — location data for requests/orders.
/// </summary>
public interface ILocationServiceClient
{
    Task<IReadOnlyList<LocationAppMapDto>> GetAllLocationsAsync(CancellationToken ct = default);
    Task<IReadOnlyList<LocationAppMapDto>> GetActiveLocationsAsync(CancellationToken ct = default);
    Task<IReadOnlyList<LocationAppMapDto>> GetLocationsByIdAsync(decimal locationId, CancellationToken ct = default);
}

/// <summary>
/// Client for FinyearService (Port 5001) — financial year for budget/request context.
/// </summary>
public interface IFinyearServiceClient
{
    Task<FinancialYearDto?> GetCurrentFinancialYearAsync(CancellationToken ct = default);
    Task<FinancialYearDto?> GetFinancialYearByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<FinancialYearDto>> GetAllFinancialYearsAsync(CancellationToken ct = default);
}

/// <summary>
/// Client for StationeryService (Port 5005) — stationery items for request line items.
/// </summary>
public interface IStationeryServiceClient
{
    Task<StationeryItemDto?> GetItemByIdAsync(long itemId, CancellationToken ct = default);
    Task<IReadOnlyList<StationeryItemDto>> GetItemsByLocationAsync(long locationId, CancellationToken ct = default);
    Task<IReadOnlyList<StationeryItemDto>> GetAllItemsAsync(CancellationToken ct = default);
}

/// <summary>
/// Client for LovService (Port 5007) — list of values for categories, dropdowns, etc.
/// </summary>
public interface ILovServiceClient
{
    Task<IReadOnlyList<LovTypeDto>> GetAllLovTypesAsync(CancellationToken ct = default);
    Task<IReadOnlyList<LovMasterDto>> GetLovMastersByTypeAsync(long lovTypeId, CancellationToken ct = default);
    Task<IReadOnlyList<ItemDataDto>> GetAllItemDataAsync(CancellationToken ct = default);
    Task<IReadOnlyList<ItemDataDto>> SearchItemDataAsync(string? catName = null, string? itemName = null, CancellationToken ct = default);
}
