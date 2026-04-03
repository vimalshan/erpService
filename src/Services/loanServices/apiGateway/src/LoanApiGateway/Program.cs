using LoanApiGateway.Authentication;
using LoanApiGateway.Caching;
using LoanApiGateway.HealthChecks;
using LoanApiGateway.Middleware;
using LoanApiGateway.RateLimiting;
using LoanApiGateway.Resilience;
using Serilog;
using Serilog.Events;
using Yarp.ReverseProxy.Forwarder;

// ══════════════════════════════════════════════════════════════════════════════
//  Bootstrap Serilog (before the host is built so startup errors are captured)
// ══════════════════════════════════════════════════════════════════════════════
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithEnvironmentName()
    .Enrich.WithThreadId()
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File(
        "logs/gateway-.log",
        rollingInterval: RollingInterval.Day,
        outputTemplate:
        "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Level:u3}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

try
{
    Log.Information("=== Loan API Gateway starting up ===");

    var builder = WebApplication.CreateBuilder(args);

    // ── Serilog ──────────────────────────────────────────────────────────────
    builder.Host.UseSerilog((context, services, configuration) =>
    {
        var logLevel = context.HostingEnvironment.IsDevelopment()
            ? LogEventLevel.Debug
            : LogEventLevel.Information;

        configuration
            .MinimumLevel.Is(logLevel)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithEnvironmentName()
            .Enrich.WithThreadId()
            .WriteTo.Console(outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level:u3}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.File(
                "logs/gateway-.log",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 30);
    });

    // ── YARP Reverse Proxy (reads ReverseProxy section from appsettings) ──────
    builder.Services
        .AddReverseProxy()
        .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

    // Register custom resilient YARP HTTP client factory
    builder.Services.AddSingleton<IForwarderHttpClientFactory, ResilientForwarderHttpClientFactory>();

    // ── Authentication & Authorization ────────────────────────────────────────
    builder.Services.AddGatewayAuthentication(builder.Configuration);

    // ── Rate Limiting ─────────────────────────────────────────────────────────
    builder.Services.AddGatewayRateLimiting(builder.Configuration);

    // ── Resilience (Circuit Breaker, Retry, Timeout) ──────────────────────────
    builder.Services.AddGatewayResilience(builder.Configuration);

    // ── Response Caching & Output Cache ───────────────────────────────────────
    builder.Services.AddGatewayCaching(builder.Configuration);

    // ── Health Checks ─────────────────────────────────────────────────────────
    builder.Services.AddGatewayHealthChecks(builder.Configuration);

    // ── Cors (open for development; lock down in production) ──────────────────
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("GatewayCors", policy =>
        {
            if (builder.Environment.IsDevelopment())
            {
                policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }
            else
            {
                var allowed = builder.Configuration
                    .GetSection("Gateway:Cors:AllowedOrigins")
                    .Get<string[]>() ?? [];
                policy.WithOrigins(allowed)
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();
            }
        });
    });

    // ── Swagger / API Documentation ───────────────────────────────────────────
    builder.Services.AddEndpointsApiExplorer();

    var app = builder.Build();

    // ══════════════════════════════════════════════════════════════════════════
    //  Middleware Pipeline  (order matters!)
    // ══════════════════════════════════════════════════════════════════════════

    // 1. Global exception handler — wraps everything below
    app.UseGlobalExceptionHandler();

    // 2. Correlation ID — must be first so all downstream middleware see it
    app.UseCorrelationId();

    // 3. Request/Response logging — logs every request with correlation ID
    app.UseRequestResponseLogging();

    // 4. HTTPS Redirection
    if (!app.Environment.IsDevelopment())
        app.UseHttpsRedirection();

    // 5. Serilog request logging (structured, includes duration)
    app.UseSerilogRequestLogging(opts =>
    {
        opts.MessageTemplate =
            "HTTP {RequestMethod} {RequestPath} → {StatusCode} in {Elapsed:0.000}ms";
        opts.EnrichDiagnosticContext = (diag, httpContext) =>
        {
            diag.Set("RemoteIp", httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown");
            diag.Set("UserAgent", httpContext.Request.Headers["User-Agent"].FirstOrDefault() ?? "unknown");
            diag.Set("CorrelationId",
                httpContext.Items.TryGetValue(CorrelationIdMiddleware.HeaderName, out var c)
                    ? c?.ToString() ?? "N/A" : "N/A");
        };
    });

    // 6. CORS
    app.UseCors("GatewayCors");

    // 7. Response Caching
    app.UseResponseCaching();

    // 8. Output Cache
    app.UseOutputCache();

    // 9. Authentication & Authorization
    app.UseAuthentication();
    app.UseAuthorization();

    // 10. Rate Limiting
    app.UseRateLimiter();

    // 11. Bulkhead Isolation
    app.UseBulkheadIsolation();

    // ── Health Check Endpoints ────────────────────────────────────────────────
    app.MapGatewayHealthChecks();

    // ── Gateway Info Endpoint ─────────────────────────────────────────────────
    app.MapGet("/", (IConfiguration config) => new
    {
        gateway = "Loan API Gateway",
        version = "1.0.0",
        environment = app.Environment.EnvironmentName,
        timestamp = DateTime.UtcNow,
        services = new[]
        {
            new { name = "LoanTransaction",  prefix = "/api/transactions",  port = 5292 },
            new { name = "LoanApplication",  prefix = "/api/applications",  port = 5282 },
            new { name = "LoanAccount",      prefix = "/api/accounts",      port = 5150 },
            new { name = "LoanDefinition",   prefix = "/api/definitions",   port = 5077 },
            new { name = "DocumentService",  prefix = "/api/documents",     port = 5280 },
            new { name = "LovService",       prefix = "/api/lovs",          port = 5008 },
            new { name = "UtilityService",   prefix = "/api/utility",       port = 5143 }
        },
        links = new
        {
            health     = "/health",
            healthLive = "/health/live",
            healthReady = "/health/ready",
            healthUi   = "/health/ui"
        }
    }).WithName("GatewayInfo").AllowAnonymous();

    // ── YARP Reverse Proxy ────────────────────────────────────────────────────
    app.MapReverseProxy();

    Log.Information("=== Loan API Gateway listening on {Urls} ===",
        string.Join(", ", app.Urls));

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Loan API Gateway terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

return 0;
