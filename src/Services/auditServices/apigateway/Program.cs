using ApiGateway.Extensions;
using ApiGateway.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ─── Serilog ───────────────────────────────────────────────────────────────
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// ─── CORS ──────────────────────────────────────────────────────────────────
var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? ["http://localhost:3000"];

builder.Services.AddCors(options =>
    options.AddPolicy("GatewayPolicy", policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials()));

// ─── JWT Authentication & Authorization ────────────────────────────────────
builder.Services.AddGatewayAuthentication(builder.Configuration);

// ─── Rate Limiting & Throttling ────────────────────────────────────────────
builder.Services.AddGatewayRateLimiting(builder.Configuration);

// ─── Response Caching & Output Cache ───────────────────────────────────────
builder.Services.AddGatewayResponseCaching(builder.Configuration);

// ─── Health Checks & Monitoring ────────────────────────────────────────────
builder.Services.AddGatewayHealthChecks(builder.Configuration);

// ─── YARP + Polly Resilience (Circuit Breaker, Retry, Timeout, Bulkhead) ───
builder.Services.AddGatewayReverseProxy(builder.Configuration);

// ─── Swagger / OpenAPI (optional, gateway-level) ───────────────────────────
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// ─── Middleware Pipeline ────────────────────────────────────────────────────

// 1. Correlation ID (must be first — enriches all subsequent logs)
app.UseCorrelationId();

// 2. Request / Response structured logging
app.UseRequestResponseLogging();

// 3. CORS
app.UseCors("GatewayPolicy");

// 4. Response Caching
app.UseResponseCaching();
app.UseOutputCache();

// 5. Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// 6. Rate Limiting & Throttling
app.UseRateLimiter();

// 7. Health Checks & UI
app.MapGatewayHealthChecks();

// ─── Gateway information endpoint (unauthenticated) ─────────────────────────
app.MapGet("/", () => Results.Json(new
{
    service    = "ERP API Gateway",
    version    = "1.0.0",
    timestamp  = DateTimeOffset.UtcNow,
    health     = "/health",
    healthUi   = "/health-ui"
})).AllowAnonymous();

// ─── YARP Reverse Proxy ─────────────────────────────────────────────────────
// Routes are defined in appsettings.json under "ReverseProxy"
// Equivalent Ocelot configuration is in ocelot.json
app.MapReverseProxy(proxyPipeline =>
{
    // Attach correlation ID context into each proxied request
    proxyPipeline.Use(async (ctx, next) =>
    {
        if (ctx.Items["CorrelationId"] is string correlationId)
            ctx.Request.Headers["X-Correlation-Id"] = correlationId;
        await next();
    });

    proxyPipeline.UseSessionAffinity();
    proxyPipeline.UseLoadBalancing();
    proxyPipeline.UsePassiveHealthChecks();
});

try
{
    Log.Information("ERP API Gateway starting on {Urls}",
        string.Join(", ", builder.Configuration
            .GetSection("Kestrel:Endpoints")
            .GetChildren()
            .Select(e => e["Url"])));
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "ERP API Gateway failed to start");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
