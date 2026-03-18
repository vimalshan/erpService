using MemberService.Domain.Interfaces;
using MemberService.Infrastructure.Data;
using MemberService.Infrastructure.DomainEvents;
using MemberService.Infrastructure.Messaging;
using MemberService.Infrastructure.Messaging.Consumers;
using MemberService.Infrastructure.Repositories;
using MemberService.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MemberService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<MemberDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(typeof(MemberDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<DapperMemberRepository>();

        // Domain Events
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        // RabbitMQ
        services.AddSingleton<IRabbitMqPublisher, RabbitMqPublisher>();
        services.AddHostedService<MemberEventConsumer>();

        // Blob Storage
        services.AddSingleton<IBlobStorageService, BlobStorageService>();

        return services;
    }
}
