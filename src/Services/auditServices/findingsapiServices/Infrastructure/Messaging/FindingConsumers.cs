using FindingsAPI.Gateway.Domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FindingsAPI.Gateway.Infrastructure.Messaging;

public class FindingCreatedConsumer : IConsumer<FindingCreatedEvent>
{
    private readonly ILogger<FindingCreatedConsumer> _logger;
    public FindingCreatedConsumer(ILogger<FindingCreatedConsumer> logger) => _logger = logger;
    public Task Consume(ConsumeContext<FindingCreatedEvent> context)
    {
        _logger.LogInformation("Finding created: {FindingId} - {Title} (Type: {FindingType})",
            context.Message.FindingId, context.Message.Title, context.Message.FindingType);
        return Task.CompletedTask;
    }
}

public class FindingClosedConsumer : IConsumer<FindingClosedEvent>
{
    private readonly ILogger<FindingClosedConsumer> _logger;
    public FindingClosedConsumer(ILogger<FindingClosedConsumer> logger) => _logger = logger;
    public Task Consume(ConsumeContext<FindingClosedEvent> context)
    {
        _logger.LogInformation("Finding closed: {FindingId} on {ClosedDate}",
            context.Message.FindingId, context.Message.ClosedDate);
        return Task.CompletedTask;
    }
}

public class FindingResponseAddedConsumer : IConsumer<FindingResponseAddedEvent>
{
    private readonly ILogger<FindingResponseAddedConsumer> _logger;
    public FindingResponseAddedConsumer(ILogger<FindingResponseAddedConsumer> logger) => _logger = logger;
    public Task Consume(ConsumeContext<FindingResponseAddedEvent> context)
    {
        _logger.LogInformation("Finding response added: {ResponseId} for Finding {FindingId} (Type: {ResponseType})",
            context.Message.FindingResponseId, context.Message.FindingId, context.Message.ResponseType);
        return Task.CompletedTask;
    }
}
