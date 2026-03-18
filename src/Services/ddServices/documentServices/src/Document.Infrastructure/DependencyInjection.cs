using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Document.Application.Common.Interfaces;
using Document.Domain.Interfaces;
using Document.Infrastructure.Consumers;
using Document.Infrastructure.Persistence;
using Document.Infrastructure.Persistence.Repositories;
using Document.Infrastructure.Services;

namespace Document.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<DocumentDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null)));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<DocumentDbContext>());

        // Generic repository
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // App services
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IBlobStorageService, BlobStorageService>();
        services.AddScoped<IMessagePublisher, MessagePublisher>();

        // MassTransit + RabbitMQ
        services.AddMassTransit(x =>
        {
            x.AddConsumer<LetterGeneratedConsumer>();
            x.AddConsumer<LetterOpenedConsumer>();

            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(configuration["RabbitMQ:Host"] ?? "localhost", "/", h =>
                {
                    h.Username(configuration["RabbitMQ:Username"] ?? "guest");
                    h.Password(configuration["RabbitMQ:Password"] ?? "guest");
                });

                cfg.ReceiveEndpoint("letter-generated-queue", e =>
                    e.ConfigureConsumer<LetterGeneratedConsumer>(ctx));

                cfg.ReceiveEndpoint("letter-opened-queue", e =>
                    e.ConfigureConsumer<LetterOpenedConsumer>(ctx));
            });
        });

        return services;
    }
}
