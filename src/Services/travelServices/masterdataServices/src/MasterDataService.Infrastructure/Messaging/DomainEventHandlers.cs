using MasterDataService.Domain.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MasterDataService.Infrastructure.Messaging;

public class GuestHouseCreatedEventHandler : INotificationHandler<GuestHouseCreatedEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<GuestHouseCreatedEventHandler> _logger;

    public GuestHouseCreatedEventHandler(IPublishEndpoint publishEndpoint, ILogger<GuestHouseCreatedEventHandler> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task Handle(GuestHouseCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Publishing GuestHouseCreated message for AdminCode: {AdminCode}", notification.GuestHouse.AdminCode);
        await _publishEndpoint.Publish(new GuestHouseCreatedMessage
        {
            AdminCode = notification.GuestHouse.AdminCode,
            GuestHouseName = notification.GuestHouse.GuestHouseName,
            DailyAmount = notification.GuestHouse.DailyAmount,
            CreatedAt = notification.OccurredOn
        }, cancellationToken);
    }
}

public class GuestHouseCreatedMessage
{
    public long AdminCode { get; set; }
    public string GuestHouseName { get; set; } = string.Empty;
    public long DailyAmount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AreaCreatedEventHandler : INotificationHandler<AreaCreatedEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<AreaCreatedEventHandler> _logger;

    public AreaCreatedEventHandler(IPublishEndpoint publishEndpoint, ILogger<AreaCreatedEventHandler> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task Handle(AreaCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Publishing AreaCreated message for AreaId: {AreaId}", notification.Area.AreaId);
        await _publishEndpoint.Publish(new AreaCreatedMessage
        {
            AreaId = notification.Area.AreaId,
            AreaName = notification.Area.AreaName,
            CreatedAt = notification.OccurredOn
        }, cancellationToken);
    }
}

public class AreaCreatedMessage
{
    public int AreaId { get; set; }
    public string AreaName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class RouteCreatedEventHandler : INotificationHandler<RouteCreatedEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<RouteCreatedEventHandler> _logger;

    public RouteCreatedEventHandler(IPublishEndpoint publishEndpoint, ILogger<RouteCreatedEventHandler> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task Handle(RouteCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Publishing RouteCreated message for RouteId: {RouteId}", notification.Route.RouteId);
        await _publishEndpoint.Publish(new RouteCreatedMessage
        {
            RouteId = notification.Route.RouteId,
            RouteName = notification.Route.RouteName,
            CreatedAt = notification.OccurredOn
        }, cancellationToken);
    }
}

public class RouteCreatedMessage
{
    public int RouteId { get; set; }
    public string RouteName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
