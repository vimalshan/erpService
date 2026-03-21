namespace TransactionService.Infrastructure.ExternalServices;

using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using TransactionService.Application.ExternalServices;

public sealed class StationeryServiceClient : IStationeryServiceClient
{
    private readonly HttpClient _http;
    private readonly ILogger<StationeryServiceClient> _logger;

    public StationeryServiceClient(HttpClient http, ILogger<StationeryServiceClient> logger)
    {
        _http = http;
        _logger = logger;
    }

    public async Task<StationeryItemDto?> GetItemByIdAsync(long itemId, CancellationToken ct = default)
    {
        try
        {
            return await _http.GetFromJsonAsync<StationeryItemDto>($"api/v1/items/{itemId}", ct);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "Failed to fetch stationery item {ItemId} from StationeryService", itemId);
            return null;
        }
    }

    public async Task<IReadOnlyList<StationeryItemDto>> GetItemsByLocationAsync(long locationId, CancellationToken ct = default)
    {
        try
        {
            var result = await _http.GetFromJsonAsync<List<StationeryItemDto>>(
                $"api/v1/items?locationId={locationId}", ct);
            return result ?? [];
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "Failed to fetch stationery items for location {LocationId}", locationId);
            return [];
        }
    }

    public async Task<IReadOnlyList<StationeryItemDto>> GetAllItemsAsync(CancellationToken ct = default)
    {
        try
        {
            var result = await _http.GetFromJsonAsync<List<StationeryItemDto>>("api/v1/items", ct);
            return result ?? [];
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "Failed to fetch all stationery items from StationeryService");
            return [];
        }
    }
}
