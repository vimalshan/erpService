using SSC.ApiGateway.Extensions;
using SSC.ApiGateway.Middleware;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ── Serilog ────────────────────────────────────────────────────────────────
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// ── Load Ocelot configuration ──────────────────────────────────────────────
builder.Configuration.AddJsonFile("ocelot.json", optional: true, reloadOnChange: true);

// ── JWT Authentication ─────────────────────────────────────────────────────
builder.Services.AddJwtAuthentication(builder.Configuration);

// ── Rate Limiting & Throttling ─────────────────────────────────────────────
builder.Services.AddGatewayRateLimiting(builder.Configuration);

// ── YARP Reverse Proxy ─────────────────────────────────────────────────────
builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// ── Response Caching ───────────────────────────────────────────────────────
builder.Services.AddResponseCaching();
builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(b => b.Expire(TimeSpan.FromSeconds(30)));
    options.AddPolicy("CacheMasterData", b => b.Expire(TimeSpan.FromMinutes(5)).Tag("master-data"));
    options.AddPolicy("CacheShort", b => b.Expire(TimeSpan.FromSeconds(15)));
    options.AddPolicy("NoCache", b => b.NoCache());
});

// ── Resilience (Circuit Breaker, Retry, Timeout, Bulkhead) ─────────────────
builder.Services.AddGatewayResilience(builder.Configuration);

// ── Health Checks ──────────────────────────────────────────────────────────
builder.Services.AddGatewayHealthChecks(builder.Configuration);

// ── CORS ───────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// ── Swagger / OpenAPI ──────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// ── Middleware Pipeline (order matters) ─────────────────────────────────────

// 1. Correlation ID (first, so all logs include it)
app.UseMiddleware<CorrelationIdMiddleware>();

// 2. Request/Response Logging
app.UseMiddleware<RequestResponseLoggingMiddleware>();

// 3. Global Exception Handler
app.UseMiddleware<GlobalExceptionMiddleware>();

// 4. CORS
app.UseCors("AllowAll");

// 5. Response Caching
app.UseResponseCaching();
app.UseOutputCache();

// 6. Rate Limiting
app.UseRateLimiter();

// 7. Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// 8. Health Check Endpoints
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("live"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// 9. Gateway Info Endpoint
app.MapGet("/gateway/info", () => Results.Ok(new
{
    Service = "SSC API Gateway",
    Version = "1.0.0",
    Timestamp = DateTime.UtcNow,
    Environment = app.Environment.EnvironmentName
}));

// 10. YARP Reverse Proxy
app.MapReverseProxy();

app.Run();
