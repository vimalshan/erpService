using ConfigService.Application.Interfaces;
using ConfigService.Domain.Common;
using ConfigService.Domain.Repositories;
using ConfigService.Infrastructure.Messaging;
using ConfigService.Infrastructure.Persistence;
using ConfigService.Infrastructure.Persistence.Repositories;
using ConfigService.Infrastructure.Services;
using Azure.Storage.Blobs;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace ConfigService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<ConfigDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(typeof(ConfigDbContext).Assembly.FullName)));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ConfigDbContext>());

        // Repositories
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<IExpenseCurrencyRepository, ExpenseCurrencyRepository>();
        services.AddScoped<IExpenseGroupRepository, ExpenseGroupRepository>();
        services.AddScoped<IExpenseTypeRepository, ExpenseTypeRepository>();
        services.AddScoped<IGlobalPayParamRepository, GlobalPayParamRepository>();
        services.AddScoped<ICalendarGstBuMapRepository, CalendarGstBuMapRepository>();
        services.AddScoped<ITravelCityRepository, TravelCityRepository>();
        services.AddScoped<ITravelCountryRepository, TravelCountryRepository>();
        services.AddScoped<ITravelClassRepository, TravelClassRepository>();
        services.AddScoped<ITravelContactRepository, TravelContactRepository>();
        services.AddScoped<ITravelBusCitySectorMapRepository, TravelBusCitySectorMapRepository>();
        services.AddScoped<ITravelBuExcludeRepository, TravelBuExcludeRepository>();
        services.AddScoped<IVendorRepository, VendorRepository>();
        services.AddScoped<IGradeCatExpenseRuleRepository, GradeCatExpenseRuleRepository>();
        services.AddScoped<IGradeCatModeMapRepository, GradeCatModeMapRepository>();
        services.AddScoped<IGradeCatStayRuleRepository, GradeCatStayRuleRepository>();
        services.AddScoped<IGradeCatExpenseMapRepository, GradeCatExpenseMapRepository>();
        services.AddScoped<IGradeTypeTravelParamRepository, GradeTypeTravelParamRepository>();

        // Dapper
        services.AddScoped<IDapperQueryService, DapperQueryService>();

        // Blob Storage
        var blobConnectionString = configuration.GetValue<string>("AzureBlobStorage:ConnectionString");
        if (!string.IsNullOrEmpty(blobConnectionString))
        {
            services.AddSingleton(new BlobServiceClient(blobConnectionString));
            services.AddScoped<IBlobStorageService, BlobStorageService>();
        }

        // RabbitMQ with MassTransit (optional – requires license and running broker)
        var rabbitHost = configuration.GetValue<string>("RabbitMQ:Host");
        if (!string.IsNullOrEmpty(rabbitHost))
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<ConfigurationUpdatedConsumer>();

                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(rabbitHost, "/", h =>
                    {
                        h.Username(configuration.GetValue<string>("RabbitMQ:Username") ?? "guest");
                        h.Password(configuration.GetValue<string>("RabbitMQ:Password") ?? "guest");
                    });

                    cfg.ReceiveEndpoint("config-service-queue", e =>
                    {
                        e.ConfigureConsumer<ConfigurationUpdatedConsumer>(ctx);
                    });
                });
            });
        }

        // Polly Circuit Breaker for HTTP clients
        services.AddHttpClient("ExternalService")
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        // MediatR for domain event handlers
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
    }
}
