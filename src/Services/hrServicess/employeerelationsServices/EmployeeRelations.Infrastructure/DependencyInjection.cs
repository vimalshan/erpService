using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Azure.Storage.Blobs;
using RabbitMQ.Client;
using EmployeeRelations.Domain.Interfaces;
using EmployeeRelations.Infrastructure.Persistence.EfCore;
using EmployeeRelations.Infrastructure.Persistence.Dapper;
using EmployeeRelations.Infrastructure.Repositories;
using EmployeeRelations.Infrastructure.Services;
using EmployeeRelations.Infrastructure.Messaging;
using EmployeeRelations.Infrastructure.Messaging.Consumers;

namespace EmployeeRelations.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("HrDatabase")!;

        // EF Core
        services.AddDbContext<EmployeeRelationsDbContext>(opts =>
            opts.UseSqlServer(connectionString,
                sql => sql.MigrationsAssembly(typeof(DependencyInjection).Assembly.FullName)));

        // Repositories
        services.AddScoped<IDisciplinaryRepository, DisciplinaryRepository>();
        services.AddScoped<IEwsRepository, EwsRepository>();
        services.AddScoped<ISurveyRepository, SurveyRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Dapper read repository
        services.AddScoped<IDapperReadRepository>(_ => new DapperReadRepository(connectionString));

        // Azure Blob Storage
        var blobConnectionString = config.GetConnectionString("AzureStorage") ?? "UseDevelopmentStorage=true";
        services.AddSingleton(new BlobServiceClient(blobConnectionString));
        services.AddScoped<IBlobStorageService, AzureBlobStorageService>();

        // RabbitMQ — optional: app degrades gracefully when broker is unavailable
        var rabbitConfig = config.GetSection("RabbitMQ");
        IConnection? rabbitConnection = null;
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = rabbitConfig["Host"] ?? "localhost",
                UserName = rabbitConfig["Username"] ?? "guest",
                Password = rabbitConfig["Password"] ?? "guest",
                VirtualHost = rabbitConfig["VirtualHost"] ?? "/",
                Port = int.TryParse(rabbitConfig["Port"], out var port) ? port : 5672,
                RequestedConnectionTimeout = TimeSpan.FromSeconds(5)
            };
            rabbitConnection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Warning] RabbitMQ unavailable: {ex.Message}. Messaging features will be disabled.");
        }

        if (rabbitConnection is not null)
        {
            services.AddSingleton<IConnection>(rabbitConnection);
            services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
            services.AddHostedService<DisciplinaryActionConsumer>();
            services.AddHostedService<EwsCompletedConsumer>();
            services.AddHostedService<SurveyResponseConsumer>();
        }
        else
        {
            services.AddSingleton<IMessagePublisher, NullMessagePublisher>();
        }

        return services;
    }
}
