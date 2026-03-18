using Azure.Storage.Blobs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RiskService.Application.DTOs;
using RiskService.Application.Interfaces;
using RiskService.Application.Queries.RiskType;
using RiskService.Domain.Interfaces;
using RiskService.Infrastructure.Messaging;
using RiskService.Infrastructure.Persistence;
using RiskService.Infrastructure.Repositories;
using RiskService.Infrastructure.Services;

namespace RiskService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;

        // EF Core
        services.AddDbContext<RiskDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Repositories
        services.AddScoped<IRiskRepository, RiskRepository>();
        services.AddScoped<IMitigationRepository, MitigationRepository>();
        services.AddScoped<ISelfAssessmentRepository, SelfAssessmentRepository>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<RiskDbContext>());

        // Dapper lookups
        services.AddScoped<IRequestHandler<GetAllRiskTypesQuery, IReadOnlyList<RiskTypeDto>>>(
            _ => new DapperLookupQueryHandler(connectionString));
        services.AddScoped<IRequestHandler<GetAllRiskImpactsQuery, IReadOnlyList<RiskImpactDto>>>(
            _ => new DapperLookupQueryHandler(connectionString));
        services.AddScoped<IRequestHandler<GetAllRiskProbabilitiesQuery, IReadOnlyList<RiskProbabilityDto>>>(
            _ => new DapperLookupQueryHandler(connectionString));
        services.AddScoped<IRequestHandler<GetAllRiskRatingsQuery, IReadOnlyList<RiskRatingDto>>>(
            _ => new DapperLookupQueryHandler(connectionString));
        services.AddScoped<IRequestHandler<GetAllRiskResponsesQuery, IReadOnlyList<RiskResponseDto>>>(
            _ => new DapperLookupQueryHandler(connectionString));

        // Blob Storage
        var blobConnectionString = configuration.GetValue<string>("AzureBlobStorage:ConnectionString");
        if (!string.IsNullOrEmpty(blobConnectionString))
        {
            services.AddSingleton(new BlobServiceClient(blobConnectionString));
            services.AddScoped<IBlobStorageService, BlobStorageService>();
        }

        // RabbitMQ Publisher
        var rabbitHost = configuration.GetValue<string>("RabbitMQ:HostName") ?? "localhost";
        var rabbitUser = configuration.GetValue<string>("RabbitMQ:UserName") ?? "guest";
        var rabbitPass = configuration.GetValue<string>("RabbitMQ:Password") ?? "guest";

        services.AddSingleton<IMessagePublisher>(sp =>
        {
            try
            {
                return RabbitMqPublisher.CreateAsync(rabbitHost, rabbitUser, rabbitPass).GetAwaiter().GetResult();
            }
            catch
            {
                var logger = sp.GetRequiredService<Microsoft.Extensions.Logging.ILoggerFactory>()
                    .CreateLogger("RabbitMQ");
                logger.LogWarning("RabbitMQ is unavailable. Messages will not be published.");
                return new NoOpMessagePublisher();
            }
        });

        // RabbitMQ Consumer (only register if host is configured)
        services.AddHostedService(sp =>
            new RiskEventConsumer(sp, sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<RiskEventConsumer>>(),
                rabbitHost, rabbitUser, rabbitPass));

        return services;
    }
}

/// <summary>Fallback publisher when RabbitMQ is unavailable.</summary>
internal class NoOpMessagePublisher : IMessagePublisher
{
    public Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken ct = default)
        => Task.CompletedTask;
}
