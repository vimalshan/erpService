using System.Text;
using System.Text.Json;
using System.Threading.RateLimiting;
using ERPGateway.HealthChecks;
using ERPGateway.Middleware;
using ERPGateway.Resilience;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.IdentityModel.Tokens;
using Polly;

var builder = WebApplication.CreateBuilder(args);
var config  = builder.Configuration;

// ─────────────────────────────────────────────────────────────────────────────
//  1. YARP Reverse Proxy
//     Routes and clusters are defined in appsettings.json → "ReverseProxy".
//     Active / Passive health checks and Round-Robin load balancing are
//     configured per-cluster there.
// ─────────────────────────────────────────────────────────────────────────────
builder.Services.AddReverseProxy()
    .LoadFromConfig(config.GetSection("ReverseProxy"));
// Active health checks (30 s polling) and passive health checks are enabled
// per-cluster inside appsettings.json → ReverseProxy.Clusters[*].HealthCheck.

// ─────────────────────────────────────────────────────────────────────────────
//  2. Per-Service Resilient HTTP Clients (Circuit-Breaker + Retry + Timeout)
//     Each downstream service gets its own named HttpClient so that Polly
//     state (circuit-breaker counters, retry history) is isolated per service.
//     The custom IForwarderHttpClientFactory (see Resilience/ folder) routes
//     each YARP cluster to the correct named client.
// ─────────────────────────────────────────────────────────────────────────────
var serviceKeys = new[]
{
    "audit", "batch", "csa", "project", "risk", "team", "timesheet", "workorder"
};

foreach (var svcKey in serviceKeys)
{
    var capturedKey = svcKey;   // capture for lambda closure

    builder.Services
        .AddHttpClient($"yarp-{capturedKey}")
        .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
        {
            MaxConnectionsPerServer       = 256,
            PooledConnectionLifetime      = TimeSpan.FromMinutes(5),
            PooledConnectionIdleTimeout   = TimeSpan.FromMinutes(2),
            EnableMultipleHttp2Connections = true,
            ConnectTimeout                = TimeSpan.FromSeconds(15)
        })
        .SetHandlerLifetime(Timeout.InfiniteTimeSpan)   // Polly manages lifecycle
        .AddResilienceHandler($"pipeline-{capturedKey}", pipeline =>
        {
            // ── Circuit Breaker ──────────────────────────────────────────────
            // Open after ≥50 % failures across ≥5 calls in a 30-second window.
            // Stays open (fast-fail) for 30 seconds before half-opening.
            pipeline.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
            {
                FailureRatio        = 0.50,
                SamplingDuration    = TimeSpan.FromSeconds(30),
                MinimumThroughput   = 5,
                BreakDuration       = TimeSpan.FromSeconds(30),
                ShouldHandle        = new PredicateBuilder<HttpResponseMessage>()
                                        .Handle<HttpRequestException>()
                                        .HandleResult(r => (int)r.StatusCode >= 500)
            });

            // ── Retry ────────────────────────────────────────────────────────
            // 3 attempts with exponential back-off (300 ms base) + jitter.
            // Only retries network errors and 5xx responses (not 4xx).
            pipeline.AddRetry(new HttpRetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                Delay            = TimeSpan.FromMilliseconds(300),
                BackoffType      = DelayBackoffType.Exponential,
                UseJitter        = true,
                ShouldHandle     = new PredicateBuilder<HttpResponseMessage>()
                                    .Handle<HttpRequestException>()
                                    .HandleResult(r => (int)r.StatusCode >= 500)
            });

            // ── Per-Attempt Timeout ──────────────────────────────────────────
            // Each individual attempt (including retries) must complete within
            // 30 seconds.  The total request could therefore take up to ~2 min.
            pipeline.AddTimeout(TimeSpan.FromSeconds(30));
        });
}

// Register the custom YARP HttpClient factory so that each cluster's outbound
// calls use its own resilience pipeline (provides per-service bulkhead isolation).
builder.Services.AddSingleton<Yarp.ReverseProxy.Forwarder.IForwarderHttpClientFactory,
                              ResilientForwarderHttpClientFactory>();

// ─────────────────────────────────────────────────────────────────────────────
//  3. JWT Authentication — multi-issuer
//     Tokens are issued by individual services (each with its own signing key).
//     The gateway accepts a token if ANY registered key successfully validates it.
// ─────────────────────────────────────────────────────────────────────────────
var signingKeys = config.GetSection("Jwt:SigningKeys").Get<List<string>>() ?? [];

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = false,   // many issuers — skip issuer check
            ValidateAudience         = false,   // many audiences — skip audience check
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ClockSkew                = TimeSpan.FromSeconds(30),

            // Try every registered key; accept the token if any key validates it.
            IssuerSigningKeyResolver = (_, _, _, _) =>
                signingKeys
                    .Select(k => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(k)))
                    .Cast<SecurityKey>()
                    .ToList()
        };
    });

builder.Services.AddAuthorization();

// ─────────────────────────────────────────────────────────────────────────────
//  4. Rate Limiting
//     Global:      Sliding-window limiter — 100 req/min per client IP.
//                  Applies to every request hitting the gateway.
//     Per-service: Bulkhead (concurrency limiter) — max 20 parallel outbound
//                  calls per downstream service (queue depth 10).
//                  Assigned individually to each YARP route via
//                  "RateLimiterPolicy": "bulkhead-<service>" in appsettings.json.
// ─────────────────────────────────────────────────────────────────────────────
builder.Services.AddRateLimiter(options =>
{
    // ── Global sliding-window (per client IP) ────────────────────────────────
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(ctx =>
        RateLimitPartition.GetSlidingWindowLimiter(
            partitionKey: ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new SlidingWindowRateLimiterOptions
            {
                PermitLimit              = 100,
                Window                   = TimeSpan.FromMinutes(1),
                SegmentsPerWindow        = 4,
                QueueProcessingOrder     = QueueProcessingOrder.OldestFirst,
                QueueLimit               = 25
            }));

    // ── Per-service bulkhead (concurrency limiter) ───────────────────────────
    foreach (var svc in serviceKeys)
    {
        options.AddConcurrencyLimiter($"bulkhead-{svc}", limiter =>
        {
            limiter.PermitLimit          = 20;
            limiter.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            limiter.QueueLimit           = 10;
        });
    }

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.OnRejected = async (ctx, ct) =>
    {
        ctx.HttpContext.Response.StatusCode    = StatusCodes.Status429TooManyRequests;
        ctx.HttpContext.Response.ContentType   = "application/json";
        await ctx.HttpContext.Response.WriteAsync(
            """{"error":"Rate limit exceeded. Please slow down and try again."}""",
            cancellationToken: ct);
    };
});

// ─────────────────────────────────────────────────────────────────────────────
//  5. Output Cache (Response Caching — 30 s for GET responses)
//     Only GET requests with successful (2xx) responses are cached.
//     POST/PUT/DELETE requests bypass the cache automatically.
// ─────────────────────────────────────────────────────────────────────────────
builder.Services.AddOutputCache(options =>
{
    options.AddPolicy("gateway-cache", policy =>
        policy.Expire(TimeSpan.FromSeconds(30)));

    options.AddPolicy("no-cache", policy => policy.NoCache());
});

// ─────────────────────────────────────────────────────────────────────────────
//  6. Health Checks
//     /health            — gateway and all downstream services
//     /health/downstream — downstream services only
// ─────────────────────────────────────────────────────────────────────────────
builder.Services
    .AddHttpClient("healthcheck")
    .ConfigureHttpClient(c => c.Timeout = TimeSpan.FromSeconds(8));

builder.Services.AddHealthChecks()
    .AddCheck<DownstreamHealthCheck>("downstream-services", tags: ["downstream"]);

// ─────────────────────────────────────────────────────────────────────────────
//  7. Logging
// ─────────────────────────────────────────────────────────────────────────────
builder.Services.AddLogging();

// ═════════════════════════════════════════════════════════════════════════════
var app = builder.Build();

// ─────────────────────────────────────────────────────────────────────────────
//  Middleware Pipeline
//  Order is intentional:
//    Correlation ID  → logs always carry the trace ID
//    Request Logging → wraps the whole pipeline (start→finish)
//    Rate Limiter    → reject over-limit requests early
//    Output Cache    → serve cached responses before auth overhead
//    Authentication  → validate JWT
//    Authorization   → enforce policies
//    Proxy           → forward to downstream service
// ─────────────────────────────────────────────────────────────────────────────

// 1. Inject / propagate X-Correlation-ID
app.UseMiddleware<CorrelationIdMiddleware>();

// 2. Log every request / response with timing
app.UseMiddleware<RequestResponseLoggingMiddleware>();

// 3. Global sliding-window + per-service bulkhead
app.UseRateLimiter();

// 4. 30-second GET response cache
app.UseOutputCache();

// 5. Validate Bearer tokens
app.UseAuthentication();
app.UseAuthorization();

// ─────────────────────────────────────────────────────────────────────────────
//  Health Endpoints  (no authentication required)
// ─────────────────────────────────────────────────────────────────────────────
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = WriteJsonHealthResponse
}).AllowAnonymous();

app.MapHealthChecks("/health/downstream", new HealthCheckOptions
{
    Predicate      = hc => hc.Tags.Contains("downstream"),
    ResponseWriter = WriteJsonHealthResponse
}).AllowAnonymous();

// ─────────────────────────────────────────────────────────────────────────────
//  YARP Reverse Proxy
// ─────────────────────────────────────────────────────────────────────────────
app.MapReverseProxy();

app.Run();

// ─────────────────────────────────────────────────────────────────────────────
//  Helper — structured JSON health-check response writer
// ─────────────────────────────────────────────────────────────────────────────
static Task WriteJsonHealthResponse(HttpContext context, HealthReport report)
{
    context.Response.ContentType = "application/json";

    var result = JsonSerializer.Serialize(new
    {
        status        = report.Status.ToString(),
        totalDuration = report.TotalDuration.TotalMilliseconds + "ms",
        uptime        = DateTime.UtcNow,
        checks        = report.Entries.Select(e => new
        {
            name        = e.Key,
            status      = e.Value.Status.ToString(),
            description = e.Value.Description,
            duration    = e.Value.Duration.TotalMilliseconds + "ms",
            data        = e.Value.Data.Count > 0 ? e.Value.Data : null,
            exception   = e.Value.Exception?.Message
        })
    }, new JsonSerializerOptions { WriteIndented = true });

    return context.Response.WriteAsync(result);
}
