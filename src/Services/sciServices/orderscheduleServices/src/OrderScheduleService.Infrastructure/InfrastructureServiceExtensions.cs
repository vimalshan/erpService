namespace OrderScheduleService.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderScheduleService.Infrastructure.Persistence;
using OrderScheduleService.Infrastructure.Repositories;
using OrderScheduleService.Domain.Interfaces;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        string connectionString)
    {
        // Add DbContext (Transient to support HotChocolate parallel resolver execution)
        services.AddDbContext<OrderScheduleDbContext>(options =>
            options.UseSqlServer(connectionString,
                sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly("OrderScheduleService.Infrastructure");
                    sqlOptions.EnableRetryOnFailure();
                }),
            ServiceLifetime.Transient,
            ServiceLifetime.Transient);

        // Register Repositories
        services.AddTransient<ITiedOrderRepository, TiedOrderRepository>();
        services.AddTransient<IScheduleRepository, ScheduleRepository>();
        services.AddTransient<IShiftRepository, ShiftRepository>();

        return services;
    }
}
