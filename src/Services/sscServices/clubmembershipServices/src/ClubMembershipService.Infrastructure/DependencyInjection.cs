using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using ClubMembershipService.Domain.Interfaces;
using ClubMembershipService.Infrastructure.Dapper;
using ClubMembershipService.Infrastructure.Data;
using ClubMembershipService.Infrastructure.Messaging;
using ClubMembershipService.Infrastructure.Repositories;
using ClubMembershipService.Infrastructure.Services;

namespace ClubMembershipService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<ClubMembershipDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ClubMembershipDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<IClubRepository, ClubRepository>();
        services.AddScoped<IClubMembershipRepository, ClubMembershipRepository>();
        services.AddScoped<IClubActivityRepository, ClubActivityRepository>();

        // Dapper
        services.AddScoped(_ =>
            new ClubDapperRepository(configuration.GetConnectionString("DefaultConnection")!));

        // Blob Storage
        services.AddSingleton<BlobStorageService>();

        // RabbitMQ Publisher
        services.AddSingleton<RabbitMqPublisher>();

        // RabbitMQ Consumers
        services.AddHostedService<MembershipCreatedConsumer>();

        return services;
    }

    public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30));

    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}
