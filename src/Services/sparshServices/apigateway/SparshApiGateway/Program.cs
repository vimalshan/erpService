using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using Serilog;
using SparshApiGateway.Middleware;
using SparshApiGateway.Resilience;

var builder = WebApplication.CreateBuilder(args);

// =============================================
// Serilog
// =============================================
builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

// =============================================
// Ocelot configuration file
// =============================================
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// =============================================
// Controllers (Gateway Auth & Info)
// =============================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Sparsh API Gateway", Version = "v1" });
});

// =============================================
// JWT Authentication & Authorization
// =============================================
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(
    jwtSettings["SecretKey"] ?? "SparshApiGateway_SuperSecret_Key_2026_Minimum32Chars!!");

var validIssuers = jwtSettings.GetSection("ValidIssuers").Get<string[]>()
    ?? ["SparshApiGateway"];
var validAudiences = jwtSettings.GetSection("ValidAudiences").Get<string[]>()
    ?? ["SparshServices"];

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
        ValidIssuers = validIssuers,
        ValidAudiences = validAudiences,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey)
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("authenticated", policy => policy.RequireAuthenticatedUser());
    options.AddPolicy("admin", policy => policy.RequireRole("Admin"));
});

// =============================================
// Rate Limiting & Throttling
// =============================================
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    // Fixed window rate limiter
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.Window = TimeSpan.FromSeconds(
            builder.Configuration.GetValue("RateLimiting:Fixed:Window", 60));
        opt.PermitLimit = builder.Configuration.GetValue("RateLimiting:Fixed:PermitLimit", 100);
        opt.QueueLimit = builder.Configuration.GetValue("RateLimiting:Fixed:QueueLimit", 10);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    // Sliding window rate limiter
    options.AddSlidingWindowLimiter("sliding", opt =>
    {
        opt.Window = TimeSpan.FromSeconds(
            builder.Configuration.GetValue("RateLimiting:Sliding:Window", 60));
        opt.SegmentsPerWindow = builder.Configuration.GetValue("RateLimiting:Sliding:SegmentsPerWindow", 6);
        opt.PermitLimit = builder.Configuration.GetValue("RateLimiting:Sliding:PermitLimit", 100);
        opt.QueueLimit = builder.Configuration.GetValue("RateLimiting:Sliding:QueueLimit", 10);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    // Concurrency limiter (Bulkhead at HTTP level)
    options.AddConcurrencyLimiter("concurrency", opt =>
    {
        opt.PermitLimit = builder.Configuration.GetValue("RateLimiting:Concurrency:PermitLimit", 50);
        opt.QueueLimit = builder.Configuration.GetValue("RateLimiting:Concurrency:QueueLimit", 25);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    // Per-client limiter using IP address
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
    {
        var clientId = context.Request.Headers["X-Client-Id"].FirstOrDefault()
            ?? context.Connection.RemoteIpAddress?.ToString()
            ?? "anonymous";

        return RateLimitPartition.GetFixedWindowLimiter(clientId, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 200,
            Window = TimeSpan.FromMinutes(1),
            QueueLimit = 20,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
        });
    });
});

// =============================================
// Response Caching
// =============================================
builder.Services.AddResponseCaching();
builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(b => b.Expire(TimeSpan.FromSeconds(10)));
    options.AddPolicy("CacheShort", b => b.Expire(TimeSpan.FromSeconds(30)));
    options.AddPolicy("CacheMedium", b => b.Expire(TimeSpan.FromMinutes(5)));
    options.AddPolicy("NoCache", b => b.NoCache());
});

// =============================================
// YARP Reverse Proxy
// =============================================
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// =============================================
// Ocelot
// =============================================
builder.Services.AddOcelot(builder.Configuration)
    .AddPolly();

// =============================================
// Health Checks (Gateway + Downstream Services)
// =============================================
var healthChecks = builder.Services.AddHealthChecks();
var services = builder.Configuration.GetSection("ServiceDiscovery:Services")
    .GetChildren().ToList();
foreach (var service in services)
{
    if (service.Value is not null)
    {
        healthChecks.AddUrlGroup(
            new Uri($"{service.Value}/health"),
            name: $"{service.Key}-health",
            tags: ["downstream", service.Key.ToLowerInvariant()]);
    }
}

// =============================================
// HttpClient with Resilience Policies (for manual forwarding if needed)
// =============================================
foreach (var service in services)
{
    if (service.Value is not null)
    {
        builder.Services.AddHttpClient(service.Key, client =>
        {
            client.BaseAddress = new Uri(service.Value);
            client.Timeout = TimeSpan.FromSeconds(30);
        })
        .AddPolicyHandler(ResiliencePolicies.GetRetryPolicy())
        .AddPolicyHandler(ResiliencePolicies.GetCircuitBreakerPolicy());
    }
}

// =============================================
// CORS
// =============================================
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
              .WithExposedHeaders("X-Correlation-Id");
    });
});

var app = builder.Build();

// =============================================
// Middleware Pipeline
// =============================================

// 1. Exception handling (outermost)
app.UseMiddleware<GatewayExceptionMiddleware>();

// 2. Correlation ID tracking
app.UseMiddleware<CorrelationIdMiddleware>();

// 3. Request/Response logging
app.UseMiddleware<RequestResponseLoggingMiddleware>();

// 4. Swagger (Development only)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sparsh API Gateway v1"));
}

// 5. CORS
app.UseCors();

// 6. Rate Limiting
app.UseRateLimiter();

// 7. Response Caching
app.UseResponseCaching();
app.UseOutputCache();

// 8. Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// 9. Controllers for gateway endpoints
app.MapControllers();

// 10. Health Checks
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = new
        {
            status = report.Status.ToString(),
            timestamp = DateTime.UtcNow,
            gateway = "Sparsh API Gateway",
            totalDuration = report.TotalDuration.TotalMilliseconds + "ms",
            checks = report.Entries.Select(e => new
            {
                service = e.Key,
                status = e.Value.Status.ToString(),
                duration = e.Value.Duration.TotalMilliseconds + "ms",
                description = e.Value.Description,
                exception = e.Value.Exception?.Message
            })
        };
        await context.Response.WriteAsJsonAsync(result);
    }
});

// 11. YARP Reverse Proxy (primary proxy engine)
app.MapReverseProxy();

// 12. Ocelot (secondary proxy engine, handles /ocelot/* routes only)
// app.Map strips the /ocelot prefix before forwarding to Ocelot
app.Map("/ocelot", ocelotApp =>
{
    ocelotApp.UseOcelot().Wait();
});

app.Run();
