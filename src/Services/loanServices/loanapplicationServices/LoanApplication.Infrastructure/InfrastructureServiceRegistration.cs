using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using LoanApplication.Domain.Interfaces;
using LoanApplication.Infrastructure.Data;
using LoanApplication.Infrastructure.Repositories;
using LoanApplication.Infrastructure.Services;
using LoanApplication.Infrastructure.UnitOfWork;
using LoanApplication.Infrastructure.Messaging;
using LoanApplication.Infrastructure.Resilience;

namespace LoanApplication.Infrastructure;

/// <summary>
/// Infrastructure layer service registration
/// </summary>
public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register DbContext
        services.AddDbContext<LoanApplicationDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(typeof(LoanApplicationDbContext).Assembly.FullName);
                sqlOptions.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(1), errorNumbersToAdd: null);
            });
        });

        // Register repositories
        services.AddScoped<ILoanApplicationRepository, LoanApplicationRepository>();

        // Register Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        // Register domain services
        services.AddScoped<ILoanEligibilityService, LoanEligibilityService>();

        // Register RabbitMQ settings and message bus
        var rabbitMQSettings = configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>() ?? new RabbitMQSettings();
        services.AddSingleton(rabbitMQSettings);
        services.AddSingleton<IMessageBus, RabbitMQMessageBus>();

        // Register Polly resilience pipelines
        var circuitBreakerSettings = configuration.GetSection("CircuitBreaker").Get<CircuitBreakerSettings>()
            ?? new CircuitBreakerSettings();
        services.AddLoanApplicationResiliencePolicies(circuitBreakerSettings);

        return services;
    }
}
