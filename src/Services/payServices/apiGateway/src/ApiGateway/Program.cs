using ApiGateway.Extensions;
using ApiGateway.Middleware;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;
using Ocelot.Provider.Polly;
using Serilog;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// Serilog Configuration
// ============================================================
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "ApiGateway")
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File("Logs/gateway-.log",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();

// ============================================================
// Ocelot Configuration
// ============================================================
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration)
    .AddCacheManager(x => x.WithDictionaryHandle())
    .AddPolly();

// ============================================================
// YARP Reverse Proxy
// ============================================================
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// ============================================================
// JWT Authentication & Authorization
// ============================================================
builder.Services.AddGatewayAuthentication(builder.Configuration);

// ============================================================
// Rate Limiting & Throttling
// ============================================================
builder.Services.AddGatewayRateLimiting(builder.Configuration);

// ============================================================
// Circuit Breaker, Retry, Timeout, Bulkhead (Polly)
// ============================================================
builder.Services.AddResiliencePolicies(builder.Configuration);

// ============================================================
// Response Caching
// ============================================================
builder.Services.AddMemoryCache();
builder.Services.AddResponseCaching();

// ============================================================
// Health Checks
// ============================================================
builder.Services.AddGatewayHealthChecks();

// ============================================================
// CORS
// ============================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("GatewayCors", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
              .WithExposedHeaders("X-Correlation-ID", "X-Cache", "Retry-After");
    });
});

// ============================================================
// Swagger
// ============================================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "ERP Microservice API Gateway", Version = "v1" });
});

var app = builder.Build();

// ============================================================
// Middleware Pipeline (ORDER MATTERS)
// ============================================================

// 1. Exception handling (outermost)
app.UseMiddleware<ExceptionHandlingMiddleware>();

// 2. Correlation ID tracking
app.UseMiddleware<CorrelationIdMiddleware>();

// 3. Request/Response logging
app.UseMiddleware<RequestResponseLoggingMiddleware>();

// 4. CORS
app.UseCors("GatewayCors");

// 5. Response caching
app.UseMiddleware<ResponseCachingMiddleware>();

// 6. Rate Limiting
app.UseRateLimiter();

// 7. Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// ============================================================
// Swagger UI
// ============================================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gateway v1");
        c.RoutePrefix = "swagger";
    });
}

// ============================================================
// Health Check Endpoints
// ============================================================
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => false, // Gateway only (no downstream checks)
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

app.MapHealthChecks("/health/services", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("services"),
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = new
        {
            status = report.Status.ToString(),
            totalDuration = report.TotalDuration.TotalMilliseconds,
            services = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                duration = e.Value.Duration.TotalMilliseconds,
                description = e.Value.Description,
                error = e.Value.Exception?.Message
            })
        };
        await context.Response.WriteAsync(JsonSerializer.Serialize(result, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        }));
    }
});

// ============================================================
// Gateway Info Endpoint
// ============================================================
app.MapGet("/gateway/info", () => Results.Ok(new
{
    gateway = "ERP Microservice API Gateway",
    version = "1.0.0",
    timestamp = DateTime.UtcNow,
    features = new[]
    {
        "YARP Reverse Proxy",
        "Ocelot API Gateway",
        "JWT Authentication",
        "Rate Limiting (Fixed/Sliding/Token Bucket)",
        "Circuit Breaker (Polly)",
        "Retry with Exponential Backoff",
        "Timeout Handling",
        "Bulkhead Isolation",
        "Correlation ID Tracking",
        "Request/Response Logging",
        "Response Caching",
        "Health Checks",
        "Load Balancing (Round Robin)"
    },
    services = new[]
    {
        new { name = "Employee Service", port = 5104, ocelotPrefix = "/api/employees", yarpPrefix = "/yarp/employees" },
        new { name = "HR Service", port = 5000, ocelotPrefix = "/api/hr", yarpPrefix = "/yarp/hr" },
        new { name = "FAQ Service", port = 5032, ocelotPrefix = "/api/faq", yarpPrefix = "/yarp/faq" },
        new { name = "Payroll Service", port = 5002, ocelotPrefix = "/api/payroll", yarpPrefix = "/yarp/payroll" },
        new { name = "Tax Service", port = 5010, ocelotPrefix = "/api/tax", yarpPrefix = "/yarp/tax" },
        new { name = "Pay Transactional Service", port = 5020, ocelotPrefix = "/api/paytransactions", yarpPrefix = "/yarp/paytransactional" }
    },
    routing = new
    {
        ocelot = "Routes via /api/* and /graphql/* paths — configured in ocelot.json",
        yarp = "Routes via /yarp/* paths — configured in appsettings.json ReverseProxy section"
    }
})).WithTags("Gateway");

// ============================================================
// Auth Token Generator (Gateway-issued tokens)
// ============================================================
app.MapGet("/gateway/token", (IConfiguration config) =>
{
    var token = AuthenticationExtensions.GenerateGatewayToken(config);
    return Results.Ok(new
    {
        token,
        tokenType = "Bearer",
        expiresIn = 3600,
        issuer = "ERPMicroserviceGateway",
        note = "This token is accepted by all downstream services configured with matching secret key."
    });
}).WithTags("Authentication");

app.MapPost("/gateway/token", (TokenRequest request, IConfiguration config) =>
{
    if (string.IsNullOrWhiteSpace(request.UserId))
        return Results.BadRequest(new { error = "userId is required" });

    var token = AuthenticationExtensions.GenerateGatewayToken(config, request.UserId, request.Role ?? "User");
    return Results.Ok(new
    {
        token,
        tokenType = "Bearer",
        expiresIn = 3600,
        userId = request.UserId,
        role = request.Role ?? "User"
    });
}).WithTags("Authentication");

// ============================================================
// Service Registry / Route Map
// ============================================================
app.MapGet("/gateway/routes", () =>
{
    var routes = new
    {
        ocelotRoutes = new
        {
            employee = new
            {
                api = "/api/employees/{everything}",
                auth = "/api/employee-auth/{everything}",
                graphql = "/graphql/employees",
                health = "/health/employee"
            },
            hr = new
            {
                api = "/api/hr/{everything}",
                health = "/health/hr"
            },
            faq = new
            {
                api = "/api/faq/{everything}",
                graphql = "/graphql/faq",
                health = "/health/faq"
            },
            payroll = new
            {
                transactions = "/api/payroll/transactions/{everything}",
                batches = "/api/payroll/batches/{everything}",
                adjustments = "/api/payroll/adjustments/{everything}",
                graphql = "/graphql/payroll",
                health = "/health/payroll"
            },
            tax = new
            {
                api = "/api/tax/{everything}",
                graphql = "/graphql/tax",
                health = "/health/tax"
            },
            payTransactional = new
            {
                transactions = "/api/paytransactions/{everything}",
                arrears = "/api/payarrears/{everything}",
                adjustments = "/api/payadjustments/{everything}",
                batches = "/api/paytransactional/batches/{everything}",
                auth = "/api/paytransactional-auth/{everything}",
                graphql = "/graphql/paytransactional",
                health = "/health/paytransactional",
                summary = "/api/paytransactional/summary/month/{monthYear}"
            }
        },
        yarpRoutes = new
        {
            employee = "/yarp/employees/{catch-all}",
            hr = "/yarp/hr/{catch-all}",
            faq = "/yarp/faq/{catch-all}",
            payroll = "/yarp/payroll/{catch-all}",
            tax = "/yarp/tax/{catch-all}",
            payTransactional = "/yarp/paytransactional/{catch-all}",
            graphql = new
            {
                employee = "/yarp/graphql/employees/{catch-all}",
                faq = "/yarp/graphql/faq/{catch-all}",
                payroll = "/yarp/graphql/payroll/{catch-all}",
                tax = "/yarp/graphql/tax/{catch-all}",
                payTransactional = "/yarp/graphql/paytransactional/{catch-all}"
            }
        }
    };
    return Results.Ok(routes);
}).WithTags("Gateway");

// ============================================================
// YARP Reverse Proxy — mapped BEFORE Ocelot
// ============================================================
app.MapReverseProxy();

// ============================================================
// Ocelot Pipeline (must be the LAST middleware — it's terminal)
// ============================================================
await app.UseOcelot();

await app.RunAsync();

// ============================================================
// Supporting Types
// ============================================================
public record TokenRequest(string UserId, string? Role);
