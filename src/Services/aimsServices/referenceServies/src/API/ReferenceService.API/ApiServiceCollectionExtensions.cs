using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ReferenceService.API.Auth;
using ReferenceService.API.Middleware;
using ReferenceService.API.HealthChecks;
using HotChocolate.Execution.Configuration;
using ReferenceService.API.GraphQL;

namespace ReferenceService.API;

/// <summary>
/// Extension methods for configuring API services and middleware.
/// </summary>
public static class ApiServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, JwtConfiguration jwtConfig)
    {
        // Register JWT service
        services.AddSingleton(jwtConfig);
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        
        // Configure JWT Authentication
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey));
                
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateIssuer = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtConfig.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
        
        services.AddAuthorization();
        
        // Add Controllers
        services.AddControllers();
        
        // Add Swagger/OpenAPI
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Reference Service API",
                Version = "v1",
                Description = "API for managing reference data (LOV, Permissions, etc.)"
            });
            
            var securityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "JWT Authentication",
                Description = "Enter JWT Bearer token",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            };
            
            c.AddSecurityDefinition("Bearer", securityScheme);
            c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
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
                    new string[] { }
                }
            });
        });
        
        // Add GraphQL
        services
            .AddGraphQLServer()
            .AddGraphQLConfiguration();
        
        // Add Health Checks
        services.AddHealthChecks()
            .AddCheck<DatabaseHealthCheck>("database")
            .AddCheck<ApiReadinessHealthCheck>("readiness");
        
        return services;
    }
    
    public static WebApplication UseApiMiddleware(this WebApplication app)
    {
        // Exception handling
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        
        // Request/Response logging
        app.UseMiddleware<RequestResponseLoggingMiddleware>();
        
        // Swagger - Always enable for testing
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Reference Service API V1");
            c.RoutePrefix = "swagger";
        });
        
        // HTTPS redirection
        app.UseHttpsRedirection();
        
        // Authentication & Authorization
        app.UseAuthentication();
        app.UseAuthorization();
        
        // Map Controllers
        app.MapControllers();
        
        // Map GraphQL
        app.MapGraphQL("/graphql");
        
        // Health checks
        app.MapHealthChecks("/health");
        app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions { Predicate = r => r.Name == "readiness" });
        app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions { Predicate = r => r.Name == "liveness" });
        
        return app;
    }
}
