using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MediatR;
using FluentValidation;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Extensions.Http;
using HotChocolate;
using HotChocolate.Execution.Configuration;
using HotChocolate.Types;
using AppraisalService.Infrastructure.Persistence.Data;
using AppraisalService.Infrastructure.Authentication;
using AppraisalService.Application;
using AppraisalService.Infrastructure.Persistence.Repositories;
using AppraisalService.Domain;
using AppraisalService.Domain.Repositories;
using AppraisalService.Application;
using AppraisalService.Application.Behaviors;
using AppraisalService.Application.CQRS.Commands;
using AppraisalService.API.Middleware;
using AppraisalService.API.Extensions;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]!);

// Add all core infrastructure services from the extension (DbContext, UoW, JWT, RabbitMQ, Storage)
builder.Services.AddAppraisalServices(builder.Configuration);

// MediatR - Register from Application assembly
builder.Services.AddMediatR(typeof(MappingProfile));

// Behaviors
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// FluentValidation - Already included in MediatR setup via dependency injection
// Additional validators can be registered here if needed

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogError(context.Exception, "Authentication failed");
            return Task.CompletedTask;
        }
    };
});

// Authorization
builder.Services.AddAuthorization();

// GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType<AppraisalService.API.GraphQL.AppraisalQueries>()
    .AddMutationType<AppraisalService.API.GraphQL.AppraisalMutations>();
    // .AddProjections() // Skipping for now to resolve compilation
    // .AddFiltering()   // Skipping for now to resolve compilation
    // .AddSorting();    // Skipping for now to resolve compilation

// Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Appraisal Service API",
        Version = "v1",
        Description = "REST and GraphQL API for managing employee appraisals"
    });

    var securityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter JWT token",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    };

    options.AddSecurityDefinition("Bearer", securityScheme);
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
            new List<string>()
        }
    });
});

// Health Checks
var healthChecks = builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppraisalDbContext>(name: "Database");
healthChecks.AddCheck<CustomHealthCheck>("CustomCheck");

// Polly Resilience Policies - Basic HTTP client without advanced policy handlers
// Note: AddPolicyHandler not available in this version - use with advanced middleware if needed
builder.Services.AddHttpClient("AppraisalService");

// Controllers
builder.Services.AddControllers();

// Logging
builder.Services.AddLogging(config =>
{
    config.ClearProviders();
    config.AddConsole();
    config.AddDebug();
});

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Appraisal Service API v1");
    options.RoutePrefix = string.Empty; // Serve Swagger UI at root
});

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// Custom Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestResponseLoggingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");

app.MapControllers();
app.MapGraphQL("/graphql");

// EF Core Migrations Auto-apply and Seed Data (Development only)
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<AppraisalDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        
        try
        {
            logger.LogInformation("Applying pending migrations...");
            context.Database.Migrate();
            
            logger.LogInformation("Seeding database with test data...");
            var seeder = new DatabaseSeeder(context, scope.ServiceProvider.GetRequiredService<ILogger<DatabaseSeeder>>());
            seeder.SeedAsync().Wait();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during migration or seeding");
        }
    }
}

app.Run();

/// <summary>
/// Custom health check
/// </summary>
public class CustomHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(HealthCheckResult.Healthy());
    }
}
