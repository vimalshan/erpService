namespace TransactionService.Infrastructure.ExternalServices;

using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using TransactionService.Application.ExternalServices;

public sealed class VendorServiceClient : IVendorServiceClient
{
    private readonly HttpClient _http;
    private readonly ILogger<VendorServiceClient> _logger;

    public VendorServiceClient(HttpClient http, ILogger<VendorServiceClient> logger)
    {
        _http = http;
        _logger = logger;
    }

    public async Task<VendorDto?> GetVendorByIdAsync(long vendorId, CancellationToken ct = default)
    {
        try
        {
            return await _http.GetFromJsonAsync<VendorDto>($"api/vendors/{vendorId}", ct);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "Failed to fetch vendor {VendorId} from VendorService", vendorId);
            return null;
        }
    }

    public async Task<IReadOnlyList<VendorDto>> GetAllVendorsAsync(char? status = null, CancellationToken ct = default)
    {
        try
        {
            var url = status.HasValue ? $"api/vendors?status={status.Value}" : "api/vendors";
            var result = await _http.GetFromJsonAsync<List<VendorDto>>(url, ct);
            return result ?? [];
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "Failed to fetch vendors from VendorService");
            return [];
        }
    }
}
