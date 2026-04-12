using System.Text.Json;
using ApiGateway.Extensions;
using ApiGateway.Middleware;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using Serilog;

// ===================== SERILOG BOOTSTRAP =====================
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/gateway-bootstrap-.log", rollingInterval: RollingInterval.Day)
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // ===================== SERILOG =====================
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    // ===================== CONFIGURATION =====================
    builder.Configuration
        .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

    // ===================== SERVICES =====================

    // JWT Authentication & Authorization
    builder.Services.AddGatewayAuthentication(builder.Configuration);

    // Rate Limiting & Throttling
    builder.Services.AddGatewayRateLimiting();

    // Circuit Breaker, Retry & Timeout (Polly)
    builder.Services.AddGatewayResilience();

    // Health Checks for all downstream services
    builder.Services.AddGatewayHealthChecks();

    // Response Caching
    builder.Services.AddGatewayResponseCaching();

    // YARP Reverse Proxy
    builder.Services.AddReverseProxy()
        .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

    // Ocelot
    builder.Services.AddOcelot(builder.Configuration)
        .AddPolly();

    // CORS
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .WithExposedHeaders("X-Correlation-ID", "X-RateLimit-Limit", "X-RateLimit-Remaining", "Retry-After");
        });
    });

    var app = builder.Build();

    // ===================== MIDDLEWARE PIPELINE =====================

    // 1. Global exception handler (outermost)
    app.UseMiddleware<GlobalExceptionMiddleware>();

    // 2. Correlation ID tracking
    app.UseMiddleware<CorrelationIdMiddleware>();

    // 3. Serilog request logging
    app.UseSerilogRequestLogging(options =>
    {
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("CorrelationId", httpContext.Items["CorrelationId"]?.ToString() ?? "N/A");
            diagnosticContext.Set("ClientIp", httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown");
        };
    });

    // 4. Request/Response logging
    app.UseMiddleware<RequestResponseLoggingMiddleware>();

    // 5. CORS
    app.UseCors();

    // 6. Response caching
    app.UseResponseCaching();
    app.UseOutputCache();

    // 7. Rate limiting
    app.UseRateLimiter();

    // 8. Authentication & Authorization
    app.UseAuthentication();
    app.UseAuthorization();

    // ===================== HEALTH CHECK ENDPOINTS =====================
    // These must be handled BEFORE Ocelot takes over the pipeline
    app.UseHealthChecks("/health", new HealthCheckOptions
    {
        Predicate = _ => false,
        ResponseWriter = async (context, report) =>
        {
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                status = report.Status.ToString(),
                gateway = "ApiGateway",
                timestamp = DateTime.UtcNow
            }));
        }
    });

    app.UseHealthChecks("/health/services", new HealthCheckOptions
    {
        Predicate = check => check.Tags.Contains("service"),
        ResponseWriter = async (context, report) =>
        {
            context.Response.ContentType = "application/json";
            var entries = report.Entries.Select(e => new
            {
                service = e.Key,
                status = e.Value.Status.ToString(),
                duration = e.Value.Duration.TotalMilliseconds + "ms",
                description = e.Value.Description
            });

            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                status = report.Status.ToString(),
                totalDuration = report.TotalDuration.TotalMilliseconds + "ms",
                services = entries
            }));
        }
    });

    // ===================== GATEWAY INFO ENDPOINT =====================
    app.Use(async (context, next) =>
    {
        if (context.Request.Path.StartsWithSegments("/gateway/info"))
        {
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                name = "ERP Travel Services API Gateway",
                version = "1.0.0",
                timestamp = DateTime.UtcNow,
                features = new[]
                {
                    "Ocelot Routing",
                    "YARP Reverse Proxy",
                    "JWT Authentication",
                    "Rate Limiting & Throttling",
                    "Correlation ID Tracking",
                    "Circuit Breaker (Polly)",
                    "Retry with Exponential Backoff",
                    "Timeout Handling",
                    "Bulkhead Isolation",
                    "Health Checks & Monitoring",
                    "Response Caching",
                    "Request/Response Logging",
                    "Load Balancing (Round Robin)"
                },
                services = new[]
                {
                    new { name = "TravelRequestService", port = 5205, prefix = "/api/travelrequests" },
                    new { name = "TravelTransactionService", port = 5082, prefix = "/api/vendors, /api/taxmasters" },
                    new { name = "BookingService", port = 5117, prefix = "/api/bookings" },
                    new { name = "ExpenseService", port = 5090, prefix = "/api/expenses" },
                    new { name = "FinanceService", port = 5294, prefix = "/api/invoices, /api/batches, /api/payments" },
                    new { name = "InsuranceService", port = 5179, prefix = "/api/insurance" },
                    new { name = "MasterDataService", port = 5166, prefix = "/api/areas, /api/guesthouses, /api/lookups, /api/routes" },
                    new { name = "AgencyService", port = 5000, prefix = "/api/agency, /api/airline" },
                    new { name = "AdminService", port = 5001, prefix = "/api/adminunits, /api/financeunits" }
                }
            }));
            return;
        }
        await next();
    });

    // ===================== YARP REVERSE PROXY (under /yarp/ prefix) =====================
    app.MapReverseProxy();

    // ===================== OCELOT (primary gateway routing) =====================
    await app.UseOcelot();

    Log.Information("API Gateway started on http://localhost:5100");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "API Gateway terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

