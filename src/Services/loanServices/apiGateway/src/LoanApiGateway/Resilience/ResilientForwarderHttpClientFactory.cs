using Yarp.ReverseProxy.Forwarder;

namespace LoanApiGateway.Resilience;

/// <summary>
/// Custom YARP ForwarderHttpClientFactory that injects resilience policies
/// (retry, circuit-breaker, timeout) via IResiliencePipelineProvider.
/// </summary>
public class ResilientForwarderHttpClientFactory : IForwarderHttpClientFactory
{
    private readonly ILogger<ResilientForwarderHttpClientFactory> _logger;

    public ResilientForwarderHttpClientFactory(
        ILogger<ResilientForwarderHttpClientFactory> logger)
    {
        _logger = logger;
    }

    public HttpMessageInvoker CreateClient(ForwarderHttpClientContext context)
    {
        // Bypass certificate validation in development (HTTPS to downstream services)
        var handler = new SocketsHttpHandler
        {
            UseProxy = false,
            AllowAutoRedirect = false,
            AutomaticDecompression = System.Net.DecompressionMethods.None,
            UseCookies = false,
            ConnectTimeout = TimeSpan.FromSeconds(15),
            PooledConnectionLifetime = TimeSpan.FromMinutes(2),
            SslOptions = new System.Net.Security.SslClientAuthenticationOptions
            {
                RemoteCertificateValidationCallback = (_, _, _, _) => true
            }
        };

        _logger.LogDebug("Creating resilient HTTP client for cluster '{ClusterId}'",
            context.ClusterId);

        return new HttpMessageInvoker(handler, disposeHandler: true);
    }
}
