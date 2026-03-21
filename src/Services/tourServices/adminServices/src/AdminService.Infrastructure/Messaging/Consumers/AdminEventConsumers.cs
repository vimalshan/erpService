using AdminService.Domain.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AdminService.Infrastructure.Messaging.Consumers;

public class AdminMasterCreatedConsumer : RabbitMqConsumerBase<AdminMasterCreatedEvent>
{
    private readonly ILogger<AdminMasterCreatedConsumer> _logger;

    protected override string QueueName => "admin.master.created";
    protected override string ExchangeName => "admin.events";
    protected override string RoutingKey => "admin.master.created";

    public AdminMasterCreatedConsumer(IConfiguration configuration, ILogger<AdminMasterCreatedConsumer> logger)
        : base(configuration, logger)
    {
        _logger = logger;
    }

    protected override Task HandleMessageAsync(AdminMasterCreatedEvent message, CancellationToken ct)
    {
        _logger.LogInformation("Consumed AdminMasterCreated: {AdminId} - {AdminName}", message.AdminId, message.AdminName);
        // Process the event - send notifications, sync data, etc.
        return Task.CompletedTask;
    }
}

public class AccessRightsGrantedConsumer : RabbitMqConsumerBase<AccessRightsGrantedEvent>
{
    private readonly ILogger<AccessRightsGrantedConsumer> _logger;

    protected override string QueueName => "admin.access.granted";
    protected override string ExchangeName => "admin.events";
    protected override string RoutingKey => "admin.access.granted";

    public AccessRightsGrantedConsumer(IConfiguration configuration, ILogger<AccessRightsGrantedConsumer> logger)
        : base(configuration, logger)
    {
        _logger = logger;
    }

    protected override Task HandleMessageAsync(AccessRightsGrantedEvent message, CancellationToken ct)
    {
        _logger.LogInformation("Consumed AccessRightsGranted: {RightsId} for {UserId}", message.RightsId, message.UserId);
        return Task.CompletedTask;
    }
}
