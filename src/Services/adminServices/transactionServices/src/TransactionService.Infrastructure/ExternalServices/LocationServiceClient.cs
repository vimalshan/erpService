namespace TransactionService.Infrastructure.ExternalServices;

using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using TransactionService.Application.ExternalServices;

public sealed class LocationServiceClient : ILocationServiceClient
{
    private readonly HttpClient _http;
    private readonly ILogger<LocationServiceClient> _logger;

    public LocationServiceClient(HttpClient http, ILogger<LocationServiceClient> logger)
    {
        _http = http;
        _logger = logger;
    }

    public async Task<IReadOnlyList<LocationAppMapDto>> GetAllLocationsAsync(CancellationToken ct = default)
    {
        try
        {
            var result = await _http.GetFromJsonAsync<List<LocationAppMapDto>>("api/v1/location-app-maps", ct);
            return result ?? [];
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "Failed to fetch locations from LocationService");
            return [];
        }
    }

    public async Task<IReadOnlyList<LocationAppMapDto>> GetActiveLocationsAsync(CancellationToken ct = default)
    {
        try
        {
            var result = await _http.GetFromJsonAsync<List<LocationAppMapDto>>("api/v1/location-app-maps/active", ct);
            return result ?? [];
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "Failed to fetch active locations from LocationService");
            return [];
        }
    }

    public async Task<IReadOnlyList<LocationAppMapDto>> GetLocationsByIdAsync(decimal locationId, CancellationToken ct = default)
    {
        try
        {
            var result = await _http.GetFromJsonAsync<List<LocationAppMapDto>>(
                $"api/v1/location-app-maps/by-location/{locationId}", ct);
            return result ?? [];
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "Failed to fetch location {LocationId} from LocationService", locationId);
            return [];
        }
    }
}
