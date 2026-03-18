using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using TimeAttendance.Domain.Interfaces;
using TimeAttendance.Infrastructure.Messaging;
using TimeAttendance.Infrastructure.Persistence;
using TimeAttendance.Infrastructure.Repositories;
using TimeAttendance.Infrastructure.Repositories.Dapper;
using TimeAttendance.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;

namespace TimeAttendance.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core - SQL Server
        services.AddDbContext<TimeAttendanceDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null);
                    sqlOptions.CommandTimeout(30);
                    sqlOptions.MigrationsAssembly(typeof(TimeAttendanceDbContext).Assembly.FullName);
                }));

        // Repositories (EF)
        services.AddScoped<IAbsenteeismDetailRepository, AbsenteeismDetailRepository>();
        services.AddScoped<IAbsenteeismMisRepository, AbsenteeismMisRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Dapper read repository
        services.AddSingleton(sp =>
            new AbsenteeismDapperRepository(
                configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not configured.")));

        // RabbitMQ
        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.SectionName));
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
        services.AddSingleton<IMessageHandler, AbsenteeismCreatedMessageHandler>();
        services.AddHostedService<RabbitMqConsumerService>();

        // Blob Storage
        services.Configure<BlobStorageOptions>(configuration.GetSection(BlobStorageOptions.SectionName));
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        // JWT Authentication
        var jwtSection = configuration.GetSection("Jwt");
        var secretKey = jwtSection["Secret"]
            ?? throw new InvalidOperationException("JWT Secret not configured.");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSection["Issuer"],
                    ValidAudience = jwtSection["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("ReadPolicy", policy =>
                policy.RequireClaim(ClaimTypes.Role, "Admin", "Reader", "Manager"));
            options.AddPolicy("WritePolicy", policy =>
                policy.RequireClaim(ClaimTypes.Role, "Admin", "Manager"));
        });

        return services;
    }
}
