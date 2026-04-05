using Yarp.ReverseProxy.Forwarder;

namespace ERPGateway.Resilience;

/// <summary>
/// Custom <see cref="IForwarderHttpClientFactory"/> that routes each YARP cluster
/// through its own named <see cref="HttpClient"/>, each pre-configured with an
/// independent Polly resilience pipeline (circuit-breaker, retry, timeout).
///
/// This provides true per-service bulkhead isolation:
///   - A circuit open on the audit service does NOT affect the batch service.
///   - Retry state is tracked separately per service.
///
/// Naming convention:  cluster "audit-cluster"  →  client "yarp-audit"
///                     cluster "batch-cluster"  →  client "yarp-batch"
///                     (takes everything before the first '-' in the cluster ID)
/// </summary>
public sealed class ResilientForwarderHttpClientFactory : IForwarderHttpClientFactory
{
    private readonly IHttpClientFactory                          _factory;
    private readonly ILogger<ResilientForwarderHttpClientFactory> _logger;

    public ResilientForwarderHttpClientFactory(
        IHttpClientFactory                          factory,
        ILogger<ResilientForwarderHttpClientFactory> logger)
    {
        _factory = factory;
        _logger  = logger;
    }

    /// <summary>
    /// Called by YARP once per cluster when config loads or changes.
    /// Returns an <see cref="HttpMessageInvoker"/> backed by the per-service
    /// resilience pipeline registered in Program.cs.
    /// </summary>
    public HttpMessageInvoker CreateClient(ForwarderHttpClientContext context)
    {
        // "audit-cluster" → "audit"
        var clusterId  = context.ClusterId;
        var dashIndex  = clusterId.IndexOf('-');
        var svcKey     = dashIndex > 0 ? clusterId[..dashIndex] : clusterId;
        var clientName = $"yarp-{svcKey}";

        _logger.LogDebug(
            "[Gateway] Creating HttpClient '{ClientName}' for cluster '{ClusterId}'",
            clientName, clusterId);

        // IHttpClientFactory returns a properly pooled, handler-managed HttpClient.
        return _factory.CreateClient(clientName);
    }
}
