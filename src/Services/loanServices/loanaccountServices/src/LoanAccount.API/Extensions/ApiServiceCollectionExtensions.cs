using System.Text;
using MediatR;
using FluentValidation;
using Serilog;
using LoanAccount.Application.Mapping;
using LoanAccount.Application.Services;
using LoanAccount.Infrastructure.Extensions;
using LoanAccount.Infrastructure.Persistence;
using LoanAccount.Infrastructure.Seed;
using LoanAccount.API.Middleware;
using LoanAccount.API.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using HotChocolate.Execution.Configuration;

namespace LoanAccount.API.Extensions;

/// <summary>
/// Extension methods for configuring API services
/// </summary>
public static class ApiServiceCollectionExtensions
{
    /// <summary>
    /// Registers all API services
    /// </summary>
    public static IServiceCollection AddApiServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Add infrastructure services
        services.AddInfrastructureServices(configuration);

        // Add MediatR for CQRS
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LoanApplicationService).Assembly));

        // Add AutoMapper - using explicit registration to work around version conflicts
        services.AddSingleton<AutoMapper.IConfigurationProvider>(sp =>
        {
            var config = new AutoMapper.MapperConfiguration(cfg => 
            {
                cfg.AddProfile<LoanMappingProfile>();
            });
            return config;
        });
        services.AddScoped<AutoMapper.IMapper>(sp => 
            new AutoMapper.Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>()));

        // Add FluentValidation
        services.AddValidatorsFromAssemblyContaining<LoanMappingProfile>();

        // Add Application Services
        services.AddScoped<LoanApplicationService>();

        // Add JWT Token Service
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        // Configure JWT Authentication
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings.GetValue<string>("SecretKey");
        var issuer = jwtSettings.GetValue<string>("Issuer");
        var audience = jwtSettings.GetValue<string>("Audience");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)),
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    context.NoResult();
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsJsonAsync(new { message = "Authentication failed" });
                },
                OnChallenge = context =>
                {
                    context.HandleResponse();
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsJsonAsync(new { message = "Authorization required" });
                }
            };
        });

        // Add Authorization
        services.AddAuthorization(options =>
        {
            options.AddPolicy("LoanManager", policy =>
                policy.RequireRole("Admin", "LoanManager"));
            options.AddPolicy("LoanViewer", policy =>
                policy.RequireRole("Admin", "LoanManager", "User"));
        });

        // Add Controllers
        services.AddControllers();

        // Add Swagger/OpenAPI
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Loan Account Service API",
                Version = "v1",
                Description = "Enterprise loan account management microservice with comprehensive features"
            });

            // Add JWT authentication to Swagger
            options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "Enter JWT token"
            });

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
        });

        // Add GraphQL
        services
            .AddGraphQLServer()
            .AddQueryType<LoanAccount.API.GraphQL.Queries.LoanQuery>()
            .AddMutationType<LoanAccount.API.GraphQL.Mutations.LoanMutation>()
            .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);

        // Add CORS
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        // Add Health Checks
        services.AddHealthChecks();

        return services;
    }

    /// <summary>
    /// Configures the HTTP request pipeline
    /// </summary>
    public static WebApplication ConfigureApiPipeline(this WebApplication app)
    {
        // Use Serilog request logging
        app.UseSerilogRequestLogging();

        // Swagger in development
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Loan Account Service API v1");
                options.RoutePrefix = "swagger";
            });
        }

        // Use CORS
        app.UseCors("AllowAll");

        // Use global exception middleware
        app.UseMiddleware<GlobalExceptionMiddleware>();

        // Use Authentication and Authorization
        app.UseAuthentication();
        app.UseAuthorization();

        // Map Controllers
        app.MapControllers();

        // Map GraphQL endpoint
        app.MapGraphQL("/graphql");

        // Map Health Checks
        app.MapHealthChecks("/health");

        return app;
    }

    /// <summary>
    /// Seeds the database with initial data
    /// </summary>
    public static async Task SeedDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<LoanAccountDbContext>();

        try
        {
            // Create database and schema if it doesn't exist
            await context.Database.EnsureCreatedAsync();
            
            // Seed data (seed method checks if data already exists)
            await LoanAccountDbContextSeed.SeedAsync(context);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database seeding error: {ex.Message}");
            throw;
        }
    }
}
