using AspNetCoreRateLimit;
using HealthGateway.HealthChecks;
using HealthGateway.Middleware;
using HealthGateway.Resilience;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Http;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using System.Text;
using System.Threading.RateLimiting;

// ─── Serilog Bootstrap ──────────────────────────────────────────────────────
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Yarp", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithEnvironmentName()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File("logs/gateway-.log",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

var configuration = builder.Configuration;

// ─── Load yarp.json + ocelot.json ─────────────────────────────────────────
configuration
    .AddJsonFile("yarp.json", optional: false, reloadOnChange: true)
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// ─── JWT Authentication ────────────────────────────────────────────────────
var jwtSection = configuration.GetSection("Jwt");
var secretKey = jwtSection["SecretKey"]
    ?? throw new InvalidOperationException("Jwt:SecretKey is not configured.");
var keyBytes = Encoding.UTF8.GetBytes(secretKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

builder.Services.AddAuthorization();

// ─── IP Rate Limiting (AspNetCoreRateLimit) ────────────────────────────────
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// ─── ASP.NET Core Fixed/Sliding-Window Rate Limiter (YARP policies) ────────
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy("standard", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 10
            }));
    options.AddPolicy("strict", context =>
        RateLimitPartition.GetSlidingWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new SlidingWindowRateLimiterOptions
            {
                PermitLimit = 20,
                Window = TimeSpan.FromMinutes(1),
                SegmentsPerWindow = 4,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 5
            }));
});

// ─── Response Caching ──────────────────────────────────────────────────────
builder.Services.AddResponseCaching(options =>
{
    options.MaximumBodySize = 4 * 1024 * 1024; // 4 MB
    options.SizeLimit = 100 * 1024 * 1024;     // 100 MB
});

// ─── Bulkhead Isolation ────────────────────────────────────────────────────
builder.Services.AddSingleton<BulkheadPolicy>();

// ─── HTTP Clients with Resilience (Retry + Circuit Breaker + Timeout) ──────
var downstreamClients = new[]
{
    ("AccidentClient",    "AccidentManagement"),
    ("CheckupClient",     "CheckupManagement"),
    ("InsuranceClient",   "InsuranceManagement"),
    ("MastersClient",     "Masters"),
    ("MedicalVisitClient","MedicalVisit"),
    ("MedicineClient",    "MedicineManagement"),
    ("TransactionClient", "HealthTransaction")
};

foreach (var (clientName, configKey) in downstreamClients)
{
    builder.Services.AddHttpClient(clientName, client =>
    {
        var baseUrl = configuration[$"Services:{configKey}"];
        if (!string.IsNullOrEmpty(baseUrl))
            client.BaseAddress = new Uri(baseUrl);
        client.Timeout = TimeSpan.FromSeconds(35);
    })
    .AddPolicyHandler(ResiliencePolicies.GetRetryPolicy())
    .AddPolicyHandler(ResiliencePolicies.GetCircuitBreakerPolicy());
}

// Health-check HTTP client (no retry — just ping)
builder.Services.AddHttpClient("HealthCheck", client =>
{
    client.Timeout = TimeSpan.FromSeconds(10);
});

// ─── Health Checks ─────────────────────────────────────────────────────────
builder.Services.AddHealthChecks()
    .Add(new HealthCheckRegistration("accident-service",
        sp => new ServiceHealthCheck(sp.GetRequiredService<IHttpClientFactory>(), configuration["Services:AccidentManagement"]!, "AccidentManagement"),
        HealthStatus.Degraded, ["downstream"]))
    .Add(new HealthCheckRegistration("checkup-service",
        sp => new ServiceHealthCheck(sp.GetRequiredService<IHttpClientFactory>(), configuration["Services:CheckupManagement"]!, "CheckupManagement"),
        HealthStatus.Degraded, ["downstream"]))
    .Add(new HealthCheckRegistration("insurance-service",
        sp => new ServiceHealthCheck(sp.GetRequiredService<IHttpClientFactory>(), configuration["Services:InsuranceManagement"]!, "InsuranceManagement"),
        HealthStatus.Degraded, ["downstream"]))
    .Add(new HealthCheckRegistration("masters-service",
        sp => new ServiceHealthCheck(sp.GetRequiredService<IHttpClientFactory>(), configuration["Services:Masters"]!, "Masters"),
        HealthStatus.Degraded, ["downstream"]))
    .Add(new HealthCheckRegistration("medicalvisit-service",
        sp => new ServiceHealthCheck(sp.GetRequiredService<IHttpClientFactory>(), configuration["Services:MedicalVisit"]!, "MedicalVisit"),
        HealthStatus.Degraded, ["downstream"]))
    .Add(new HealthCheckRegistration("medicine-service",
        sp => new ServiceHealthCheck(sp.GetRequiredService<IHttpClientFactory>(), configuration["Services:MedicineManagement"]!, "MedicineManagement"),
        HealthStatus.Degraded, ["downstream"]))
    .Add(new HealthCheckRegistration("transaction-service",
        sp => new ServiceHealthCheck(sp.GetRequiredService<IHttpClientFactory>(), configuration["Services:HealthTransaction"]!, "HealthTransaction"),
        HealthStatus.Degraded, ["downstream"]));

// ─── YARP Reverse Proxy ────────────────────────────────────────────────────
builder.Services.AddReverseProxy()
    .LoadFromConfig(configuration.GetSection("ReverseProxy"));

// ─── CORS ──────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// ═══════════════════════════════════════════════════════════════════════════
var app = builder.Build();
// ═══════════════════════════════════════════════════════════════════════════

// ─── Middleware Pipeline ───────────────────────────────────────────────────

// 1. Correlation ID (first — populates context for all subsequent logging)
app.UseMiddleware<CorrelationIdMiddleware>();

// 2. Request/Response Logging
app.UseMiddleware<RequestResponseLoggingMiddleware>();

// 3. IP Rate Limiting
app.UseIpRateLimiting();

// 4. ASP.NET Core Rate Limiter (policy-based)
app.UseRateLimiter();

// 5. CORS
app.UseCors();

// 6. Response Caching
app.UseResponseCaching();

// 7. Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// ─── Health Endpoints ─────────────────────────────────────────────────────
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = System.Text.Json.JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            gateway = "HealthGateway",
            timestamp = DateTime.UtcNow,
            services = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description,
                duration = e.Value.Duration.TotalMilliseconds + "ms"
            })
        });
        await context.Response.WriteAsync(result);
    }
});

app.MapHealthChecks("/health/downstream", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("downstream"),
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = System.Text.Json.JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            services = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description
            })
        });
        await context.Response.WriteAsync(result);
    }
});

// ─── Gateway Info Endpoint ────────────────────────────────────────────────
app.MapGet("/", () => new
{
    gateway = "Health ERP API Gateway",
    version = "1.0.0",
    timestamp = DateTime.UtcNow,
    routes = new[]
    {
        "/api/accident/**     → AccidentManagement  :5000",
        "/api/checkup/**      → CheckupManagement   :7101",
        "/api/insurance/**    → InsuranceManagement :5100",
        "/api/masters/**      → Masters             :5200",
        "/api/medicalvisit/** → MedicalVisit        :5300",
        "/api/medicine/**     → MedicineManagement  :5400",
        "/api/transaction/**  → HealthTransaction   :5500"
    }
});

// ─── YARP Reverse Proxy (last — handles all proxied traffic) ─────────────
app.MapReverseProxy();

Log.Information("Health ERP API Gateway started on http://localhost:5600");

app.Run();
