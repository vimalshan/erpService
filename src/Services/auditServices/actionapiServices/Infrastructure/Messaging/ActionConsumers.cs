using ActionService.Domain.Entities;
using MassTransit;

namespace ActionService.Infrastructure.Messaging;

public class ActionCreatedConsumer : IConsumer<ActionCreatedEvent>
{
    private readonly ILogger<ActionCreatedConsumer> _logger;

    public ActionCreatedConsumer(ILogger<ActionCreatedConsumer> logger) => _logger = logger;

    public Task Consume(ConsumeContext<ActionCreatedEvent> context)
    {
        _logger.LogInformation("Action created event received: {ActionId}", context.Message.ActionItem.Id);
        return Task.CompletedTask;
    }
}

public class ActionCompletedConsumer : IConsumer<ActionCompletedEvent>
{
    private readonly ILogger<ActionCompletedConsumer> _logger;

    public ActionCompletedConsumer(ILogger<ActionCompletedConsumer> logger) => _logger = logger;

    public Task Consume(ConsumeContext<ActionCompletedEvent> context)
    {
        _logger.LogInformation("Action completed event received: {ActionId}", context.Message.ActionItem.Id);
        return Task.CompletedTask;
    }
}
