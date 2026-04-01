using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;
using ApiGateway.HealthChecks;
using ApiGateway.Middleware;
using ApiGateway.Resilience;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace ApiGateway;

public class Program
{
    public static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        try
        {
            var builder = WebApplication.CreateBuilder(args);

            // ─── Serilog ────────────────────────────────────────────────
            builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

            // ─── YARP Reverse Proxy ─────────────────────────────────────
            builder.Services
                .AddReverseProxy()
                .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

            // ─── JWT Authentication ─────────────────────────────────────
            var jwt = builder.Configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwt["Key"]!);

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opts =>
                {
                    opts.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer           = true,
                        ValidateAudience         = true,
                        ValidateLifetime         = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer              = jwt["Issuer"],
                        ValidAudience            = jwt["Audience"],
                        IssuerSigningKey         = new SymmetricSecurityKey(key),
                        ClockSkew                = TimeSpan.FromMinutes(1)
                    };
                    opts.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = ctx =>
                        {
                            Log.Warning("JWT auth failed: {Error}", ctx.Exception.Message);
                            return Task.CompletedTask;
                        }
                    };
                });

            builder.Services.AddAuthorization(opts =>
            {
                opts.AddPolicy("AdminOnly",  p => p.RequireRole("Admin"));
                opts.AddPolicy("ReviewerOnly", p => p.RequireRole("Reviewer"));
            });

            // ─── Rate Limiting (ASP.NET Core built-in) ──────────────────
            builder.Services.AddRateLimiter(opts =>
            {
                opts.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                opts.OnRejected = async (ctx, ct) =>
                {
                    ctx.HttpContext.Response.ContentType = "application/json";
                    await ctx.HttpContext.Response.WriteAsync(
                        """{"type":"https://httpstatuses.com/429","title":"Too Many Requests","status":429,"detail":"Rate limit exceeded. Try again later."}""", ct);
                };

                // Global fixed-window limiter
                opts.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(ctx =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        ctx.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
                        _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 120,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 10,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                        }));

                // Per-second sliding window
                opts.AddPolicy("per-second", ctx =>
                    RateLimitPartition.GetSlidingWindowLimiter(
                        ctx.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
                        _ => new SlidingWindowRateLimiterOptions
                        {
                            PermitLimit = 10,
                            Window = TimeSpan.FromSeconds(1),
                            SegmentsPerWindow = 2,
                            QueueLimit = 5,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                        }));

                // Strict limiter for auth endpoints
                opts.AddPolicy("auth-endpoints", ctx =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        ctx.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
                        _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 20,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0
                        }));
            });

            // ─── AspNetCoreRateLimit (IP-based, for ocelot.json parity) ─
            builder.Services.AddMemoryCache();
            builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
            builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            builder.Services.AddInMemoryRateLimiting();

            // ─── Response Caching ───────────────────────────────────────
            builder.Services.AddResponseCaching(opts =>
            {
                opts.MaximumBodySize = 64 * 1024 * 1024; // 64 MB
                opts.UseCaseSensitivePaths = false;
            });
            builder.Services.AddOutputCache(opts =>
            {
                opts.AddBasePolicy(b => b.NoCache());
                opts.AddPolicy("CacheGet30s", b => b
                    .Expire(TimeSpan.FromSeconds(30))
                    .SetVaryByQuery("*")
                    .Tag("gateway-cache"));
                opts.AddPolicy("CacheMaster60s", b => b
                    .Expire(TimeSpan.FromSeconds(60))
                    .SetVaryByQuery("*")
                    .Tag("master-cache"));
            });

            // ─── Resilience (Polly) ─────────────────────────────────────
            builder.Services.AddGatewayResilience();

            // ─── Health Checks ──────────────────────────────────────────
            builder.Services.AddHttpClient("HealthChecks");
            builder.Services
                .AddHealthChecks()
                .AddCheck<GatewayHealthCheck>("gateway", tags: ["live"])
                .AddCheck<DownstreamServiceHealthCheck>("downstream-services", tags: ["ready"]);

            // ─── CORS ───────────────────────────────────────────────────
            builder.Services.AddCors(opts =>
                opts.AddDefaultPolicy(p =>
                    p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

            // ═══════════════════════════════════════════════════════════════
            var app = builder.Build();

            // ─── Middleware Pipeline (order matters) ─────────────────────
            app.UseMiddleware<CorrelationIdMiddleware>();
            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
            app.UseSerilogRequestLogging();
            app.UseCors();
            app.UseIpRateLimiting();
            app.UseRateLimiter();
            app.UseResponseCaching();
            app.UseOutputCache();
            app.UseAuthentication();
            app.UseAuthorization();

            // ─── Dev-only token endpoint ────────────────────────────────
            if (app.Environment.IsDevelopment())
            {
                app.MapPost("/auth/token", (IConfiguration config) =>
                {
                    var jwtConfig = config.GetSection("Jwt");
                    var signingKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtConfig["Key"]!));
                    var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, "TestUser"),
                        new Claim(ClaimTypes.Role, "Admin"),
                        new Claim(ClaimTypes.Role, "Reviewer")
                    };
                    var token = new JwtSecurityToken(
                        issuer: jwtConfig["Issuer"],
                        audience: jwtConfig["Audience"],
                        claims: claims,
                        expires: DateTime.UtcNow.AddHours(1),
                        signingCredentials: creds);
                    return Results.Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
                }).AllowAnonymous().WithTags("Auth");
            }

            // ─── Health Check Endpoints ─────────────────────────────────
            app.MapHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = WriteHealthResponse
            });
            app.MapHealthChecks("/health/live", new HealthCheckOptions
            {
                Predicate = hc => hc.Tags.Contains("live"),
                ResponseWriter = WriteHealthResponse
            });
            app.MapHealthChecks("/health/ready", new HealthCheckOptions
            {
                Predicate = hc => hc.Tags.Contains("ready"),
                ResponseWriter = WriteHealthResponse
            });

            // ─── Gateway Info Endpoint ──────────────────────────────────
            app.MapGet("/gateway/info", () => Results.Ok(new
            {
                service = "LET API Gateway",
                version = "1.0.0",
                framework = "YARP + Ocelot Config",
                timestamp = DateTimeOffset.UtcNow,
                routes = new
                {
                    leave           = "/api/leave/{**}        → localhost:5166",
                    courses         = "/api/courses/{**}      → localhost:5215",
                    requests        = "/api/requests/{**}     → localhost:5006",
                    reviews         = "/api/reviews/{**}      → localhost:5114",
                    development     = "/api/development/{**}  → localhost:5216",
                    master          = "/api/master/{**}       → localhost:5279",
                    financialYears  = "/api/financial-years/{**} → localhost:5279",
                    letTransaction  = "/api/let/{**}          → localhost:5320",
                    letGraphql      = "/graphql/let/{**}      → localhost:5320"
                }
            })).AllowAnonymous().WithTags("Gateway");

            // ─── YARP Reverse Proxy ─────────────────────────────────────
            app.MapReverseProxy();

            Log.Information("LET API Gateway started on {Urls}", string.Join(", ",
                app.Urls.Any() ? app.Urls : ["http://localhost:5400"]));

            await app.RunAsync();
        }
        catch (Exception ex) when (ex is not HostAbortedException)
        {
            Log.Fatal(ex, "API Gateway terminated unexpectedly");
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }

    private static async Task WriteHealthResponse(HttpContext ctx, HealthReport report)
    {
        ctx.Response.ContentType = "application/json";

        var entries = report.Entries.Select(e => new
        {
            name = e.Key,
            status = e.Value.Status.ToString(),
            description = e.Value.Description,
            duration = e.Value.Duration.ToString(),
            data = e.Value.Data.Count > 0 ? e.Value.Data : null
        });

        var result = new
        {
            status = report.Status.ToString(),
            totalDuration = report.TotalDuration.ToString(),
            entries
        };

        await ctx.Response.WriteAsJsonAsync(result);
    }
}
