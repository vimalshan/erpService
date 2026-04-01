using LetTransactionService.Application.Interfaces;
using LetTransactionService.Domain.Interfaces;
using LetTransactionService.Infrastructure.BlobStorage;
using LetTransactionService.Infrastructure.Dapper;
using LetTransactionService.Infrastructure.Data;
using LetTransactionService.Infrastructure.DomainEvents;
using LetTransactionService.Infrastructure.Messaging;
using LetTransactionService.Infrastructure.Messaging.Consumers;
using LetTransactionService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LetTransactionService.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<LetTransactionDbContext>(opts =>
            opts.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(typeof(LetTransactionDbContext).Assembly.FullName)
                          .EnableRetryOnFailure(3)));

        // Unit of Work / Repositories
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<LetTransactionDbContext>());
        services.AddScoped<ILetRequestRepository, LetRequestRepository>();
        services.AddScoped<IFeedbackRepository, FeedbackRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();

        // Dapper read-side
        services.AddScoped<DapperLetReadRepository>();

        // Domain event dispatcher
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        // Blob Storage
        services.AddScoped<IBlobStorageService, AzureBlobStorageService>();

        // RabbitMQ
        services.Configure<RabbitMqOptions>(opts =>
            configuration.GetSection(RabbitMqOptions.SectionName).Bind(opts));
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
        services.AddHostedService<ReviewApprovalConsumer>();

        return services;
    }
}
