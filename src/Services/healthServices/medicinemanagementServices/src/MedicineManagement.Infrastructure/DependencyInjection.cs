using Azure.Storage.Blobs;
using MedicineManagement.Domain.Interfaces;
using MedicineManagement.Infrastructure.Dapper;
using MedicineManagement.Infrastructure.Messaging;
using MedicineManagement.Infrastructure.Messaging.Consumers;
using MedicineManagement.Infrastructure.Persistence;
using MedicineManagement.Infrastructure.Repositories;
using MedicineManagement.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MedicineManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;

        // EF Core
        services.AddDbContext<MedicineManagementDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Dapper
        services.AddSingleton(new DapperQueryService(connectionString));

        // Repositories
        services.AddScoped<IMedicineTypeRepository, MedicineTypeRepository>();
        services.AddScoped<IMedicinePackagingRepository, MedicinePackagingRepository>();
        services.AddScoped<IMedicineRepository, MedicineRepository>();
        services.AddScoped<IDoctorAttendantRepository, DoctorAttendantRepository>();
        services.AddScoped<IMedicineCreditRepository, MedicineCreditRepository>();
        services.AddScoped<IMedicineIssueRepository, MedicineIssueRepository>();
        services.AddScoped<IPurchaseRepository, PurchaseRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Azure Blob Storage
        var blobConnectionString = configuration.GetValue<string>("AzureBlobStorage:ConnectionString");
        if (!string.IsNullOrEmpty(blobConnectionString))
        {
            services.AddSingleton(new BlobServiceClient(blobConnectionString));
            services.AddScoped<IBlobStorageService, AzureBlobStorageService>();
        }

        // RabbitMQ
        var rabbitHost = configuration.GetValue("RabbitMQ:HostName", "localhost")!;
        var rabbitUser = configuration.GetValue("RabbitMQ:UserName", "guest")!;
        var rabbitPass = configuration.GetValue("RabbitMQ:Password", "guest")!;

        services.AddSingleton<IMessagePublisher>(sp =>
            new RabbitMqPublisher(rabbitHost, rabbitUser, rabbitPass,
                sp.GetRequiredService<ILogger<RabbitMqPublisher>>()));

        // RabbitMQ Consumers
        services.AddHostedService(sp =>
            new PurchaseCreatedConsumer(rabbitHost, rabbitUser, rabbitPass, sp,
                sp.GetRequiredService<ILogger<PurchaseCreatedConsumer>>()));

        services.AddHostedService(sp =>
            new LowStockAlertConsumer(rabbitHost, rabbitUser, rabbitPass, sp,
                sp.GetRequiredService<ILogger<LowStockAlertConsumer>>()));

        return services;
    }
}
