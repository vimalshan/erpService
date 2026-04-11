using System.Text;
using System.Threading.RateLimiting;
using ApiGateway.Middleware;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ───────────────────────────────────────────────
// 1. Serilog
// ───────────────────────────────────────────────
builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

// ───────────────────────────────────────────────
// 2. Ocelot + CacheManager + Polly (Circuit Breaker / Retry / Bulkhead)
// ───────────────────────────────────────────────
builder.Configuration
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

builder.Services
    .AddOcelot(builder.Configuration)
    .AddCacheManager(x => x.WithDictionaryHandle())
    .AddPolly();

// ───────────────────────────────────────────────
// 3. YARP Reverse Proxy (Load Balancing / Health Checks)
// ───────────────────────────────────────────────
builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// ───────────────────────────────────────────────
// 4. JWT Authentication & Authorization
// ───────────────────────────────────────────────
var jwtSection = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSection["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSection["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSection["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(1)
    };
});

builder.Services.AddAuthorization();

// ───────────────────────────────────────────────
// 5. Rate Limiting & Throttling
// ───────────────────────────────────────────────
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    // Fixed window — general API calls
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = builder.Configuration.GetValue("RateLimiting:Fixed:PermitLimit", 100);
        opt.Window = TimeSpan.FromSeconds(builder.Configuration.GetValue("RateLimiting:Fixed:Window", 60));
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = builder.Configuration.GetValue("RateLimiting:Fixed:QueueLimit", 10);
    });

    // Sliding window — read-heavy endpoints
    options.AddSlidingWindowLimiter("sliding", opt =>
    {
        opt.PermitLimit = builder.Configuration.GetValue("RateLimiting:Sliding:PermitLimit", 200);
        opt.Window = TimeSpan.FromSeconds(builder.Configuration.GetValue("RateLimiting:Sliding:Window", 60));
        opt.SegmentsPerWindow = builder.Configuration.GetValue("RateLimiting:Sliding:SegmentsPerWindow", 6);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = builder.Configuration.GetValue("RateLimiting:Sliding:QueueLimit", 10);
    });

    // Concurrency limiter — bulkhead isolation
    options.AddConcurrencyLimiter("concurrency", opt =>
    {
        opt.PermitLimit = builder.Configuration.GetValue("RateLimiting:Concurrency:PermitLimit", 50);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = builder.Configuration.GetValue("RateLimiting:Concurrency:QueueLimit", 25);
    });
});

// ───────────────────────────────────────────────
// 6. Response Caching
// ───────────────────────────────────────────────
builder.Services.AddResponseCaching();

// ───────────────────────────────────────────────
// 7. Health Checks — Gateway self + downstream services
// ───────────────────────────────────────────────
var healthCheckEndpoints = builder.Configuration
    .GetSection("HealthCheckEndpoints")
    .GetChildren();

var hcBuilder = builder.Services.AddHealthChecks()
    .AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy("Gateway is alive"));

foreach (var endpoint in healthCheckEndpoints)
{
    hcBuilder.AddUrlGroup(
        new Uri(endpoint.Value!),
        name: endpoint.Key,
        tags: new[] { "downstream" });
}

// ───────────────────────────────────────────────
// 8. CORS
// ───────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ───────────────────────────────────────────────
// 9. Swagger (Gateway overview)
// ───────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Tour ERP API Gateway",
        Version = "v1",
        Description = "Unified API Gateway for all Tour ERP microservices (Ocelot + YARP)"
    });
});

// ═══════════════════════════════════════════════
// Build
// ═══════════════════════════════════════════════
var app = builder.Build();

// ───────────────────────────────────────────────
// Middleware pipeline (order matters)
// ───────────────────────────────────────────────

// 1. Global exception handler
app.UseMiddleware<GlobalExceptionMiddleware>();

// 2. Correlation ID — attaches to every request
app.UseMiddleware<CorrelationIdMiddleware>();

// 3. Request/Response logging (with CorrelationId context)
app.UseMiddleware<RequestResponseLoggingMiddleware>();

// 4. Swagger (development only)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tour ERP Gateway v1");
        c.RoutePrefix = "swagger";
    });
}

// 5. CORS
app.UseCors();

// 6. Rate limiting
app.UseRateLimiter();

// 7. Response caching
app.UseResponseCaching();

// 8. Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// 9. Health check endpoints
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("downstream"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = check => !check.Tags.Contains("downstream"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// 10. Gateway info endpoint
app.MapGet("/", () => Results.Ok(new
{
    Service = "Tour ERP API Gateway",
    Version = "1.0.0",
    Timestamp = DateTime.UtcNow,
    Endpoints = new
    {
        Health = "/health",
        HealthReady = "/health/ready",
        HealthLive = "/health/live",
        Swagger = "/swagger",
        OcelotRoutes = "Ocelot routes via /api/{service}/*",
        YarpRoutes = "YARP routes via /yarp/{service}/*"
    }
}));

// 11. YARP reverse proxy
app.MapReverseProxy();

// 12. Ocelot pipeline (must be last — it takes over the pipeline)
await app.UseOcelot();

app.Run();
