using System.Threading.RateLimiting;
using ApiGateway.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- Serilog ---
builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

// --- Configuration ---
builder.Configuration
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// --- JWT Authentication ---
var jwtConfig = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtConfig["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfig["Issuer"],
        ValidAudience = jwtConfig["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.FromMinutes(2)
    };
});
builder.Services.AddAuthorization();

// --- Rate Limiting (Built-in .NET) ---
var rateLimitConfig = builder.Configuration.GetSection("RateLimiting");
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = rateLimitConfig.GetValue("Fixed:PermitLimit", 100);
        opt.Window = TimeSpan.FromSeconds(rateLimitConfig.GetValue("Fixed:Window", 60));
        opt.QueueLimit = rateLimitConfig.GetValue("Fixed:QueueLimit", 10);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    options.AddSlidingWindowLimiter("sliding", opt =>
    {
        opt.PermitLimit = rateLimitConfig.GetValue("Sliding:PermitLimit", 200);
        opt.Window = TimeSpan.FromSeconds(rateLimitConfig.GetValue("Sliding:Window", 60));
        opt.SegmentsPerWindow = rateLimitConfig.GetValue("Sliding:SegmentsPerWindow", 6);
        opt.QueueLimit = rateLimitConfig.GetValue("Sliding:QueueLimit", 10);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    options.AddTokenBucketLimiter("token", opt =>
    {
        opt.TokenLimit = rateLimitConfig.GetValue("Token:TokenLimit", 500);
        opt.ReplenishmentPeriod = TimeSpan.FromSeconds(rateLimitConfig.GetValue("Token:ReplenishmentPeriod", 10));
        opt.TokensPerPeriod = rateLimitConfig.GetValue("Token:TokensPerPeriod", 50);
        opt.QueueLimit = rateLimitConfig.GetValue("Token:QueueLimit", 10);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.ContentType = "application/json";
        await context.HttpContext.Response.WriteAsJsonAsync(new
        {
            error = "Too many requests",
            message = "Rate limit exceeded. Please try again later.",
            retryAfter = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter)
                ? retryAfter.TotalSeconds
                : 60
        }, cancellationToken);
    };
});

// --- Response Caching ---
builder.Services.AddResponseCaching();

// --- Resilience Policies (Polly) via HttpClient ---
var resilienceConfig = builder.Configuration.GetSection("Resilience");
builder.Services.AddHttpClient("GatewayClient")
    .AddResilienceHandler("gateway-pipeline", pipelineBuilder =>
    {
        // Retry
        pipelineBuilder.AddRetry(new RetryStrategyOptions<HttpResponseMessage>
        {
            MaxRetryAttempts = resilienceConfig.GetValue("Retry:MaxRetries", 3),
            Delay = TimeSpan.FromSeconds(resilienceConfig.GetValue("Retry:BaseDelay", 1)),
            BackoffType = DelayBackoffType.Exponential,
            ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                .HandleResult(r => (int)r.StatusCode >= 500)
                .Handle<HttpRequestException>()
                .Handle<TimeoutRejectedException>()
        });

        // Circuit Breaker
        pipelineBuilder.AddCircuitBreaker(new CircuitBreakerStrategyOptions<HttpResponseMessage>
        {
            FailureRatio = resilienceConfig.GetValue("CircuitBreaker:FailureRatio", 0.5),
            SamplingDuration = TimeSpan.FromSeconds(resilienceConfig.GetValue("CircuitBreaker:SamplingDuration", 30)),
            MinimumThroughput = resilienceConfig.GetValue("CircuitBreaker:MinimumThroughput", 10),
            BreakDuration = TimeSpan.FromSeconds(resilienceConfig.GetValue("CircuitBreaker:BreakDuration", 30)),
            ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                .HandleResult(r => (int)r.StatusCode >= 500)
                .Handle<HttpRequestException>()
                .Handle<TimeoutRejectedException>()
        });

        // Timeout
        pipelineBuilder.AddTimeout(TimeSpan.FromSeconds(
            resilienceConfig.GetValue("Timeout:TimeoutSeconds", 30)));
    });

// --- YARP Reverse Proxy ---
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// --- Ocelot ---
builder.Services.AddOcelot(builder.Configuration)
    .AddPolly();

// --- Health Checks ---
var serviceUrls = builder.Configuration.GetSection("ServiceUrls");
var healthChecksBuilder = builder.Services.AddHealthChecks();

foreach (var service in serviceUrls.GetChildren())
{
    var serviceName = service.Key;
    var serviceUrl = service.Value;
    if (!string.IsNullOrEmpty(serviceUrl))
    {
        healthChecksBuilder.AddUrlGroup(
            new Uri($"{serviceUrl}/health"),
            name: $"{serviceName}-health",
            tags: ["downstream", serviceName.ToLower()]);
    }
}

// --- CORS ---
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
              .WithExposedHeaders("X-Correlation-ID");
    });
});

// --- Swagger ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ERP API Gateway", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });
});

var app = builder.Build();

// --- Middleware Pipeline ---
app.UseSerilogRequestLogging();

// Correlation ID (first in pipeline)
app.UseCorrelationId();

// Request/Response Logging
app.UseRequestResponseLogging();

// CORS
app.UseCors();

// Response Caching
app.UseResponseCaching();

// Rate Limiting
app.UseRateLimiter();

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ERP API Gateway v1");
        c.RoutePrefix = "swagger";
    });
}

// --- Gateway Info Endpoint ---
app.MapGet("/", () => Results.Ok(new
{
    service = "ERP API Gateway",
    version = "1.0.0",
    status = "running",
    timestamp = DateTime.UtcNow,
    endpoints = new
    {
        swagger = "/swagger",
        health = "/health",
        ocelotRoutes = "/api/{service-name}/{everything}",
        yarpRoutes = "/yarp/{service-name}/{everything}",
        graphql = "/graphql/{service-name}"
    }
}));

// --- Service Discovery Endpoint ---
app.MapGet("/api/services", () =>
{
    var services = new[]
    {
        new { Name = "SecurityService", Port = 5009, Prefix = "/api/security", GraphQL = "/graphql/security" },
        new { Name = "VehicleTracking", Port = 5102, Prefix = "/api/vehicle-tracking", GraphQL = "/graphql/vehicle-tracking" },
        new { Name = "DispatchPlanning", Port = 5255, Prefix = "/api/dispatch-planning", GraphQL = "/graphql/dispatch-planning" },
        new { Name = "OrderSchedule", Port = 5160, Prefix = "/api/order-schedule", GraphQL = "/graphql/order-schedule" },
        new { Name = "FillingOperation", Port = 5058, Prefix = "/api/filling-operation", GraphQL = "/graphql/filling-operation" },
        new { Name = "EximManagement", Port = 5085, Prefix = "/api/exim-management", GraphQL = "/graphql/exim-management" },
        new { Name = "GSTCompliance", Port = 5282, Prefix = "/api/gst-compliance", GraphQL = "/graphql/gst-compliance" },
        new { Name = "InventoryManagement", Port = 5097, Prefix = "/api/inventory-management", GraphQL = "/graphql/inventory-management" },
        new { Name = "ProductionManagement", Port = 5087, Prefix = "/api/production-management", GraphQL = "/graphql/production-management" },
        new { Name = "MamAllocation", Port = 5140, Prefix = "/api/mam-allocation", GraphQL = "/graphql/mam-allocation" },
        new { Name = "PurchaseSales", Port = 5170, Prefix = "/api/purchase-sales", GraphQL = "/graphql/purchase-sales" },
        new { Name = "MasterData", Port = 5180, Prefix = "/api/master-data", GraphQL = "/graphql/master-data" },
        new { Name = "StrategicStock", Port = 5045, Prefix = "/api/strategic-stock", GraphQL = "/graphql/strategic-stock" },
        new { Name = "ErrorLogging", Port = 5292, Prefix = "/api/error-logging", GraphQL = "/graphql/error-logging" },
        new { Name = "SciTransactional", Port = 5150, Prefix = "/api/sci-transactional", GraphQL = "/graphql/sci-transactional" }
    };
    return Results.Ok(new { totalServices = services.Length, services });
}).RequireRateLimiting("fixed");

// --- Health Check Endpoint ---
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = new
        {
            status = report.Status.ToString(),
            totalDuration = report.TotalDuration.TotalMilliseconds,
            timestamp = DateTime.UtcNow,
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                duration = e.Value.Duration.TotalMilliseconds,
                description = e.Value.Description,
                exception = e.Value.Exception?.Message
            })
        };
        await context.Response.WriteAsJsonAsync(result);
    }
});

// --- YARP Reverse Proxy ---
app.MapReverseProxy();

// --- Ocelot (must be last) ---
await app.UseOcelot();

await app.RunAsync();
