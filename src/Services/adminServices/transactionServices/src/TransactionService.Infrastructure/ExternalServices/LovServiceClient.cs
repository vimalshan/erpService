namespace TransactionService.Infrastructure.ExternalServices;

using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using TransactionService.Application.ExternalServices;

public sealed class LovServiceClient : ILovServiceClient
{
    private readonly HttpClient _http;
    private readonly ILogger<LovServiceClient> _logger;

    public LovServiceClient(HttpClient http, ILogger<LovServiceClient> logger)
    {
        _http = http;
        _logger = logger;
    }

    public async Task<IReadOnlyList<LovTypeDto>> GetAllLovTypesAsync(CancellationToken ct = default)
    {
        try
        {
            var result = await _http.GetFromJsonAsync<List<LovTypeDto>>("api/v1/lov-types", ct);
            return result ?? [];
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "Failed to fetch LOV types from LovService");
            return [];
        }
    }

    public async Task<IReadOnlyList<LovMasterDto>> GetLovMastersByTypeAsync(long lovTypeId, CancellationToken ct = default)
    {
        try
        {
            var result = await _http.GetFromJsonAsync<List<LovMasterDto>>(
                $"api/v1/lov-masters/by-type/{lovTypeId}", ct);
            return result ?? [];
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "Failed to fetch LOV masters for type {TypeId} from LovService", lovTypeId);
            return [];
        }
    }

    public async Task<IReadOnlyList<ItemDataDto>> GetAllItemDataAsync(CancellationToken ct = default)
    {
        try
        {
            var result = await _http.GetFromJsonAsync<List<ItemDataDto>>("api/v1/item-data", ct);
            return result ?? [];
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "Failed to fetch item data from LovService");
            return [];
        }
    }

    public async Task<IReadOnlyList<ItemDataDto>> SearchItemDataAsync(
        string? catName = null, string? itemName = null, CancellationToken ct = default)
    {
        try
        {
            var queryParams = new List<string>();
            if (!string.IsNullOrEmpty(catName)) queryParams.Add($"catName={Uri.EscapeDataString(catName)}");
            if (!string.IsNullOrEmpty(itemName)) queryParams.Add($"itemName={Uri.EscapeDataString(itemName)}");

            var url = queryParams.Count > 0
                ? $"api/v1/item-data/search?{string.Join("&", queryParams)}"
                : "api/v1/item-data/search";

            var result = await _http.GetFromJsonAsync<List<ItemDataDto>>(url, ct);
            return result ?? [];
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "Failed to search item data from LovService");
            return [];
        }
    }
}
