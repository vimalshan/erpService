using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using Serilog;
using ApiGateway.Middleware;
using ApiGateway.Extensions;
using ApiGateway.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// ─── Serilog ────────────────────────────────────────────────────────────────────
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("Yarp", Serilog.Events.LogEventLevel.Warning)
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File("logs/api-gateway-.log", rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {CorrelationId} {Message:lj}{NewLine}{Exception}")
    .CreateLogger();
builder.Host.UseSerilog();

// ─── Configuration ──────────────────────────────────────────────────────────────
builder.Configuration
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? "your-very-long-secret-key-change-this-in-production-at-least-32-characters";

// ─── JWT Authentication ─────────────────────────────────────────────────────────
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer("GatewayAuth", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ValidateIssuer = false,   // Gateway accepts tokens from any service issuer
        ValidateAudience = false, // Gateway forwards to downstream; downstream validates audience
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromSeconds(30)
    };
});
builder.Services.AddAuthorization();

// ─── Rate Limiting (built-in .NET) ──────────────────────────────────────────────
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    // Global fixed window
    options.AddFixedWindowLimiter("GlobalFixed", opt =>
    {
        opt.PermitLimit = 100;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 10;
    });

    // Per-client sliding window
    options.AddSlidingWindowLimiter("PerClient", opt =>
    {
        opt.PermitLimit = 50;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.SegmentsPerWindow = 6;
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 5;
    });

    // Strict limiter for auth endpoints
    options.AddTokenBucketLimiter("AuthEndpoints", opt =>
    {
        opt.TokenLimit = 20;
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 5;
        opt.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
        opt.TokensPerPeriod = 5;
    });

    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.ContentType = "application/json";
        await context.HttpContext.Response.WriteAsJsonAsync(new
        {
            error = "Too many requests. Please try again later.",
            retryAfter = "60 seconds"
        }, cancellationToken);
    };
});

// ─── Response Caching ───────────────────────────────────────────────────────────
builder.Services.AddResponseCaching();
builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(b => b.Expire(TimeSpan.FromSeconds(30)));
    options.AddPolicy("CacheGetRequests", b =>
        b.Expire(TimeSpan.FromMinutes(2))
         .SetVaryByQuery("*")
         .Tag("api-cache"));
});

// ─── YARP Reverse Proxy ─────────────────────────────────────────────────────────
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// ─── Resilience (Polly via HttpClient) ──────────────────────────────────────────
builder.Services.AddResilienceServices();

// ─── Health Checks ──────────────────────────────────────────────────────────────
builder.Services.AddGatewayHealthChecks(builder.Configuration);

// ─── Ocelot ─────────────────────────────────────────────────────────────────────
builder.Services.AddOcelot(builder.Configuration)
    .AddPolly();

// ─── CORS ───────────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("GatewayCors", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// ─── Middleware Pipeline (order matters) ─────────────────────────────────────────

// 1. Correlation ID (first — so all logs have it)
app.UseMiddleware<CorrelationIdMiddleware>();

// 2. Request/Response Logging
app.UseMiddleware<RequestResponseLoggingMiddleware>();

// 3. Rate Limiting
app.UseRateLimiter();

// 4. Response Caching
app.UseResponseCaching();
app.UseOutputCache();

// 5. CORS
app.UseCors("GatewayCors");

// 6. Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// 7. Health Check endpoints
app.UseGatewayHealthChecks();

// 8. Gateway Dashboard (handled before Ocelot via middleware)
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/gateway/status"))
    {
        await context.Response.WriteAsJsonAsync(new
        {
            gateway = "DD ERP API Gateway",
            version = "1.0.0",
            timestamp = DateTime.UtcNow,
            environment = app.Environment.EnvironmentName
        });
        return;
    }

    if (context.Request.Path.StartsWithSegments("/gateway/services"))
    {
        var config = context.RequestServices.GetRequiredService<IConfiguration>();
        var routes = config.GetSection("ReverseProxy:Clusters").GetChildren()
            .Select(c => new
            {
                service = c.Key,
                destinations = c.GetSection("Destinations").GetChildren()
                    .Select(d => d["Address"]).ToArray()
            });
        await context.Response.WriteAsJsonAsync(routes);
        return;
    }

    await next();
});

// 9. YARP Reverse Proxy (handles /yarp/* routes)
app.MapReverseProxy();

// 10. Ocelot Pipeline (handles /api/* routes as primary gateway)
await app.UseOcelot();

// 11. Run the application
await app.RunAsync();
