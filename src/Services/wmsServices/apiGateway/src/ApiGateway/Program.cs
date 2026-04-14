using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;
using ApiGateway.DelegatingHandlers;
using ApiGateway.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// Serilog Configuration
// ============================================================
builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

// ============================================================
// Ocelot Configuration
// ============================================================
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

builder.Services
    .AddOcelot(builder.Configuration)
    .AddCacheManager(x => x.WithDictionaryHandle())
    .AddPolly();

// ============================================================
// YARP Reverse Proxy Configuration
// ============================================================
builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// ============================================================
// JWT Authentication & Authorization
// ============================================================
var jwtKey = builder.Configuration["Jwt:Key"]!;
var jwtIssuer = builder.Configuration["Jwt:Issuer"]!;
var jwtAudience = builder.Configuration["Jwt:Audience"]!;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer("GatewayAuth", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ClockSkew = TimeSpan.FromMinutes(1)
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogWarning("JWT Authentication failed: {Error}", context.Exception.Message);
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

// ============================================================
// Rate Limiting & Throttling (ASP.NET Core built-in)
// ============================================================
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    // Global fixed-window rate limit
    options.AddFixedWindowLimiter("GlobalRateLimit", opt =>
    {
        opt.PermitLimit = builder.Configuration.GetValue("RateLimiting:GlobalRateLimit:PermitLimit", 500);
        opt.Window = TimeSpan.FromSeconds(builder.Configuration.GetValue("RateLimiting:GlobalRateLimit:Window", 60));
        opt.QueueLimit = builder.Configuration.GetValue("RateLimiting:GlobalRateLimit:QueueLimit", 10);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    // Per-client sliding window rate limit
    options.AddSlidingWindowLimiter("PerClientRateLimit", opt =>
    {
        opt.PermitLimit = builder.Configuration.GetValue("RateLimiting:PerClientRateLimit:PermitLimit", 100);
        opt.Window = TimeSpan.FromSeconds(builder.Configuration.GetValue("RateLimiting:PerClientRateLimit:Window", 60));
        opt.SegmentsPerWindow = 6;
        opt.QueueLimit = builder.Configuration.GetValue("RateLimiting:PerClientRateLimit:QueueLimit", 5);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    // Client ID extraction
    options.OnRejected = async (context, cancellationToken) =>
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
        var clientId = context.HttpContext.Request.Headers["X-ClientId"].FirstOrDefault() ?? "Anonymous";
        logger.LogWarning("Rate limit exceeded for client {ClientId} on {Path}", clientId, context.HttpContext.Request.Path);

        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.ContentType = "application/json";
        await context.HttpContext.Response.WriteAsJsonAsync(new
        {
            statusCode = 429,
            message = "Too many requests. Please try again later.",
            retryAfter = "60s"
        }, cancellationToken);
    };
});

// ============================================================
// Health Checks for all downstream services
// ============================================================
var healthBuilder = builder.Services.AddHealthChecks();
var serviceHealthChecks = builder.Configuration.GetSection("ServiceHealthChecks").GetChildren();

foreach (var service in serviceHealthChecks)
{
    healthBuilder.AddUrlGroup(
        new Uri(service.Value!),
        name: service.Key,
        tags: ["downstream"]);
}

// ============================================================
// Delegating Handlers (Circuit Breaker, Retry, Bulkhead)
// ============================================================
builder.Services.AddTransient<CircuitBreakerDelegatingHandler>();
builder.Services.AddTransient<RetryDelegatingHandler>();
builder.Services.AddTransient<BulkheadDelegatingHandler>();

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
              .WithExposedHeaders("X-Correlation-ID", "X-RateLimit-Limit", "X-RateLimit-Remaining");
    });
});

// ============================================================
// Response Caching
// ============================================================
builder.Services.AddResponseCaching();
builder.Services.AddMemoryCache();

var app = builder.Build();

// ============================================================
// Middleware Pipeline (Order matters!)
// ============================================================

// 1. Global exception handling
app.UseMiddleware<GlobalExceptionMiddleware>();

// 2. Correlation ID tracking
app.UseMiddleware<CorrelationIdMiddleware>();

// 3. Request/Response logging
app.UseMiddleware<RequestResponseLoggingMiddleware>();

// 4. CORS
app.UseCors("GatewayCors");

// 5. Rate Limiting
app.UseRateLimiter();

// 6. Response Caching
app.UseResponseCaching();

// 7. Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// ============================================================
// Gateway local endpoints handled via middleware (before Ocelot)
// Ocelot's UseOcelot() is terminal and swallows ALL requests,
// so we must handle local endpoints explicitly before it runs.
// ============================================================
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value?.ToLower() ?? "";

    if (context.Request.Method == "GET" && path == "/")
    {
        await context.Response.WriteAsJsonAsync(new
        {
            service = "WMS API Gateway",
            version = "1.0.0",
            framework = "Ocelot + YARP",
            features = new[]
            {
                "JWT Authentication & Authorization",
                "Rate Limiting & Throttling",
                "Circuit Breaker Pattern",
                "Retry & Timeout Handling",
                "Bulkhead Isolation",
                "Request/Response Logging",
                "Correlation ID Tracking",
                "Health Checks & Monitoring",
                "Response Caching",
                "Load Balancing (Round Robin)"
            },
            endpoints = new
            {
                ocelot = new
                {
                    security = "/api/security/{everything}",
                    warehouse = "/api/warehouse/{everything}",
                    racking = "/api/racking/{everything}",
                    employee = "/api/employee/{everything}",
                    product = "/api/product/{everything}",
                    inventory = "/api/inventory/{everything}",
                    supplier = "/api/supplier/{everything}",
                    customer = "/api/customer/{everything}",
                    purchaseOrder = "/api/purchaseorder/{everything}",
                    receiving = "/api/receiving/{everything}",
                    salesOrder = "/api/salesorder/{everything}",
                    shipment = "/api/shipment/{everything}",
                    order = "/api/order/{everything}",
                    fleet = "/api/fleet/{everything}",
                    auditLog = "/api/auditlog/{everything}",
                    transactional = "/api/transactional/{everything}"
                },
                yarp = new
                {
                    security = "/yarp/security/{everything}",
                    warehouse = "/yarp/warehouse/{everything}",
                    racking = "/yarp/racking/{everything}",
                    employee = "/yarp/employee/{everything}",
                    product = "/yarp/product/{everything}",
                    inventory = "/yarp/inventory/{everything}",
                    supplier = "/yarp/supplier/{everything}",
                    customer = "/yarp/customer/{everything}",
                    purchaseOrder = "/yarp/purchaseorder/{everything}",
                    receiving = "/yarp/receiving/{everything}",
                    salesOrder = "/yarp/salesorder/{everything}",
                    shipment = "/yarp/shipment/{everything}",
                    order = "/yarp/order/{everything}",
                    fleet = "/yarp/fleet/{everything}",
                    auditLog = "/yarp/auditlog/{everything}",
                    transactional = "/yarp/transactional/{everything}"
                },
                graphql = "Each service: /api/{service}/graphql",
                health = new[] { "/health", "/health/ready", "/health/live" }
            },
            timestamp = DateTime.UtcNow
        });
        return;
    }

    if (context.Request.Method == "GET" && path == "/health")
    {
        await context.Response.WriteAsJsonAsync(new
        {
            status = "Healthy",
            service = "WMS API Gateway",
            timestamp = DateTime.UtcNow
        });
        return;
    }

    if (context.Request.Method == "GET" && path == "/health/live")
    {
        await context.Response.WriteAsJsonAsync(new
        {
            status = "Alive",
            timestamp = DateTime.UtcNow
        });
        return;
    }

    if (context.Request.Method == "POST" && path == "/api/gateway/token")
    {
        var config = context.RequestServices.GetRequiredService<IConfiguration>();
        LoginRequest? request;
        try
        {
            request = await context.Request.ReadFromJsonAsync<LoginRequest>();
        }
        catch
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new { error = "Invalid request body." });
            return;
        }

        if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new { error = "Username and password are required." });
            return;
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, request.Username),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var expiryMinutes = int.Parse(config["Jwt:ExpiryInMinutes"] ?? "120");
        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: credentials);

        await context.Response.WriteAsJsonAsync(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            expiration = token.ValidTo
        });
        return;
    }

    await next();
});

// ============================================================
// YARP Reverse Proxy (for /yarp/* routes)
// ============================================================
app.MapReverseProxy();

// ============================================================
// Ocelot Pipeline (must be last - it takes over routing)
// ============================================================
await app.UseOcelot();

app.Run();

public record LoginRequest(string Username, string Password);
