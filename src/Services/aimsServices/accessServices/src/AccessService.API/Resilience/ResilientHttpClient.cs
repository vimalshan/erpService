namespace AccessService.API.Resilience;

using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

/// <summary>
/// Service for resilient HTTP calls with Polly policies applied
/// </summary>
public interface IResilientHttpClient
{
    Task<HttpResponseMessage> GetAsync(string url, CancellationToken cancellationToken = default);
    Task<HttpResponseMessage> PostAsync(string url, HttpContent content, CancellationToken cancellationToken = default);
    Task<HttpResponseMessage> PutAsync(string url, HttpContent content, CancellationToken cancellationToken = default);
    Task<HttpResponseMessage> DeleteAsync(string url, CancellationToken cancellationToken = default);
}

/// <summary>
/// Implementation of resilient HTTP client with Polly policies
/// </summary>
public class ResilientHttpClient : IResilientHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly Polly.IAsyncPolicy<HttpResponseMessage> _resiliencePolicy;
    private readonly ILogger<ResilientHttpClient> _logger;

    public ResilientHttpClient(HttpClient httpClient, ILogger<ResilientHttpClient> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _resiliencePolicy = ResiliencePolicy.CombinedResiliencePolicy(logger);
    }

    /// <summary>
    /// GET request with resilience policies applied
    /// </summary>
    public async Task<HttpResponseMessage> GetAsync(string url, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Making GET request to {url}");
        
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        return await _resiliencePolicy.ExecuteAsync(
            ct => _httpClient.SendAsync(request, ct),
            cancellationToken
        );
    }

    /// <summary>
    /// POST request with resilience policies applied
    /// </summary>
    public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Making POST request to {url}");
        
        using var request = new HttpRequestMessage(HttpMethod.Post, url) { Content = content };
        return await _resiliencePolicy.ExecuteAsync(
            ct => _httpClient.SendAsync(request, ct),
            cancellationToken
        );
    }

    /// <summary>
    /// PUT request with resilience policies applied
    /// </summary>
    public async Task<HttpResponseMessage> PutAsync(string url, HttpContent content, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Making PUT request to {url}");
        
        using var request = new HttpRequestMessage(HttpMethod.Put, url) { Content = content };
        return await _resiliencePolicy.ExecuteAsync(
            ct => _httpClient.SendAsync(request, ct),
            cancellationToken
        );
    }

    /// <summary>
    /// DELETE request with resilience policies applied
    /// </summary>
    public async Task<HttpResponseMessage> DeleteAsync(string url, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Making DELETE request to {url}");
        
        using var request = new HttpRequestMessage(HttpMethod.Delete, url);
        return await _resiliencePolicy.ExecuteAsync(
            ct => _httpClient.SendAsync(request, ct),
            cancellationToken
        );
    }
}
