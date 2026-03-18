using CSA.Service.Application.Interfaces;
using CSA.Service.Domain.Interfaces;
using CSA.Service.Infrastructure.Data;
using CSA.Service.Infrastructure.Messaging;
using CSA.Service.Infrastructure.Repositories;
using CSA.Service.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CSA.Service.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<CsaDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("CsaDatabase")));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<CsaDbContext>());

        // Repositories
        services.AddScoped<IControlRepository, ControlRepository>();
        services.AddScoped<ISurveyRepository, SurveyRepository>();
        services.AddScoped<ISurveyQuestionRepository, SurveyQuestionRepository>();
        services.AddScoped<ISurveyFeedbackRepository, SurveyFeedbackRepository>();
        services.AddScoped<IProcessRepository, ProcessRepository>();
        services.AddScoped<ISubProcessRepository, SubProcessRepository>();
        services.AddScoped<IUnitRepository, UnitRepository>();
        services.AddScoped<IEvidenceRepository, EvidenceRepository>();
        services.AddScoped<IUnitMapDetailRepository, UnitMapDetailRepository>();

        // Dapper
        services.AddScoped<IDapperQueryService, DapperQueryService>();

        // Blob Storage
        services.AddSingleton<IBlobStorageService, BlobStorageService>();

        // RabbitMQ Publisher
        services.AddSingleton<IMessagePublisher>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var logger = sp.GetRequiredService<ILogger<RabbitMqPublisher>>();
            return RabbitMqPublisher.CreateAsync(config, logger).GetAwaiter().GetResult();
        });

        // RabbitMQ Consumers
        services.AddHostedService<ControlCreatedConsumer>();
        services.AddHostedService<SurveyFeedbackConsumer>();

        // Polly Circuit Breaker HTTP clients
        services.AddCircuitBreakerPolicies();

        return services;
    }
}
