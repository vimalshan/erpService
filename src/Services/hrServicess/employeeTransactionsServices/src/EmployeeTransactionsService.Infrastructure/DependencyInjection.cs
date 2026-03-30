using Azure.Storage.Blobs;
using EmployeeTransactionsService.Application.Contracts;
using EmployeeTransactionsService.Infrastructure.Dapper;
using EmployeeTransactionsService.Infrastructure.Messaging;
using EmployeeTransactionsService.Infrastructure.Persistence;
using EmployeeTransactionsService.Infrastructure.Repositories;
using EmployeeTransactionsService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.CircuitBreaker;

namespace EmployeeTransactionsService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<EmployeeTransactionsDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null)));

        services.AddScoped(sp => (EmployeeTransactionsDbContext)sp.GetRequiredService<EmployeeTransactionsDbContext>());
        services.AddScoped<EmployeeTransactionsDbContext>();
        services.AddScoped<EmployeeTransactionsService.Domain.Interfaces.IUnitOfWork>(sp => sp.GetRequiredService<EmployeeTransactionsDbContext>());

        services.AddScoped<EmployeeTransactionsService.Domain.Interfaces.IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<EmployeeTransactionsService.Domain.Interfaces.IEmployeeGradeRepository, EmployeeGradeRepository>();
        services.AddScoped<EmployeeTransactionsService.Domain.Interfaces.IEmployeeGradeChangeRepository, EmployeeGradeChangeRepository>();
        services.AddScoped<EmployeeTransactionsService.Domain.Interfaces.IEmployeeProbationRepository, EmployeeProbationRepository>();
        services.AddScoped<EmployeeTransactionsService.Domain.Interfaces.IAlertGroupRepository, AlertGroupRepository>();
        services.AddScoped<EmployeeTransactionsService.Domain.Interfaces.IStationeryItemImageRepository, StationeryItemImageRepository>();

        services.AddSingleton(_ => new ResiliencePipelineBuilder()
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                FailureRatio = 0.5,
                SamplingDuration = TimeSpan.FromSeconds(30),
                MinimumThroughput = 2,
                BreakDuration = TimeSpan.FromSeconds(15)
            })
            .Build());

        services.AddScoped<ITransactionReadService, TransactionReadService>();

        services.AddSingleton(_ => new BlobServiceClient(configuration.GetConnectionString("BlobStorage") ?? "UseDevelopmentStorage=true"));
        services.AddScoped<IBlobStorageService, BlobStorageService>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();

        services.AddSingleton<IOptions<RabbitMqOptions>>(_ =>
            Options.Create(configuration.GetSection(RabbitMqOptions.SectionName).Get<RabbitMqOptions>() ?? new RabbitMqOptions()));
        services.AddSingleton(_ => new RabbitMqConnectionProvider(RabbitMqConnectionFactory.Create(configuration)));
        services.AddSingleton<IMessagePublisher, RabbitMqMessagePublisher>();
        services.AddHostedService<EmployeeOnboardedConsumer>();
        services.AddHostedService<ProbationReviewedConsumer>();

        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        return services;
    }
}