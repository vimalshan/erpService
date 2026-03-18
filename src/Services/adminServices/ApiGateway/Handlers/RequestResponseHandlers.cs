using System.Net;
using Microsoft.Extensions.Caching.Memory;

namespace ApiGateway.Handlers;

/// <summary>
/// Custom HTTP handler for request processing and delegation
/// </summary>
public class GatewayHttpHandler : DelegatingHandler
{
    private readonly ILogger<GatewayHttpHandler> _logger;

    public GatewayHttpHandler(ILogger<GatewayHttpHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var correlationId = request.Headers.Contains("X-Correlation-ID") 
            ? request.Headers.GetValues("X-Correlation-ID").First()
            : Guid.NewGuid().ToString();

        // Add correlation ID if not present
        if (!request.Headers.Contains("X-Correlation-ID"))
        {
            request.Headers.Add("X-Correlation-ID", correlationId);
        }

        // Add request ID
        request.Headers.Add("X-Request-ID", Guid.NewGuid().ToString());

        // Log outgoing request
        _logger.LogInformation(
            "Outgoing request [{CorrelationId}]: {Method} {Uri}",
            correlationId, request.Method, request.RequestUri);

        try
        {
            var response = await base.SendAsync(request, cancellationToken);

            // Log response
            _logger.LogInformation(
                "Received response [{CorrelationId}]: {StatusCode} from {Uri}",
                correlationId, response.StatusCode, request.RequestUri);

            return response;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex,
                "Request [{CorrelationId}]: Failed with exception {Message}",
                correlationId, ex.Message);
            throw;
        }
    }
}

/// <summary>
/// Response transformation handler for normalizing responses
/// </summary>
public class ResponseTransformationHandler : DelegatingHandler
{
    private readonly ILogger<ResponseTransformationHandler> _logger;

    public ResponseTransformationHandler(ILogger<ResponseTransformationHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        // Add response metadata headers
        if (!response.Headers.Contains("X-Response-Time"))
        {
            response.Headers.Add("X-Response-Time", DateTime.UtcNow.ToString("o"));
        }

        // Add service information if available
        if (request.Headers.Contains("X-Service"))
        {
            var serviceName = request.Headers.GetValues("X-Service").First();
            response.Headers.Add("X-Service-Response", serviceName);
        }

        return response;
    }
}

/// <summary>
/// Retry and circuit breaker handler
/// </summary>
public class ResilienceHandler : DelegatingHandler
{
    private readonly ILogger<ResilienceHandler> _logger;
    private readonly int _maxRetries;
    private readonly int _timeoutSeconds;

    public ResilienceHandler(ILogger<ResilienceHandler> logger, int maxRetries = 3, int timeoutSeconds = 10)
    {
        _logger = logger;
        _maxRetries = maxRetries;
        _timeoutSeconds = timeoutSeconds;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        int retryCount = 0;
        HttpResponseMessage? response = null;

        while (retryCount <= _maxRetries)
        {
            try
            {
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                cts.CancelAfter(TimeSpan.FromSeconds(_timeoutSeconds));

                response = await base.SendAsync(request, cts.Token);

                // Return on success
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }

                // Retry on specific status codes
                if (IsRetryableStatusCode(response.StatusCode))
                {
                    if (retryCount < _maxRetries)
                    {
                        _logger.LogWarning(
                            "Retrying request (attempt {RetryCount}/{MaxRetries}): {StatusCode} {Uri}",
                            retryCount + 1, _maxRetries, response.StatusCode, request.RequestUri);

                        await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, retryCount)), cancellationToken);
                        retryCount++;
                        continue;
                    }
                }

                return response;
            }
            catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
            {
                // Timeout occurred
                _logger.LogWarning(
                    "Request timeout (attempt {RetryCount}/{MaxRetries}): {Uri}",
                    retryCount + 1, _maxRetries, request.RequestUri);

                if (retryCount < _maxRetries)
                {
                    await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, retryCount)), cancellationToken);
                    retryCount++;
                    continue;
                }

                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex,
                    "Request failed (attempt {RetryCount}/{MaxRetries}): {Uri}",
                    retryCount + 1, _maxRetries, request.RequestUri);

                if (retryCount < _maxRetries)
                {
                    await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, retryCount)), cancellationToken);
                    retryCount++;
                    continue;
                }

                throw;
            }
        }

        return response ?? throw new InvalidOperationException("No response received");
    }

    private static bool IsRetryableStatusCode(HttpStatusCode statusCode) =>
        statusCode == HttpStatusCode.RequestTimeout ||
        statusCode == HttpStatusCode.ServiceUnavailable ||
        statusCode == HttpStatusCode.GatewayTimeout ||
        statusCode == HttpStatusCode.TooManyRequests;
}

/// <summary>
/// Request caching handler for GET requests
/// </summary>
public class CachingHandler : DelegatingHandler
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<CachingHandler> _logger;
    private readonly int _cacheDurationSeconds;

    public CachingHandler(IMemoryCache cache, ILogger<CachingHandler> logger, int cacheDurationSeconds = 60)
    {
        _cache = cache;
        _logger = logger;
        _cacheDurationSeconds = cacheDurationSeconds;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Only cache GET requests
        if (request.Method != HttpMethod.Get)
        {
            return await base.SendAsync(request, cancellationToken);
        }

        var cacheKey = $"http_{request.RequestUri?.AbsoluteUri}";

        if (_cache.TryGetValue(cacheKey, out HttpResponseMessage? cachedResponse))
        {
            _logger.LogInformation("Cache hit for {Uri}", request.RequestUri);
            return cachedResponse!;
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode && response.Content != null)
        {
            _cache.Set(cacheKey, response, TimeSpan.FromSeconds(_cacheDurationSeconds));
            _logger.LogInformation("Cached response for {Uri}", request.RequestUri);
        }

        return response;
    }
}
