using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using UserService.API.GraphQL;
using UserService.API.Middleware;
using UserService.Application.Behaviors;
using UserService.Application.Commands;
using UserService.Application.Commands.Handlers;
using UserService.Domain.Repositories;
using UserService.Infrastructure.Data;
using UserService.Infrastructure.Messaging;
using UserService.Infrastructure.Persistence;
using UserService.Infrastructure.Services;

namespace UserService.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<UserServiceDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Unit of Work and Repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreateUserCommand).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
        });

        // Authentication
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
        var issuer = jwtSettings["Issuer"] ?? "UserService";
        var audience = jwtSettings["Audience"] ?? "UserServiceAPI";
        var expirationMinutes = int.Parse(jwtSettings["ExpirationMinutes"] ?? "60");

        services.AddSingleton<ITokenService>(new JwtTokenService(secretKey, issuer, audience, expirationMinutes));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(secretKey)),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true
                };
            });

        services.AddAuthorization();

        // Health Checks
        services.AddHealthChecks();

        // GraphQL (HotChocolate)
        services
            .AddGraphQLServer()
            .AddQueryType<UserQuery>()
            .AddMutationType<UserMutation>();

        // Services
        services.AddScoped<HealthCheckService>();

        // Swagger
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "User Service API",
                Version = "v1",
                Description = "User Management Microservice API"
            });

            c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme",
                Name = "Authorization",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

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
                    Array.Empty<string>()
                }
            });
        });

        // Logging
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("logs/userservice-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        services.AddLogging(loggingBuilder =>
            loggingBuilder.AddSerilog());

        return services;
    }

    public static void AddRabbitMqServices(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqSettings = configuration.GetSection("RabbitMQ");
        var hostName = rabbitMqSettings["HostName"] ?? "localhost";
        var port = int.Parse(rabbitMqSettings["Port"] ?? "5672");
        var userName = rabbitMqSettings["UserName"] ?? "guest";
        var password = rabbitMqSettings["Password"] ?? "guest";

        services.AddSingleton(sp => new RabbitMqPublisher(hostName, port, userName, password));
        services.AddSingleton(sp => new UserDomainEventConsumer(hostName, port, userName, password));
    }
}

/// <summary>
/// Application builder extensions
/// </summary>
public static class ApplicationBuilderExtensions
{
    public static async Task UseApplicationPipeline(this WebApplication app)
    {
        // Always enable Swagger for easier testing and development
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Service API v1");
        });

        // Exception handling before other middleware
        app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
        
        app.UseHttpsRedirection();
        app.UseMiddleware<AuthenticationMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.MapGraphQL();          // GraphQL endpoint: /graphql  (playground: /graphql/ui)

        app.MapHealthChecks("/health");

        // Apply migrations and seed data
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<UserServiceDbContext>();
            var logger    = scope.ServiceProvider.GetRequiredService<ILogger<UserServiceDbContext>>();
            dbContext.Database.Migrate();
            await DatabaseSeeder.SeedAsync(dbContext, logger);
        }
    }
}
