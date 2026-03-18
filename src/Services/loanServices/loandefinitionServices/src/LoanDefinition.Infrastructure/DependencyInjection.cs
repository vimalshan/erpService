using Azure.Storage.Blobs;
using LoanDefinition.Domain.Repositories;
using LoanDefinition.Infrastructure.BlobStorage;
using LoanDefinition.Infrastructure.Dapper;
using LoanDefinition.Infrastructure.Messaging;
using LoanDefinition.Infrastructure.Messaging.Consumers;
using LoanDefinition.Infrastructure.Persistence;
using LoanDefinition.Infrastructure.Repositories;
using LoanDefinition.Infrastructure.Resilience;
using LoanDefinition.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LoanDefinition.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("LoanDb")!;

        // EF Core
        services.AddDbContext<LoanDefinitionDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<LoanDefinitionDbContext>());

        // Repositories
        services.AddScoped<ILoanTypeMasterRepository, LoanTypeMasterRepository>();
        services.AddScoped<ILoanMasterRepository, LoanMasterRepository>();
        services.AddScoped<ILoanSubClassRepository, LoanSubClassRepository>();
        services.AddScoped<ILoanInterestRateRepository, LoanInterestRateRepository>();
        services.AddScoped<ILoanLimitRangeRepository, LoanLimitRangeRepository>();
        services.AddScoped<ILoanPerquisiteRepository, LoanPerquisiteRepository>();
        services.AddScoped<ILoanFestivalRepository, LoanFestivalRepository>();
        services.AddScoped<ILoanFestivalMapRepository, LoanFestivalMapRepository>();
        services.AddScoped<ILoanAccountMasterRepository, LoanAccountMasterRepository>();

        // Dapper
        services.AddSingleton<ILoanDapperQueries>(new LoanDapperQueries(connectionString));

        // RabbitMQ
        services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMq"));
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
        services.AddHostedService<LoanApprovalConsumer>();
        services.AddHostedService<LoanRateUpdateConsumer>();

        // Azure Blob Storage
        var blobConnectionString = configuration.GetConnectionString("BlobStorage");
        if (!string.IsNullOrEmpty(blobConnectionString))
        {
            services.AddSingleton(new BlobServiceClient(blobConnectionString));
        }
        else
        {
            // Use development storage emulator
            services.AddSingleton(new BlobServiceClient("UseDevelopmentStorage=true"));
        }
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

        // Polly
        services.AddPollyPolicies();

        return services;
    }
}
