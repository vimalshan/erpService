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
        // Add DbContext
        services.AddDbContext<OrderScheduleDbContext>(options =>
            options.UseSqlServer(connectionString,
                sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly("OrderScheduleService.Infrastructure");
                    sqlOptions.EnableRetryOnFailure();
                }));

        // Register Repositories
        services.AddScoped<ITiedOrderRepository, TiedOrderRepository>();
        services.AddScoped<IScheduleRepository, ScheduleRepository>();
        services.AddScoped<IShiftRepository, ShiftRepository>();

        return services;
    }
}
