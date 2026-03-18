using CalendarService.Domain.Interfaces;
using CalendarService.Infrastructure.Dapper;
using CalendarService.Infrastructure.Messaging.Consumers;
using CalendarService.Infrastructure.Persistence;
using CalendarService.Infrastructure.Persistence.Repositories;
using CalendarService.Infrastructure.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CalendarService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        // EF Core
        services.AddDbContext<CalendarDbContext>(opts =>
            opts.UseSqlServer(config.GetConnectionString("CalendarDb"),
                sql => sql.MigrationsAssembly(typeof(CalendarDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<ICalendarRepository, CalendarRepository>();
        services.AddScoped<IHolidayRepository, HolidayRepository>();
        services.AddScoped<IShiftRepository, ShiftRepository>();
        services.AddScoped<IPatternRepository, PatternRepository>();

        // Dapper read service
        services.AddScoped<DapperReadService>();

        // Blob Storage
        services.AddSingleton<IBlobStorageService, BlobStorageService>();

        // MassTransit / RabbitMQ
        services.AddMassTransit(x =>
        {
            x.AddConsumer<CalendarCreatedConsumer>();
            x.AddConsumer<HolidayCreatedConsumer>();
            x.AddConsumer<ShiftCreatedConsumer>();

            x.UsingRabbitMq((ctx, cfg) =>
            {
                var rmq = config.GetSection("RabbitMQ");
                cfg.Host(rmq["Host"] ?? "localhost", rmq["VirtualHost"] ?? "/", h =>
                {
                    h.Username(rmq["Username"] ?? "guest");
                    h.Password(rmq["Password"] ?? "guest");
                });

                cfg.ReceiveEndpoint("calendar-created", e => e.ConfigureConsumer<CalendarCreatedConsumer>(ctx));
                cfg.ReceiveEndpoint("holiday-created", e => e.ConfigureConsumer<HolidayCreatedConsumer>(ctx));
                cfg.ReceiveEndpoint("shift-created", e => e.ConfigureConsumer<ShiftCreatedConsumer>(ctx));
            });
        });

        return services;
    }
}
