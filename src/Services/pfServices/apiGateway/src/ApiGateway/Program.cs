using System.Text;
using System.Threading.RateLimiting;
using ApiGateway.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Polly;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ─── Serilog ────────────────────────────────────────────────────────────────
builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

// ─── Configuration: load ocelot.json alongside appsettings ──────────────────
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// ─── JWT Authentication ─────────────────────────────────────────────────────
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection["Key"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });
builder.Services.AddAuthorization();

// ─── Rate Limiting & Throttling ─────────────────────────────────────────────
var rateLimitSection = builder.Configuration.GetSection("RateLimiting");
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    // Fixed window per client IP
    options.AddPolicy("PerEndpoint", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = rateLimitSection.GetValue<int>("PermitLimit"),
                Window = TimeSpan.FromSeconds(rateLimitSection.GetValue<int>("WindowInSeconds")),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = rateLimitSection.GetValue<int>("QueueLimit")
            }));

    // Global concurrency limiter (bulkhead isolation)
    var bulkhead = builder.Configuration.GetSection("ResiliencePolicy:Bulkhead");
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetConcurrencyLimiter(
            partitionKey: "global",
            factory: _ => new ConcurrencyLimiterOptions
            {
                PermitLimit = bulkhead.GetValue<int>("MaxParallelization"),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = bulkhead.GetValue<int>("MaxQueuingActions")
            }));
});

// ─── Response Caching ───────────────────────────────────────────────────────
builder.Services.AddResponseCaching();

// ─── CORS ───────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("GatewayPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
              .WithExposedHeaders("X-Correlation-ID");
    });
});

// ─── YARP Reverse Proxy ─────────────────────────────────────────────────────
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// ─── Resilience (Circuit Breaker, Retry, Timeout) via HttpClientFactory ─────
builder.Services.AddHttpClient("GatewayClient")
    .AddStandardResilienceHandler(options =>
    {
        // Retry policy
        var retrySection = builder.Configuration.GetSection("ResiliencePolicy:Retry");
        options.Retry.MaxRetryAttempts = retrySection.GetValue<int>("MaxRetryAttempts");
        options.Retry.Delay = TimeSpan.FromMilliseconds(retrySection.GetValue<int>("DelayInMilliseconds"));
        options.Retry.BackoffType = DelayBackoffType.Exponential;

        // Circuit Breaker
        var cbSection = builder.Configuration.GetSection("ResiliencePolicy:CircuitBreaker");
        options.CircuitBreaker.FailureRatio = cbSection.GetValue<double>("FailureRatio");
        options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(cbSection.GetValue<int>("SamplingDurationInSeconds"));
        options.CircuitBreaker.MinimumThroughput = cbSection.GetValue<int>("MinimumThroughput");
        options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(cbSection.GetValue<int>("BreakDurationInSeconds"));

        // Timeout
        var timeoutSection = builder.Configuration.GetSection("ResiliencePolicy:Timeout");
        options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(timeoutSection.GetValue<int>("TimeoutInSeconds"));
    });

// ─── Health Checks ──────────────────────────────────────────────────────────
builder.Services.AddHealthChecks()
    .AddUrlGroup(new Uri("http://localhost:5278/health"), name: "member-service", tags: ["services"])
    .AddUrlGroup(new Uri("http://localhost:5225/health"), name: "contribution-service", tags: ["services"])
    .AddUrlGroup(new Uri("http://localhost:5171/health"), name: "investment-service", tags: ["services"])
    .AddUrlGroup(new Uri("http://localhost:5149/health"), name: "settlement-service", tags: ["services"])
    .AddUrlGroup(new Uri("http://localhost:5004/health"), name: "loan-service", tags: ["services"])
    .AddUrlGroup(new Uri("http://localhost:5068/health"), name: "accounting-service", tags: ["services"])
    .AddUrlGroup(new Uri("http://localhost:5125/health"), name: "bank-service", tags: ["services"])
    .AddUrlGroup(new Uri("http://localhost:5090/health"), name: "masterdata-service", tags: ["services"])
    .AddUrlGroup(new Uri("http://localhost:5160/health"), name: "pftransactional-service", tags: ["services"])
    .AddUrlGroup(new Uri("http://localhost:5079/health"), name: "trust-service", tags: ["services"]);

var app = builder.Build();

// ─── Middleware Pipeline (order matters) ────────────────────────────────────

// 1. Correlation ID (first, so every downstream log/header gets it)
app.UseMiddleware<CorrelationIdMiddleware>();

// 2. Serilog request logging
app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("CorrelationId",
            httpContext.Request.Headers["X-Correlation-ID"].FirstOrDefault() ?? httpContext.TraceIdentifier);
        diagnosticContext.Set("ClientIp", httpContext.Connection.RemoteIpAddress?.ToString());
    };
});

// 3. Request/Response logging middleware
app.UseMiddleware<RequestResponseLoggingMiddleware>();

// 4. CORS
app.UseCors("GatewayPolicy");

// 5. Response caching
app.UseResponseCaching();

// 6. Rate limiting & bulkhead
app.UseRateLimiter();

// 7. Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// ─── Health Check Endpoints ─────────────────────────────────────────────────
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = new
        {
            status = report.Status.ToString(),
            totalDuration = report.TotalDuration.TotalMilliseconds,
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

app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = _ => false // liveness — no dependency checks
});

app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("services")
});

// ─── Gateway Info Endpoint ──────────────────────────────────────────────────
app.MapGet("/", () => Results.Ok(new
{
    service = "PF Services API Gateway",
    version = "1.0.0",
    status = "Running",
    endpoints = new
    {
        health = "/health",
        liveness = "/health/live",
        readiness = "/health/ready",
        services = new Dictionary<string, string>
        {
            ["member"]          = "/api/member/{path}",
            ["contribution"]    = "/api/contribution/{path}",
            ["investment"]      = "/api/investment/{path}",
            ["settlement"]      = "/api/settlement/{path}",
            ["loan"]            = "/api/loan/{path}",
            ["accounting"]      = "/api/accounting/{path}",
            ["bank"]            = "/api/bank/{path}",
            ["masterdata"]      = "/api/masterdata/{path}",
            ["pftransactional"] = "/api/pftransactional/{path}",
            ["trust"]           = "/api/trust/{path}"
        }
    }
}));

// ─── YARP Reverse Proxy ─────────────────────────────────────────────────────
app.MapReverseProxy();

app.Run();
