namespace CommunityService.API.Extensions;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using MediatR;
using AutoMapper;
using FluentValidation;
using CommunityService.Application.Commands;
using CommunityService.Application.Mappings;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.Messaging;
using Application.Behaviors;
using Middleware;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string not found");
        
        services.AddDbContext<CommunityDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Repositories
        services.AddScoped<ICommunityRepository, CommunityRepository>();
        services.AddScoped<ICommunityMemberRepository, CommunityMemberRepository>();

        // MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreateCommunityCommand).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        });

        // AutoMapper
        services.AddAutoMapper(typeof(CommunityMappingProfile));

        // Blob Storage
        services.AddScoped<IBlobStorageService>(sp =>
            new AzureBlobStorageService(
                configuration.GetConnectionString("AzureBlobStorage") ?? "DefaultEndpointsProtocol=https;",
                "community-assets"));

        // RabbitMQ
        var rabbitMqHost = configuration["RabbitMq:HostName"] ?? "localhost";
        services.AddScoped<IMessagePublisher>(sp =>
            new RabbitMqPublisher(rabbitMqHost));
        services.AddScoped<IMessageConsumer>(sp =>
            new CommunityEventConsumer(
                rabbitMqHost,
                sp.GetRequiredService<ILogger<CommunityEventConsumer>>()));

        // Health Checks
        services.AddHealthChecks()
            .AddCheck<DatabaseHealthCheck>("Database")
            .AddCheck("RabbitMQ", new RabbitMqHealthCheck(rabbitMqHost, 
                new Logger<RabbitMqHealthCheck>(new LoggerFactory())));

        return services;
    }

    public static IServiceCollection AddAuthenticationAndAuthorization(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt");
        var signingKey = jwtSettings["SigningKey"] ?? throw new InvalidOperationException("JWT Signing Key not configured");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(signingKey)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = jwtSettings["Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPolicy", policy =>
                policy.RequireClaim("role", "Admin"));
            options.AddPolicy("ModeratorPolicy", policy =>
                policy.RequireClaim("role", "Admin", "Moderator"));
        });

        return services;
    }

    public static IServiceCollection AddApiServices(
        this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new() 
            { 
                Title = "Community Service API", 
                Version = "v1",
                Description = "Community management microservice" 
            });

            var securityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter the Bearer token",
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
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}

public static class WebApplicationExtensions
{
    public static WebApplication UseCustomMiddleware(this WebApplication app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseMiddleware<AuthenticationMiddleware>();

        return app;
    }

    public static WebApplication UseSwaggerAndHealthChecks(this WebApplication app)
    {
        // Enable Swagger for all environments during development
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Community Service API v1");
        });

        app.MapHealthChecks("/health");
        app.MapHealthChecks("/health/ready");
        app.MapHealthChecks("/health/live");

        return app;
    }
}
