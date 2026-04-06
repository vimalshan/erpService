using MassTransit;

namespace EmployeeService.API.Extensions;

/// <summary>
/// Extension method for configuring MassTransit with RabbitMQ
/// </summary>
public static class MassTransitExtensions
{
    public static IServiceCollection AddMassTransitWithRabbitMQ(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var rabbitMqConfig = configuration.GetSection("RabbitMQ");
        var host = rabbitMqConfig["Hostname"] ?? "localhost";
        var port = rabbitMqConfig.GetValue<ushort>("Port", 5672);
        var username = rabbitMqConfig["Username"] ?? "guest";
        var password = rabbitMqConfig["Password"] ?? "guest";
        var vhost = rabbitMqConfig["VirtualHost"] ?? "/";

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(host, port, vhost, h =>
                {
                    h.Username(username);
                    h.Password(password);
                });

                cfg.ConfigureEndpoints(ctx);
            });
        });

        return services;
    }
}
