using MassTransit;
using Microsoft.Extensions.Logging;
using LovService.Application.Events;

namespace LovService.Infrastructure.MessageBus;

public sealed class LovMasterCreatedConsumer : IConsumer<LovMasterCreatedIntegrationEvent>
{
    private readonly ILogger _logger;

    public LovMasterCreatedConsumer(ILogger<LovMasterCreatedConsumer> logger)
        => _logger = logger;

    public Task Consume(ConsumeContext<LovMasterCreatedIntegrationEvent> context)
    {
        _logger.LogInformation("LOV Master created integration event received: {LovId}", context.Message.LovId);
        return Task.CompletedTask;
    }
}

public sealed class LovMasterUpdatedConsumer : IConsumer<LovMasterUpdatedIntegrationEvent>
{
    private readonly ILogger _logger;

    public LovMasterUpdatedConsumer(ILogger<LovMasterUpdatedConsumer> logger)
        => _logger = logger;

    public Task Consume(ConsumeContext<LovMasterUpdatedIntegrationEvent> context)
    {
        _logger.LogInformation("LOV Master updated integration event received: {LovId}", context.Message.LovId);
        return Task.CompletedTask;
    }
}

public sealed class LovMasterDeletedConsumer : IConsumer<LovMasterDeletedIntegrationEvent>
{
    private readonly ILogger _logger;

    public LovMasterDeletedConsumer(ILogger<LovMasterDeletedConsumer> logger)
        => _logger = logger;

    public Task Consume(ConsumeContext<LovMasterDeletedIntegrationEvent> context)
    {
        _logger.LogInformation("LOV Master deleted integration event received: {LovId}", context.Message.LovId);
        return Task.CompletedTask;
    }
}
