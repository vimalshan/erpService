using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using TrustService.API.GraphQL;
using TrustService.API.Middleware;
using TrustService.Application;
using TrustService.Application.Common.Interfaces;
using TrustService.Application.DTOs;
using TrustService.Application.Features.Trusts.Queries;
using TrustService.Infrastructure;
using TrustService.Infrastructure.Persistence;
using TrustService.Infrastructure.Persistence.Seed;
using MediatR;

// Serilog bootstrap
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Serilog
    builder.Host.UseSerilog((context, loggerConfig) =>
        loggerConfig.ReadFrom.Configuration(context.Configuration));

    // Application & Infrastructure layers
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    // Controllers
    builder.Services.AddControllers();

    // Swagger / OpenAPI
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "TrustService API",
            Version = "v1",
            Description = "Trust Management Microservice API"
        });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(_ => new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecuritySchemeReference("Bearer"),
                new List<string>()
            }
        });
    });

    // JWT Authentication
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    var secretKey = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret not configured");

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
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

    builder.Services.AddAuthorization();

    // GraphQL (Hot Chocolate)
    builder.Services
        .AddGraphQLServer()
        .AddQueryType<Query>()
        .AddMutationType<Mutation>()
        .AddFiltering()
        .AddSorting();

    // Health Checks
    builder.Services.AddHealthChecks()
        .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!,
            name: "sqlserver");

    // CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    });

    var app = builder.Build();

    // Exception handling middleware
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    // Swagger
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TrustService API v1"));
    }

    app.UseHttpsRedirection();
    app.UseCors("AllowAll");

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    // GraphQL endpoint
    app.MapGraphQL("/graphql");

    // Health check endpoints
    app.MapHealthChecks("/health");

    // --- Minimal API endpoints ---
    var trustGroup = app.MapGroup("/api/minimal/trusts")
        .RequireAuthorization()
        .WithTags("Trusts (Minimal API)");

    trustGroup.MapGet("/", async (ISender mediator, CancellationToken ct) =>
    {
        var result = await mediator.Send(new GetAllTrustsQuery(), ct);
        return Results.Ok(result);
    }).WithName("GetAllTrustsMinimal");

    trustGroup.MapGet("/active", async (ISender mediator, CancellationToken ct) =>
    {
        var result = await mediator.Send(new GetActiveTrustsQuery(), ct);
        return Results.Ok(result);
    }).WithName("GetActiveTrustsMinimal");

    trustGroup.MapGet("/{trustCode}", async (string trustCode, ISender mediator, CancellationToken ct) =>
    {
        var result = await mediator.Send(new GetTrustByCodeQuery(trustCode), ct);
        return result is null ? Results.NotFound() : Results.Ok(result);
    }).WithName("GetTrustByCodeMinimal");

    trustGroup.MapGet("/dapper", async (string? statusFilter, ISender mediator, CancellationToken ct) =>
    {
        var result = await mediator.Send(new GetTrustsByDapperQuery(statusFilter), ct);
        return Results.Ok(result);
    }).WithName("GetTrustsByDapperMinimal");

    // Seed database
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<TrustDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        await context.Database.MigrateAsync();
        await TrustDbContextSeed.SeedAsync(context, logger);
    }

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

// Make Program class accessible for integration tests
public partial class Program { }
