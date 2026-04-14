using Polly;
using Polly.Bulkhead;

namespace ApiGateway.DelegatingHandlers;

public class BulkheadDelegatingHandler : DelegatingHandler
{
    private readonly ILogger<BulkheadDelegatingHandler> _logger;
    private readonly IAsyncPolicy<HttpResponseMessage> _bulkheadPolicy;

    public BulkheadDelegatingHandler(ILogger<BulkheadDelegatingHandler> logger, IConfiguration configuration)
    {
        _logger = logger;

        var maxParallelization = configuration.GetValue("Bulkhead:MaxParallelization", 50);
        var maxQueuingActions = configuration.GetValue("Bulkhead:MaxQueuingActions", 25);

        _bulkheadPolicy = Policy.BulkheadAsync<HttpResponseMessage>(
            maxParallelization: maxParallelization,
            maxQueuingActions: maxQueuingActions,
            onBulkheadRejectedAsync: _ =>
            {
                _logger.LogWarning("Bulkhead limit reached: max {MaxParallel} concurrent, {MaxQueue} queued",
                    maxParallelization, maxQueuingActions);
                return Task.CompletedTask;
            });
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            return await _bulkheadPolicy.ExecuteAsync(ct => base.SendAsync(request, ct), cancellationToken);
        }
        catch (BulkheadRejectedException)
        {
            _logger.LogError("Request to {Uri} rejected by bulkhead - too many concurrent requests", request.RequestUri);
            return new HttpResponseMessage(System.Net.HttpStatusCode.TooManyRequests)
            {
                Content = new StringContent("{\"message\":\"Too many concurrent requests. Please try again later.\"}")
            };
        }
    }
}
