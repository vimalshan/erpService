using MasterService.Domain.Interfaces;
using MasterService.Infrastructure.BlobStorage;
using MasterService.Infrastructure.Dapper;
using MasterService.Infrastructure.Messaging;
using MasterService.Infrastructure.Persistence;
using MasterService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace MasterService.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sql => sql.EnableRetryOnFailure(maxRetryCount: 3));
            // Suppress the pending-changes check that fires when a manually-created
            // model snapshot doesn't byte-for-byte match EF's compiled model.
            options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
        });

        // Repositories
        services.AddScoped<ISkillRepository, SkillRepository>();
        services.AddScoped<ITrainingRepository, TrainingRepository>();
        services.AddScoped<IJobRepository, JobRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IFinancialYearRepository, FinancialYearRepository>();

        // Dapper
        services.AddScoped<IDapperRepository, DapperRepository>();
        services.AddScoped<SkillDapperRepository>();

        // Messaging
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();

        // Register MediatR notification handlers in Infrastructure assembly
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(InfrastructureServiceExtensions).Assembly));

        // Blob Storage
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        return services;
    }
}
