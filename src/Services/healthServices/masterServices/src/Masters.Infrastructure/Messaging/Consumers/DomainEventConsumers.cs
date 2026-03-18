using Masters.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Masters.Infrastructure.Messaging.Consumers;

public class LovTypeMasterCreatedConsumer : RabbitMqConsumer<LovTypeMasterCreatedEvent>
{
    private readonly ILogger<LovTypeMasterCreatedConsumer> _logger;

    public LovTypeMasterCreatedConsumer(
        string connectionString,
        ILogger<LovTypeMasterCreatedConsumer> logger) 
        : base(connectionString, "lov-typemaster-created", logger)
    {
        _logger = logger;
    }

    protected override Task HandleMessageAsync(LovTypeMasterCreatedEvent message, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Processing LovTypeMasterCreated event: {LovTypeCode} - {LovTypeName}", 
            message.LovTypeCode, 
            message.LovTypeName);

        // Add your business logic here (e.g., send notifications, update cache, etc.)

        return Task.CompletedTask;
    }
}

public class LovMasterCreatedConsumer : RabbitMqConsumer<LovMasterCreatedEvent>
{
    private readonly ILogger<LovMasterCreatedConsumer> _logger;

    public LovMasterCreatedConsumer(
        string connectionString,
        ILogger<LovMasterCreatedConsumer> logger) 
        : base(connectionString, "lov-master-created", logger)
    {
        _logger = logger;
    }

    protected override Task HandleMessageAsync(LovMasterCreatedEvent message, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Processing LovMasterCreated event: {LovId} - {LovName}", 
            message.LovId, 
            message.LovName);

        // Add your business logic here (e.g., send notifications, update cache, etc.)

        return Task.CompletedTask;
    }
}
