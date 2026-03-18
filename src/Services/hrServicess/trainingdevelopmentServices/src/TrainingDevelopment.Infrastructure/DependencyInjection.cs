using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrainingDevelopment.Domain.Interfaces;
using TrainingDevelopment.Infrastructure.BlobStorage;
using TrainingDevelopment.Infrastructure.Dapper;
using TrainingDevelopment.Infrastructure.Data;
using TrainingDevelopment.Infrastructure.Messaging;
using TrainingDevelopment.Infrastructure.Repositories;

namespace TrainingDevelopment.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                    sqlOptions.CommandTimeout(60);
                })
            .ConfigureWarnings(w => w.Ignore(
                Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning)));

        // Dapper
        services.AddSingleton<DapperContext>();
        services.AddScoped<TrainingDetailDapperRepository>();

        // Repositories
        services.AddScoped<ITrainingDetailRepository, TrainingDetailRepository>();
        services.AddScoped<IInstituteMasterRepository, InstituteMasterRepository>();
        services.AddScoped<IProgramLovRepository, ProgramLovRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Messaging
        services.AddSingleton<RabbitMQProducer>();
        services.AddHostedService<TrainingEventConsumer>();

        // MediatR handlers from this assembly (domain event handlers)
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        // Blob Storage
        services.AddSingleton<BlobStorageService>();

        return services;
    }
}
