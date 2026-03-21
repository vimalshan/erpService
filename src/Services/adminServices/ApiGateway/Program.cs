using ApiGateway.Configuration;
using ApiGateway.Handlers;
using ApiGateway.HealthChecks;
using ApiGateway.Middleware;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File(
        path: "logs/gateway-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] {Message:lj}{NewLine}{Exception}",
        retainedFileCountLimit: 30)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

try
{
    Log.Information("Starting ERP API Gateway application...");

    // ==================== Configuration ====================
    var gatewayConfig = ServiceConfigurationSetup.GetServiceConfigurations(builder.Configuration);
    builder.Services.AddSingleton(gatewayConfig);

    // ==================== Add Services ====================
    
    // Add controllers
    builder.Services.AddControllers();

    // Add memory cache for response caching
    builder.Services.AddMemoryCache();

    // Add rate limiting
    builder.Services.AddOptions();
    builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimitPolicy"));
    builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));
    builder.Services.AddInMemoryRateLimiting();
    builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

    // Add health checks
    HealthCheckConfiguration.AddGatewayHealthChecks(builder.Services, builder.Configuration);

    // Add JWT authentication
    builder.Services.ConfigureJwtAuthentication(builder.Configuration);

    // Add authorization policies
    builder.Services.ConfigureAuthorizationPolicies();

    // Configure HTTP clients for all services with resilience policies
    builder.Services.ConfigureHttpClients(gatewayConfig);

    // Add Swagger/OpenAPI
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "ADMIN API Gateway",
            Version = "v1.0",
            Description = "Central API Gateway for ADMIN Microservices"
        });
    });

    // Add CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });

    // ==================== Build Application ====================
    var app = builder.Build();

    Log.Information("Configuring middleware pipeline...");

    // Custom middleware
    app.UseMiddleware<RequestResponseLoggingMiddleware>();
    app.UseMiddleware<SecurityHeadersMiddleware>();
    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseMiddleware<RequestValidationMiddleware>();

    // Rate limiting
    app.UseIpRateLimiting();

    // HTTPS redirection (in production)
    if (app.Environment.IsProduction())
    {
        app.UseHttpsRedirection();
        app.UseHsts();
    }

    // Swagger/OpenAPI
    Log.Information("Enabling Swagger UI");
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ERP API Gateway v1");
        options.RoutePrefix = "swagger";
    });

    // CORS
    app.UseCors("AllowAll");

    // Authentication & Authorization
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    // Health checks
    Log.Information("Configuring health check endpoints...");
    app.MapHealthChecks("/health", new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = async (context, report) =>
        {
            context.Response.ContentType = "application/json";
            var result = new
            {
                status = report.Status.ToString(),
                timestamp = DateTime.UtcNow
            };
            await context.Response.WriteAsJsonAsync(result);
        }
    });

    // API endpoints
    app.MapControllers();

    // Default route
    app.MapGet("/", async (HttpContext context) =>
    {
        context.Response.ContentType = "application/json";
        var result = new
        {
            message = "ERP API Gateway",
            version = "1.0.0",
            status = "running",
            timestamp = DateTime.UtcNow
        };
        await context.Response.WriteAsJsonAsync(result);
    }).WithName("GatewayInfo");

    // ==================== Run Application ====================
    Log.Information("ERP API Gateway is starting on port 5000");
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
