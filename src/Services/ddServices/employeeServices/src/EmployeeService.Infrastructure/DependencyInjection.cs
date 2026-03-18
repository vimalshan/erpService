using EmployeeService.Application.Mappings;
using EmployeeService.Application.Validators;
using EmployeeService.Application.Abstractions;
using EmployeeService.Domain.Repositories;
using EmployeeService.Infrastructure.Data;
using EmployeeService.Infrastructure.Messaging;
using EmployeeService.Infrastructure.Repositories;
using EmployeeService.Shared.Messaging;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EmployeeService.Infrastructure
{
    /// <summary>
    /// Dependency Injection Extension Methods
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Add Infrastructure services to DI container
        /// </summary>
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString, IConfiguration configuration)
        {
            // Add DbContext
            services.AddDbContext<EmployeeServiceDbContext>(options =>
                options.UseSqlServer(connectionString,
                    b => b.MigrationsAssembly(typeof(EmployeeServiceDbContext).Assembly.FullName)));

            // Add Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Add Repositories
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            // Add RabbitMQ messaging
            var rabbitMqSection = configuration.GetSection("RabbitMQ");
            var rabbitMqSettings = new RabbitMqSettings
            {
                HostName = rabbitMqSection["HostName"] ?? "localhost",
                Port = int.TryParse(rabbitMqSection["Port"], out var port) ? port : 5672,
                UserName = rabbitMqSection["UserName"] ?? "guest",
                Password = rabbitMqSection["Password"] ?? "guest",
                VirtualHost = rabbitMqSection["VirtualHost"] ?? "/",
                EmployeeEventsQueueName = rabbitMqSection["EmployeeEventsQueueName"] ?? "employee.events",
                PublishRetryCount = int.TryParse(rabbitMqSection["PublishRetryCount"], out var publishRetryCount) ? publishRetryCount : 2,
                PublishRetryDelaySeconds = int.TryParse(rabbitMqSection["PublishRetryDelaySeconds"], out var publishRetryDelaySeconds) ? publishRetryDelaySeconds : 2,
                PublishCircuitBreakDurationSeconds = int.TryParse(rabbitMqSection["PublishCircuitBreakDurationSeconds"], out var publishCircuitBreakDurationSeconds) ? publishCircuitBreakDurationSeconds : 30,
                PublishCircuitMinimumThroughput = int.TryParse(rabbitMqSection["PublishCircuitMinimumThroughput"], out var publishCircuitMinimumThroughput) ? publishCircuitMinimumThroughput : 2,
                PublishCircuitFailureRatio = double.TryParse(rabbitMqSection["PublishCircuitFailureRatio"], out var publishCircuitFailureRatio) ? publishCircuitFailureRatio : 0.5,
                ConsumerRetryDelaySeconds = int.TryParse(rabbitMqSection["ConsumerRetryDelaySeconds"], out var consumerRetryDelaySeconds) ? consumerRetryDelaySeconds : 10
            };
            services.AddSingleton(rabbitMqSettings);
            services.AddSingleton<IEmployeeEventPublisher, RabbitMqEmployeeEventPublisher>();

            return services;
        }

        /// <summary>
        /// Add Application services to DI container
        /// </summary>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Add MediatR - for CQRS Command/Query handling
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(EmployeeService.Application.Handlers.Commands.CreateEmployeeCommandHandler).Assembly));

            // Add AutoMapper
            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            // Add FluentValidation
            services.AddValidatorsFromAssembly(typeof(CreateEmployeeCommandValidator).Assembly);

            return services;
        }
    }
}
