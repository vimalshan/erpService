using Polly;

namespace ApiGateway.DelegatingHandlers;

public class RetryDelegatingHandler : DelegatingHandler
{
    private readonly ILogger<RetryDelegatingHandler> _logger;
    private readonly IAsyncPolicy<HttpResponseMessage> _retryPolicy;

    public RetryDelegatingHandler(ILogger<RetryDelegatingHandler> logger, IConfiguration configuration)
    {
        _logger = logger;

        var maxRetries = configuration.GetValue("Retry:MaxRetryAttempts", 3);
        var delay = configuration.GetValue("Retry:DelayInMilliseconds", 500);

        _retryPolicy = Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .OrResult(r => (int)r.StatusCode >= 500)
            .WaitAndRetryAsync(
                retryCount: maxRetries,
                sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(delay * Math.Pow(2, attempt - 1)),
                onRetry: (outcome, timespan, retryAttempt, _) =>
                {
                    _logger.LogWarning(
                        "Retry {RetryAttempt}/{MaxRetries} after {Delay}ms for {Reason}",
                        retryAttempt, maxRetries, timespan.TotalMilliseconds,
                        outcome.Exception?.Message ?? $"HTTP {(int)outcome.Result.StatusCode}");
                });
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return await _retryPolicy.ExecuteAsync(ct => base.SendAsync(request, ct), cancellationToken);
    }
}
