using System.Text;
using InvoiceProcessing.Domain.Interfaces;
using InvoiceProcessing.Infrastructure.Auth;
using InvoiceProcessing.Infrastructure.Messaging;
using InvoiceProcessing.Infrastructure.Persistence;
using InvoiceProcessing.Infrastructure.Persistence.Dapper;
using InvoiceProcessing.Infrastructure.Persistence.Repositories;
using InvoiceProcessing.Infrastructure.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace InvoiceProcessing.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<InvoiceProcessingDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(InvoiceProcessingDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDocumentRepository, DocumentRepository>();

        // Dapper
        services.AddScoped<IDapperQueryService, DapperQueryService>();

        // RabbitMQ
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
        services.AddHostedService<InvoiceMessageConsumer>();

        // Azure Blob Storage
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        // JWT Auth
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        var jwtSettings = configuration.GetSection("Jwt");
        var key = jwtSettings["Key"];
        if (!string.IsNullOrEmpty(key))
        {
            services.AddAuthentication(options =>
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };
            });
        }

        services.AddAuthorization();

        // Health Checks
        services.AddHealthChecks()
            .AddDbContextCheck<InvoiceProcessingDbContext>("database");

        return services;
    }
}
