using Azure.Storage.Blobs;
using MeetingModule.Domain.Interfaces;
using MeetingModule.Infrastructure.BlobStorage;
using MeetingModule.Infrastructure.Messaging;
using MeetingModule.Infrastructure.Persistence;
using MeetingModule.Infrastructure.Persistence.Dapper;
using MeetingModule.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeetingModule.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;

        // EF Core
        services.AddDbContext<MeetingDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Repositories
        services.AddScoped<IMeetingTypeRepository, MeetingTypeRepository>();
        services.AddScoped<IMeetingScheduleRepository, MeetingScheduleRepository>();
        services.AddScoped<IPollDetailRepository, PollDetailRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Dapper
        services.AddSingleton<IDapperQueryService>(_ => new DapperQueryService(connectionString));

        // RabbitMQ
        services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMQ"));
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
        services.AddHostedService<MeetingCreatedConsumer>();
        services.AddHostedService<MeetingStatusChangedConsumer>();

        // Azure Blob Storage
        var blobConnectionString = configuration.GetValue<string>("AzureBlobStorage:ConnectionString");
        if (!string.IsNullOrEmpty(blobConnectionString))
        {
            services.AddSingleton(_ => new BlobServiceClient(blobConnectionString));
            services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();
        }

        // MediatR notification handlers from this assembly
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        return services;
    }
}
