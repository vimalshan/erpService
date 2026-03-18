using LoanManagement.Domain.Interfaces;
using LoanManagement.Infrastructure.Data;
using LoanManagement.Infrastructure.Messaging;
using LoanManagement.Infrastructure.Repositories;
using LoanManagement.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LoanManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // EF Core DbContext
        services.AddDbContext<LoanManagementDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("LoanManagement"),
                sql => sql.MigrationsAssembly(typeof(LoanManagementDbContext).Assembly.FullName)));

        // Unit of Work
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<LoanManagementDbContext>());

        // Repositories
        services.AddScoped<ILoanRepository, LoanRepository>();
        services.AddScoped<IDisbursementRepository, DisbursementRepository>();
        services.AddScoped<IInterestRepository, InterestRepository>();
        services.AddScoped<IRepaymentRepository, RepaymentRepository>();

        // RabbitMQ
        services.Configure<RabbitMqSettings>(opts =>
            configuration.GetSection("RabbitMQ").Bind(opts, o => { }));
        services.AddSingleton<RabbitMqPublisher>();
        services.AddHostedService<LoanEventConsumer>();

        // Blob Storage
        services.AddSingleton<BlobStorageService>();

        return services;
    }
}
