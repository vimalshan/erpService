using MassTransit;
using Microsoft.Extensions.Logging;

namespace MasterDataService.Infrastructure.Messaging.Consumers;

public class GuestHouseCreatedConsumer : IConsumer<GuestHouseCreatedMessage>
{
    private readonly ILogger<GuestHouseCreatedConsumer> _logger;

    public GuestHouseCreatedConsumer(ILogger<GuestHouseCreatedConsumer> logger) => _logger = logger;

    public Task Consume(ConsumeContext<GuestHouseCreatedMessage> context)
    {
        _logger.LogInformation("Consumed GuestHouseCreated: {GuestHouseName} (AdminCode: {AdminCode})",
            context.Message.GuestHouseName, context.Message.AdminCode);
        return Task.CompletedTask;
    }
}

public class AreaCreatedConsumer : IConsumer<AreaCreatedMessage>
{
    private readonly ILogger<AreaCreatedConsumer> _logger;

    public AreaCreatedConsumer(ILogger<AreaCreatedConsumer> logger) => _logger = logger;

    public Task Consume(ConsumeContext<AreaCreatedMessage> context)
    {
        _logger.LogInformation("Consumed AreaCreated: {AreaName} (AreaId: {AreaId})",
            context.Message.AreaName, context.Message.AreaId);
        return Task.CompletedTask;
    }
}

public class RouteCreatedConsumer : IConsumer<RouteCreatedMessage>
{
    private readonly ILogger<RouteCreatedConsumer> _logger;

    public RouteCreatedConsumer(ILogger<RouteCreatedConsumer> logger) => _logger = logger;

    public Task Consume(ConsumeContext<RouteCreatedMessage> context)
    {
        _logger.LogInformation("Consumed RouteCreated: {RouteName} (RouteId: {RouteId})",
            context.Message.RouteName, context.Message.RouteId);
        return Task.CompletedTask;
    }
}
