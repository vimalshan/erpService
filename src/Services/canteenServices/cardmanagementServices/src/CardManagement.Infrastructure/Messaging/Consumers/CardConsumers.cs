using MassTransit;
using Microsoft.Extensions.Logging;
using CardManagement.Domain.Events;

namespace CardManagement.Infrastructure.Messaging.Consumers;

public class GuestCardCreatedConsumer : IConsumer<GuestCardCreatedEvent>
{
    private readonly ILogger<GuestCardCreatedConsumer> _logger;

    public GuestCardCreatedConsumer(ILogger<GuestCardCreatedConsumer> logger) => _logger = logger;

    public Task Consume(ConsumeContext<GuestCardCreatedEvent> context)
    {
        _logger.LogInformation(
            "GuestCardCreated: CanteenUnit={CanteenUnit}, CardNumber={CardNumber}, CardName={CardName}",
            context.Message.CanteenUnit, context.Message.CardNumber, context.Message.CardName);
        return Task.CompletedTask;
    }
}

public class GuestCardClosedConsumer : IConsumer<GuestCardClosedEvent>
{
    private readonly ILogger<GuestCardClosedConsumer> _logger;

    public GuestCardClosedConsumer(ILogger<GuestCardClosedConsumer> logger) => _logger = logger;

    public Task Consume(ConsumeContext<GuestCardClosedEvent> context)
    {
        _logger.LogInformation(
            "GuestCardClosed: CanteenUnit={CanteenUnit}, CardNumber={CardNumber}",
            context.Message.CanteenUnit, context.Message.CardNumber);
        return Task.CompletedTask;
    }
}

public class CardSettledConsumer : IConsumer<CardSettledEvent>
{
    private readonly ILogger<CardSettledConsumer> _logger;

    public CardSettledConsumer(ILogger<CardSettledConsumer> logger) => _logger = logger;

    public Task Consume(ConsumeContext<CardSettledEvent> context)
    {
        _logger.LogInformation(
            "CardSettled: CanteenUnit={CanteenUnit}, CardNumber={CardNumber}, Date={Date}",
            context.Message.CanteenUnit, context.Message.CardNumber, context.Message.SettlementDate);
        return Task.CompletedTask;
    }
}
