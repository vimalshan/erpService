using Microsoft.EntityFrameworkCore;
using Serilog;
using MediatR;
using FluentValidation;
using AutoMapper;
using CheckupManagementService.Infrastructure.Persistence;
using CheckupManagementService.Infrastructure.Extensions;
using CheckupManagementService.Infrastructure.Repositories;
using CheckupManagementService.Application.Commands;
using CheckupManagementService.Infrastructure.EventBus;
using CheckupManagementService.Infrastructure.EventPublishing;
using Shared.Infrastructure.Middleware;
using Shared.Infrastructure.Logging;
using Shared.Infrastructure.Caching;
using Shared.Infrastructure.Authentication;
using Shared.Core.Repositories;
using Shared.Events;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var environment = builder.Environment;

// Serilog Configuration
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/checkup-service-.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("ServiceName", "CheckupManagementService")
    .CreateLogger();

builder.Host.UseSerilog();

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? new[] { "http://localhost:3000" };
        builder.WithOrigins(allowedOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// HttpContext for User Context
builder.Services.AddHttpContextAccessor();

// Database Configuration
builder.Services.AddDbContext<CheckupManagementDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("HealthDb"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(maxRetryCount: 3)));

// CQRS - MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Controllers
builder.Services.AddControllers();

// Validation
// builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
// builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Shared.Infrastructure.Validation.ValidationBehavior<,>));

// User Context
builder.Services.AddScoped<IUserContext, UserContext>();

// AutoMapper
builder.Services.AddAutoMapperServices();

// GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType<CheckupManagementService.GraphQL.CheckupQuery>()
    .AddMutationType<CheckupManagementService.GraphQL.CheckupMutation>()
    .AddType<CheckupManagementService.GraphQL.CheckupMasterDtoType>()
    .AddType<CheckupManagementService.GraphQL.HealthMainDtoType>()
    .AddType<CheckupManagementService.GraphQL.TestMasterDtoType>()
    .AddType<CheckupManagementService.GraphQL.ErrorInfoType>();

// Event Bus (RabbitMQ)
builder.Services.AddScoped<IEventBus, RabbitMQEventBus>();
builder.Services.Configure<RabbitMQOptions>(configuration.GetSection("RabbitMQ"));

// Event Publisher
builder.Services.AddScoped<IEventPublisher, DomainEventPublisher>();

// Repository Pattern
builder.Services.AddScoped(typeof(IRepository<,>), typeof(GenericRepository<,>));

// Polly Resilience
builder.Services.AddHttpClient();

// RabbitMQ Consumers as Hosted Services
builder.Services.AddHostedService<CheckupManagementService.Infrastructure.EventBus.CheckupCreatedEventConsumer>();
builder.Services.AddHostedService<CheckupManagementService.Infrastructure.EventBus.HealthExaminationEventConsumer>();
builder.Services.AddHostedService<CheckupManagementService.Infrastructure.EventBus.CheckupApprovalEventConsumer>();

// Health Checks
builder.Services.AddHealthChecks()
    .AddCheck<CheckupManagementService.Infrastructure.HealthChecks.DatabaseHealthCheck>("Database")
    .AddCheck<CheckupManagementService.Infrastructure.HealthChecks.RabbitMQHealthCheck>("RabbitMQ")
    .AddCheck<CheckupManagementService.Infrastructure.HealthChecks.RedisHealthCheck>("Redis")
    .AddCheck<CheckupManagementService.Infrastructure.HealthChecks.ApiHealthCheck>("API");

// Application Insights
builder.Services.AddApplicationInsightsTelemetry(configuration["Azure:ApplicationInsightsInstrumentationKey"]);

// Redis Caching
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = configuration["Redis:Configuration"] ?? "localhost:6379";
});
builder.Services.AddScoped<ICacheService, RedisCacheService>();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Checkup Management Service API",
        Version = "v1",
        Description = "API for managing health checkups and medical examinations",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Support Team",
            Email = "support@healthservice.com"
        },
        License = new Microsoft.OpenApi.Models.OpenApiLicense
        {
            Name = "MIT"
        }
    });

    // Use only the first description when there are multiple matches for same action
    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

    // Add security scheme definition for JWT Bearer token
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\""
    });

    // Add security requirements for all endpoints
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Include all controllers and their methods
    options.DocInclusionPredicate((docName, apiDesc) =>
    {
        return true; // Include all endpoints
    });
});

// JWT Authentication
builder.Services.AddJwtAuthentication(configuration);

var app = builder.Build();

// Enable Swagger UI early, before exception handling middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Checkup Management Service API v1");
        c.RoutePrefix = "swagger";
    });
}

// Middleware
app.UseCors("AllowAll");
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGraphQL("/graphql");

// Health Checks
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var response = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description,
                duration = e.Value.Duration.TotalMilliseconds
            }),
            timestamp = DateTime.UtcNow
        };
        await context.Response.WriteAsJsonAsync(response);
    }
});

// Basic health endpoint
app.MapGet("/health/live", () => new { status = "alive", service = "CheckupManagementService", timestamp = DateTime.UtcNow })
    .WithName("Liveness Check")
    .WithOpenApi();

app.MapGet("/health/ready", () => new { status = "ready", service = "CheckupManagementService", timestamp = DateTime.UtcNow })
    .WithName("Readiness Check")
    .WithOpenApi();

try
{
    Log.Information("Checkup Management Service starting...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Checkup Management Service terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
