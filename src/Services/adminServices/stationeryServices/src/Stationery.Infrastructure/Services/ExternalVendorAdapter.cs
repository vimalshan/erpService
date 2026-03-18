using Polly.CircuitBreaker;
using Polly.Retry;
using Stationery.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace Stationery.Infrastructure.Services;

public class ExternalVendorAdapter : IVendorAdapter
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ExternalVendorAdapter> _logger;
    private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;
    private readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> _circuitBreakerPolicy;

    public ExternalVendorAdapter(
        HttpClient httpClient, 
        ILogger<ExternalVendorAdapter> logger,
        AsyncRetryPolicy<HttpResponseMessage> retryPolicy,
        AsyncCircuitBreakerPolicy<HttpResponseMessage> circuitBreakerPolicy)
    {
        _httpClient = httpClient;
        _logger = logger;
        _retryPolicy = retryPolicy;
        _circuitBreakerPolicy = circuitBreakerPolicy;
    }

    public async Task<bool> SubmitOrderAsync(long orderId, List<VendorItemDto> items)
    {
        _logger.LogInformation("Submitting order {OrderId} to external vendor...", orderId);

        try
        {
            // Execute with combined retry and circuit breaker policies
            var response = await _retryPolicy.ExecuteAsync(() => 
                _circuitBreakerPolicy.ExecuteAsync(() => 
                    _httpClient.PostAsJsonAsync("https://api.vendor.com/v1/orders", new { orderId, items })
                )
            );

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Order {OrderId} successfully submitted.", orderId);
                return true;
            }
        }
        catch (BrokenCircuitException)
        {
            _logger.LogCritical("Circuit is OPEN! Order {OrderId} submission skipped to avoid overwhelming vendor.", orderId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to submit order {OrderId} after retries.", orderId);
        }

        return false;
    }
}
