using System.Text;
using ApiGateway.HealthChecks;
using ApiGateway.Middleware;
using ApiGateway.Resilience;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using Scalar.AspNetCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // --- Serilog ---
    builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

    // --- Load ocelot.json ---
    builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

    // --- JWT Authentication ---
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    var secretKey = jwtSettings["SecretKey"]!;

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
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });
    builder.Services.AddAuthorization();

    // --- Rate Limiting (AspNetCoreRateLimit) ---
    builder.Services.AddMemoryCache();
    builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
    builder.Services.Configure<ClientRateLimitOptions>(builder.Configuration.GetSection("ClientRateLimiting"));
    builder.Services.AddInMemoryRateLimiting();
    builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

    // --- Response Caching ---
    builder.Services.AddResponseCaching();
    builder.Services.AddOutputCache(options =>
    {
        var cacheDuration = builder.Configuration.GetValue("ResponseCaching:DefaultDurationSeconds", 30);
        options.AddBasePolicy(p => p.Expire(TimeSpan.FromSeconds(cacheDuration)));
        options.AddPolicy("NoCache", p => p.NoCache());
    });

    // --- YARP Reverse Proxy ---
    builder.Services.AddReverseProxy()
        .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

    // --- Ocelot ---
    builder.Services.AddOcelot(builder.Configuration)
        .AddPolly();

    // --- Resilience (Circuit Breaker, Retry, Timeout, Bulkhead) ---
    builder.Services.AddResiliencePolicies(builder.Configuration);
    builder.Services.AddSingleton(sp =>
    {
        var maxParallel = builder.Configuration.GetValue("BulkheadPolicy:MaxParallelization", 50);
        return new BulkheadManager(maxParallel, sp.GetRequiredService<ILogger<BulkheadManager>>());
    });

    // --- Health Checks ---
    builder.Services.AddGatewayHealthChecks(builder.Configuration);

    // --- CORS ---
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("X-Correlation-ID", "X-RateLimit-Limit", "X-RateLimit-Remaining"));
    });

    // --- OpenAPI ---
    builder.Services.AddOpenApi();

    var app = builder.Build();

    // --- Middleware Pipeline (order matters) ---
    app.UseSerilogRequestLogging();
    app.UseCors();

    // 1. Correlation ID (first — everything after has the ID)
    app.UseMiddleware<CorrelationIdMiddleware>();

    // 2. Request/Response Logging
    app.UseMiddleware<RequestResponseLoggingMiddleware>();

    // 3. Rate Limiting
    app.UseIpRateLimiting();

    // 4. Response Caching
    app.UseResponseCaching();
    app.UseOutputCache();

    // 5. Authentication & Authorization
    app.UseAuthentication();
    app.UseAuthorization();

    // --- Health Checks ---
    app.MapGatewayHealthChecks();

    // --- OpenAPI + Scalar ---
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("SRF Sparsh API Gateway")
               .WithTheme(ScalarTheme.Moon);
    });

    // --- Gateway Info Endpoints ---
    app.MapGet("/", () => Results.Ok(new
    {
        service = "SRF Sparsh API Gateway",
        version = "1.0.0",
        status = "running",
        timestamp = DateTime.UtcNow,
        endpoints = new
        {
            health = "/health",
            healthGateway = "/health/gateway",
            healthServices = "/health/services",
            scalar = "/scalar/v1",
            ocelot = "/ocelot/{service}/{path}",
            yarp = "/api/{service}/{path}",
            services = new
            {
                approval = "/api/approval/{path}",
                booking = "/api/booking/{path}",
                community = "/api/community/{path}",
                compensation = "/api/compensation/{path}",
                groupmanagement = "/api/groupmanagement/{path}",
                location = "/api/location/{path}",
                meeting = "/api/meeting/{path}",
                proxy = "/api/proxy/{path}",
                reimbursement = "/api/reimbursement/{path}",
                stipend = "/api/stipend/{path}",
                timesheet = "/api/timesheet/{path}",
                transaction = "/api/transaction/{path}",
                usermanagement = "/api/usermanagement/{path}",
                websitecontent = "/api/websitecontent/{path}"
            }
        }
    }));

    app.MapGet("/gateway/status", (ResiliencePolicyFactory factory, BulkheadManager bulkhead) => Results.Ok(new
    {
        gateway = "running",
        timestamp = DateTime.UtcNow,
        bulkheads = bulkhead.GetStatus(),
        services = app.Configuration.GetSection("ServiceDiscovery").GetChildren()
                      .ToDictionary(c => c.Key, c => c.Value)
    }));

    // --- Auth endpoint on the gateway itself ---
    app.MapPost("/gateway/auth/token", (LoginRequest request, IConfiguration config) =>
    {
        // Demo token generator — same logic as downstream services
        var jwt = config.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["SecretKey"]!));
        var creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);
        var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: [
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, request.Username),
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, "Admin"),
                new System.Security.Claims.Claim("sub", request.Username)
            ],
            expires: DateTime.UtcNow.AddMinutes(config.GetValue("JwtSettings:ExpirationMinutes", 60)),
            signingCredentials: creds);

        var tokenString = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
        return Results.Ok(new { token = tokenString, expiresIn = config.GetValue("JwtSettings:ExpirationMinutes", 60) });
    }).AllowAnonymous();

    // --- YARP Reverse Proxy ---
    app.MapReverseProxy();

    // --- Ocelot Pipeline (handles /ocelot/* routes only) ---
    app.MapWhen(
        context => context.Request.Path.StartsWithSegments("/ocelot"),
        ocelotApp => ocelotApp.UseOcelot().Wait());

    Log.Information("SRF Sparsh API Gateway started on port 5100");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "API Gateway terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

public record LoginRequest(string Username, string Password);
