using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.EntityFrameworkCore;
using SSCTransactional.Application.Interfaces;
using SSCTransactional.Domain.Interfaces;
using SSCTransactional.Infrastructure.DapperRepositories;
using SSCTransactional.Infrastructure.Messaging;
using SSCTransactional.Infrastructure.Messaging.Consumers;
using SSCTransactional.Infrastructure.Persistence;
using SSCTransactional.Infrastructure.Repositories;
using SSCTransactional.Infrastructure.Settings;
using SSCTransactional.Infrastructure.Storage;

namespace SSCTransactional.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        // Repositories
        services.AddScoped<IAllocationRepository, AllocationRepository>();
        services.AddScoped<ICorrespondenceRepository, CorrespondenceRepository>();
        services.AddScoped<IDocumentApprovalRepository, DocumentApprovalRepository>();
        services.AddScoped<IRescanRepository, RescanRepository>();
        services.AddScoped<IRevokeRepository, RevokeRepository>();
        services.AddScoped<IDocumentApproverRepository, DocumentApproverRepository>();
        services.AddScoped<IOracleInvoiceRepository, OracleInvoiceRepository>();
        services.AddScoped<IOraclePaymentRepository, OraclePaymentRepository>();
        services.AddScoped<IOracleBankDetailRepository, OracleBankDetailRepository>();
        services.AddScoped<IOracleDueDetailRepository, OracleDueDetailRepository>();
        services.AddScoped<IDocumentStatusRepository, DocumentStatusRepository>();

        // Dapper
        services.AddScoped<AllocationDapperRepository>();
        services.AddScoped<CorrespondenceDapperRepository>();

        // RabbitMQ
        services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQ"));
        services.AddSingleton<RabbitMQPublisher>();
        services.AddSingleton<IMessagePublisher, RabbitMQMessagePublisher>();
        services.AddHostedService<AllocationCreatedConsumer>();
        services.AddHostedService<CorrespondenceCreatedConsumer>();

        // Blob Storage
        services.AddSingleton<BlobStorageService>();

        // Circuit Breaker via HttpClientFactory (Polly v8 / Microsoft.Extensions.Http.Resilience)
        services.AddHttpClient("ExternalService", client =>
        {
            client.BaseAddress = new Uri("https://api.external-service.local/");
            client.Timeout = TimeSpan.FromSeconds(60);
        }).AddStandardResilienceHandler(options =>
        {
            options.CircuitBreaker.FailureRatio = 0.5;
            options.CircuitBreaker.MinimumThroughput = 5;
            options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(30);
        });

        // MediatR for domain event handlers in Infrastructure
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        return services;
    }
}
