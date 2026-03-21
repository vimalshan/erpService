namespace TransactionService.Infrastructure.ExternalServices;

using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using TransactionService.Application.ExternalServices;

public sealed class FinyearServiceClient : IFinyearServiceClient
{
    private readonly HttpClient _http;
    private readonly ILogger<FinyearServiceClient> _logger;

    public FinyearServiceClient(HttpClient http, ILogger<FinyearServiceClient> logger)
    {
        _http = http;
        _logger = logger;
    }

    public async Task<FinancialYearDto?> GetCurrentFinancialYearAsync(CancellationToken ct = default)
    {
        try
        {
            return await _http.GetFromJsonAsync<FinancialYearDto>("api/financialyear/current", ct);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "Failed to fetch current financial year from FinyearService");
            return null;
        }
    }

    public async Task<FinancialYearDto?> GetFinancialYearByIdAsync(long id, CancellationToken ct = default)
    {
        try
        {
            return await _http.GetFromJsonAsync<FinancialYearDto>($"api/financialyear/{id}", ct);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "Failed to fetch financial year {Id} from FinyearService", id);
            return null;
        }
    }

    public async Task<IReadOnlyList<FinancialYearDto>> GetAllFinancialYearsAsync(CancellationToken ct = default)
    {
        try
        {
            var result = await _http.GetFromJsonAsync<List<FinancialYearDto>>("api/financialyear", ct);
            return result ?? [];
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "Failed to fetch financial years from FinyearService");
            return [];
        }
    }
}
