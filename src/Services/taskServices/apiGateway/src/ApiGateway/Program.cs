using System.Text;
using System.Text.Json;
using System.Threading.RateLimiting;
using ApiGateway.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using Ocelot.Cache.CacheManager;
using Polly;
using Polly.CircuitBreaker;
using Polly.Timeout;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ──────────────────────────────────────────────────
// 1. SERILOG
// ──────────────────────────────────────────────────
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Service", "ApiGateway")
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();
builder.Host.UseSerilog();

// ──────────────────────────────────────────────────
// 2. JWT AUTHENTICATION & AUTHORIZATION
// ──────────────────────────────────────────────────
var jwtKey = builder.Configuration["Jwt:Key"]!;
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "TaskTransactional";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "TaskTransactional";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer("Bearer", options =>
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
        ClockSkew = TimeSpan.FromMinutes(2)
    };
});
builder.Services.AddAuthorization();

// ──────────────────────────────────────────────────
// 3. RATE LIMITING & THROTTLING (built-in .NET)
// ──────────────────────────────────────────────────
var rlConfig = builder.Configuration.GetSection("RateLimiting");
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = 429;

    // Fixed Window policy (attached to YARP routes via "RateLimiterPolicy": "fixed")
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = rlConfig.GetValue("PermitLimit", 100);
        opt.Window = TimeSpan.FromSeconds(rlConfig.GetValue("WindowSeconds", 60));
        opt.QueueLimit = rlConfig.GetValue("QueueLimit", 10);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    // Sliding Window policy for more granular limiting
    options.AddSlidingWindowLimiter("sliding", opt =>
    {
        opt.PermitLimit = 200;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.SegmentsPerWindow = 6;
        opt.QueueLimit = 20;
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    // Concurrency limiter for bulkhead isolation
    var bhConfig = builder.Configuration.GetSection("Bulkhead");
    options.AddConcurrencyLimiter("bulkhead", opt =>
    {
        opt.PermitLimit = bhConfig.GetValue("MaxParallelization", 25);
        opt.QueueLimit = bhConfig.GetValue("MaxQueuingActions", 50);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    // Global (fallback) limiter per client IP
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(ctx =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 500,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            }));

    options.OnRejected = async (context, ct) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        context.HttpContext.Response.ContentType = "application/json";
        var response = new
        {
            status = 429,
            title = "Too Many Requests",
            detail = "Rate limit exceeded. Please wait before making another request.",
            retryAfter = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter)
                ? retryAfter.TotalSeconds : 60
        };
        await context.HttpContext.Response.WriteAsync(JsonSerializer.Serialize(response), ct);
    };
});

// ──────────────────────────────────────────────────
// 4. POLLY: CIRCUIT BREAKER + RETRY + TIMEOUT + BULKHEAD
// ──────────────────────────────────────────────────
var cbConfig = builder.Configuration.GetSection("CircuitBreaker");
var retryConfig = builder.Configuration.GetSection("Retry");
var timeoutConfig = builder.Configuration.GetSection("Timeout");
var bulkheadConfig = builder.Configuration.GetSection("Bulkhead");

// Register a named HttpClient with Polly policies for downstream calls
builder.Services.AddHttpClient("GatewayClient")
    .AddPolicyHandler(Policy<HttpResponseMessage>
        .Handle<HttpRequestException>()
        .OrResult(r => (int)r.StatusCode >= 500)
        .WaitAndRetryAsync(
            retryConfig.GetValue("MaxRetryAttempts", 3),
            attempt => TimeSpan.FromSeconds(Math.Pow(retryConfig.GetValue("BaseDelaySeconds", 2), attempt)),
            onRetry: (outcome, delay, attempt, _) =>
            {
                Log.Warning("Retry {Attempt} after {Delay}s — {Reason}",
                    attempt, delay.TotalSeconds,
                    outcome.Exception?.Message ?? $"HTTP {(int?)outcome.Result?.StatusCode}");
            }))
    .AddPolicyHandler(Policy<HttpResponseMessage>
        .Handle<HttpRequestException>()
        .OrResult(r => (int)r.StatusCode >= 500)
        .CircuitBreakerAsync(
            handledEventsAllowedBeforeBreaking: cbConfig.GetValue("HandledEventsAllowedBeforeBreaking", 5),
            durationOfBreak: TimeSpan.FromSeconds(cbConfig.GetValue("DurationOfBreakSeconds", 30)),
            onBreak: (_, duration) => Log.Error("Circuit OPEN for {Duration}s", duration.TotalSeconds),
            onReset: () => Log.Information("Circuit CLOSED — recovered"),
            onHalfOpen: () => Log.Information("Circuit HALF-OPEN — testing")))
    .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(
        TimeSpan.FromSeconds(timeoutConfig.GetValue("Seconds", 30)),
        TimeoutStrategy.Optimistic))
    .AddPolicyHandler(Policy.BulkheadAsync<HttpResponseMessage>(
        bulkheadConfig.GetValue("MaxParallelization", 25),
        bulkheadConfig.GetValue("MaxQueuingActions", 50)));

// ──────────────────────────────────────────────────
// 5. RESPONSE CACHING
// ──────────────────────────────────────────────────
builder.Services.AddResponseCaching();
builder.Services.AddOutputCache(options =>
{
    var cacheDuration = builder.Configuration.GetValue("Caching:DefaultDurationSeconds", 60);
    options.AddBasePolicy(b => b.Expire(TimeSpan.FromSeconds(cacheDuration)));
    options.AddPolicy("no-cache", b => b.NoCache());
    options.AddPolicy("short-cache", b => b.Expire(TimeSpan.FromSeconds(15)));
    options.AddPolicy("long-cache", b => b.Expire(TimeSpan.FromMinutes(10)));
});

// ──────────────────────────────────────────────────
// 6. YARP REVERSE PROXY (primary gateway)
// ──────────────────────────────────────────────────
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// ──────────────────────────────────────────────────
// 7. OCELOT CONFIGURATION (secondary gateway)
// ──────────────────────────────────────────────────
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration)
    .AddPolly()
    .AddCacheManager(x => x.WithDictionaryHandle());

// ──────────────────────────────────────────────────
// 8. HEALTH CHECKS
// ──────────────────────────────────────────────────
builder.Services.AddHealthChecks()
    .AddUrlGroup(new Uri("http://localhost:5188/health/live"), name: "lookup-service", tags: ["services"])
    .AddUrlGroup(new Uri("http://localhost:5257/health"), name: "task-service", tags: ["services"])
    .AddUrlGroup(new Uri("http://localhost:5310/health/live"), name: "transactional-service", tags: ["services"])
    .AddUrlGroup(new Uri("http://localhost:5070/health"), name: "complaint-service", tags: ["services"])
    .AddUrlGroup(new Uri("http://localhost:5160/health/live"), name: "energy-service", tags: ["services"])
    .AddUrlGroup(new Uri("http://localhost:5154/health/live"), name: "unit-service", tags: ["services"])
    .AddRabbitMQ(sp =>
    {
        var config = sp.GetRequiredService<IConfiguration>();
        var factory = new RabbitMQ.Client.ConnectionFactory
        {
            HostName = config["RabbitMQ:HostName"] ?? "localhost",
            UserName = config["RabbitMQ:UserName"] ?? "guest",
            Password = config["RabbitMQ:Password"] ?? "guest",
            Port = int.TryParse(config["RabbitMQ:Port"], out var port) ? port : 5672
        };
        return factory.CreateConnectionAsync().GetAwaiter().GetResult();
    }, name: "rabbitmq", tags: ["infrastructure"]);

// ──────────────────────────────────────────────────
// 9. SWAGGER
// ──────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ERP API Gateway", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header. Enter 'Bearer' [space] and then your token.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement((_) => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer"),
            new List<string>()
        }
    });
});

// ──────────────────────────────────────────────────
// 10. CORS
// ──────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
              .WithExposedHeaders("X-Correlation-ID", "X-RateLimit-Remaining", "Retry-After");
    });
});

// ──────────────────────────────────────────────────
// 11. REQUEST TIMEOUT (YARP route-level)
// ──────────────────────────────────────────────────
builder.Services.AddRequestTimeouts(options =>
{
    options.DefaultPolicy = new Microsoft.AspNetCore.Http.Timeouts.RequestTimeoutPolicy
    {
        Timeout = TimeSpan.FromSeconds(timeoutConfig.GetValue("Seconds", 30))
    };
    options.AddPolicy("default-timeout", TimeSpan.FromSeconds(timeoutConfig.GetValue("Seconds", 30)));
    options.AddPolicy("long-timeout", TimeSpan.FromSeconds(120));
});

// ═══════════════════════════════════════════════════
//  BUILD APP
// ═══════════════════════════════════════════════════
var app = builder.Build();

// ──────────────────────────────────────────────────
// MIDDLEWARE PIPELINE (order matters)
// ──────────────────────────────────────────────────

// 1. Global exception handler (outermost)
app.UseMiddleware<GlobalExceptionMiddleware>();

// 2. Correlation ID injection
app.UseMiddleware<CorrelationIdMiddleware>();

// 3. Request/Response logging
app.UseMiddleware<RequestResponseLoggingMiddleware>();

// 4. CORS
app.UseCors();

// 5. Rate Limiting
app.UseRateLimiter();

// 6. Response Caching + Output Cache
app.UseResponseCaching();
app.UseOutputCache();

// 7. Request Timeouts
app.UseRequestTimeouts();

// 8. Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// 9. Swagger (Development only)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gateway v1");
        c.SwaggerEndpoint("/swagger/lookup/v1/swagger.json", "Lookup Service");
        c.SwaggerEndpoint("/swagger/task/v1/swagger.json", "Task Service");
        c.SwaggerEndpoint("/swagger/transactional/v1/swagger.json", "Transactional Service");
        c.SwaggerEndpoint("/swagger/complaint/v1/swagger.json", "Complaint Service");
        c.SwaggerEndpoint("/swagger/energy/v1/swagger.json", "Energy Service");
        c.SwaggerEndpoint("/swagger/unit/v1/swagger.json", "Unit Service");
    });
}

// ──────────────────────────────────────────────────
// GATEWAY STATUS ENDPOINT
// ──────────────────────────────────────────────────
app.MapGet("/", () => Results.Ok(new
{
    service = "ERP API Gateway",
    version = "1.0.0",
    timestamp = DateTime.UtcNow,
    documentation = "/swagger",
    health = "/gateway/health",
    routes = new
    {
        yarp = new
        {
            description = "Primary gateway — YARP Reverse Proxy",
            prefix = "/api/{service}/...",
            services = new[]
            {
                new { name = "Lookup Service",       prefix = "/api/lookup/",        port = 5188 },
                new { name = "Task Service",         prefix = "/api/task/",           port = 5257 },
                new { name = "Transactional Service", prefix = "/api/transactional/", port = 5310 },
                new { name = "Complaint Service",    prefix = "/api/complaint/",      port = 5070 },
                new { name = "Energy Service",       prefix = "/api/energy/",         port = 5160 },
                new { name = "Unit Service",         prefix = "/api/unit/",           port = 5154 }
            }
        },
        ocelot = new
        {
            description = "Secondary gateway — Ocelot",
            prefix = "/ocelot/{service}/...",
            services = new[]
            {
                new { name = "Lookup Service",       prefix = "/ocelot/lookup/" },
                new { name = "Task Service",         prefix = "/ocelot/task/" },
                new { name = "Transactional Service", prefix = "/ocelot/transactional/" },
                new { name = "Complaint Service",    prefix = "/ocelot/complaint/" },
                new { name = "Energy Service",       prefix = "/ocelot/energy/" },
                new { name = "Unit Service",         prefix = "/ocelot/unit/" }
            }
        },
        graphql = new
        {
            description = "GraphQL endpoints proxied through YARP",
            endpoints = new[]
            {
                "/graphql/lookup/",
                "/graphql/task/",
                "/graphql/transactional/",
                "/graphql/complaint/",
                "/graphql/energy/",
                "/graphql/unit/"
            }
        }
    }
})).WithName("GatewayHome");

// ──────────────────────────────────────────────────
// AUTH ENDPOINT (gateway-level token generation)
// ──────────────────────────────────────────────────
app.MapPost("/gateway/auth/login", (LoginRequest request, IConfiguration config) =>
{
    if (request.Username != "admin" || request.Password != "admin123")
        return Results.Unauthorized();

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
    var creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);
    var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
        issuer: config["Jwt:Issuer"],
        audience: config["Jwt:Audience"],
        claims: [
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, request.Username),
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, "Admin"),
            new System.Security.Claims.Claim("jti", Guid.NewGuid().ToString()),
            new System.Security.Claims.Claim("sub", request.Username)
        ],
        expires: DateTime.UtcNow.AddHours(8),
        signingCredentials: creds);

    return Results.Ok(new
    {
        token = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token),
        expiry = token.ValidTo
    });
}).WithName("GatewayLogin").AllowAnonymous();

// ──────────────────────────────────────────────────
// HEALTH CHECK ENDPOINTS
// ──────────────────────────────────────────────────
app.MapHealthChecks("/gateway/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(new
        {
            gateway = "ERP API Gateway",
            status = report.Status.ToString(),
            totalDuration = $"{report.TotalDuration.TotalMilliseconds:F0}ms",
            timestamp = DateTime.UtcNow,
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description,
                duration = $"{e.Value.Duration.TotalMilliseconds:F0}ms",
                tags = e.Value.Tags
            })
        });
        await context.Response.WriteAsync(result);
    }
});

app.MapHealthChecks("/gateway/health/services", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("services"),
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            services = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                duration = $"{e.Value.Duration.TotalMilliseconds:F0}ms"
            })
        });
        await context.Response.WriteAsync(result);
    }
});

app.MapHealthChecks("/gateway/health/live", new HealthCheckOptions
{
    Predicate = _ => false
});

app.MapHealthChecks("/gateway/health/infra", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("infrastructure"),
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            infrastructure = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                duration = $"{e.Value.Duration.TotalMilliseconds:F0}ms"
            })
        });
        await context.Response.WriteAsync(result);
    }
});

// ──────────────────────────────────────────────────
// MAP YARP & OCELOT
// ──────────────────────────────────────────────────

// YARP: primary reverse proxy for /api/{service}/ and /graphql/{service}/
app.MapReverseProxy();

// Ocelot: secondary gateway for /ocelot/{service}/
// Branch Ocelot to only handle /ocelot/ prefixed requests (Ocelot is terminal middleware)
app.MapWhen(
    context => context.Request.Path.StartsWithSegments("/ocelot"),
    ocelotApp =>
    {
        ocelotApp.UseOcelot().Wait();
    });

app.Run();

// ─── Auth Request Model ──────────────────────────
record LoginRequest(string Username, string Password);
