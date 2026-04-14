using Polly;
using Polly.CircuitBreaker;

namespace ApiGateway.DelegatingHandlers;

public class CircuitBreakerDelegatingHandler : DelegatingHandler
{
    private readonly ILogger<CircuitBreakerDelegatingHandler> _logger;
    private readonly IAsyncPolicy<HttpResponseMessage> _circuitBreakerPolicy;

    public CircuitBreakerDelegatingHandler(ILogger<CircuitBreakerDelegatingHandler> logger, IConfiguration configuration)
    {
        _logger = logger;

        var exceptionsAllowed = configuration.GetValue("CircuitBreaker:ExceptionsAllowedBeforeBreaking", 3);
        var breakDuration = configuration.GetValue("CircuitBreaker:DurationOfBreakInSeconds", 30);

        _circuitBreakerPolicy = Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .OrResult(r => (int)r.StatusCode >= 500)
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: exceptionsAllowed,
                durationOfBreak: TimeSpan.FromSeconds(breakDuration),
                onBreak: (outcome, timespan) =>
                {
                    _logger.LogWarning("Circuit breaker OPEN for {Duration}s. Reason: {Reason}",
                        timespan.TotalSeconds, outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString());
                },
                onReset: () =>
                {
                    _logger.LogInformation("Circuit breaker RESET - service recovered");
                },
                onHalfOpen: () =>
                {
                    _logger.LogInformation("Circuit breaker HALF-OPEN - testing service");
                });
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            return await _circuitBreakerPolicy.ExecuteAsync(ct => base.SendAsync(request, ct), cancellationToken);
        }
        catch (BrokenCircuitException ex)
        {
            _logger.LogError("Circuit is open! Request to {Uri} blocked. {Message}", request.RequestUri, ex.Message);
            return new HttpResponseMessage(System.Net.HttpStatusCode.ServiceUnavailable)
            {
                Content = new StringContent("{\"message\":\"Service temporarily unavailable. Circuit breaker is open.\"}")
            };
        }
    }
}
