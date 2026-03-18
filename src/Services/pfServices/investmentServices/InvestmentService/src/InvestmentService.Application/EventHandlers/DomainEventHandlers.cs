using InvestmentService.Domain.Events;
using InvestmentService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InvestmentService.Application.EventHandlers;

public class InvestmentPurchasedEventHandler : INotificationHandler<InvestmentPurchasedEvent>
{
    private readonly ILogger<InvestmentPurchasedEventHandler> _logger;
    private readonly IMessagePublisher _publisher;

    public InvestmentPurchasedEventHandler(ILogger<InvestmentPurchasedEventHandler> logger, IMessagePublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(InvestmentPurchasedEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Investment {InvNo} purchased on {Date} for {Value}",
            notification.InvestmentNo, notification.PurchaseDate, notification.PurchaseValue);

        await _publisher.PublishAsync("investment-events", "event.investment.purchased", notification, ct);
    }
}

public class InvestmentRedeemedEventHandler : INotificationHandler<InvestmentRedeemedEvent>
{
    private readonly ILogger<InvestmentRedeemedEventHandler> _logger;
    private readonly IMessagePublisher _publisher;

    public InvestmentRedeemedEventHandler(ILogger<InvestmentRedeemedEventHandler> logger, IMessagePublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(InvestmentRedeemedEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Investment {InvNo} redeemed on {Date} for {Value}",
            notification.InvestmentNo, notification.RedemptionDate, notification.RedemptionValue);

        await _publisher.PublishAsync("investment-events", "event.investment.redeemed", notification, ct);
    }
}

public class InvestmentMaturedEventHandler : INotificationHandler<InvestmentMaturedEvent>
{
    private readonly ILogger<InvestmentMaturedEventHandler> _logger;
    private readonly IMessagePublisher _publisher;

    public InvestmentMaturedEventHandler(ILogger<InvestmentMaturedEventHandler> logger, IMessagePublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(InvestmentMaturedEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Investment {InvNo} matured on {Date}",
            notification.InvestmentNo, notification.MaturityDate);

        await _publisher.PublishAsync("investment-events", "event.investment.matured", notification, ct);
    }
}

public class InvestmentApprovedEventHandler : INotificationHandler<InvestmentApprovedEvent>
{
    private readonly ILogger<InvestmentApprovedEventHandler> _logger;

    public InvestmentApprovedEventHandler(ILogger<InvestmentApprovedEventHandler> logger) { _logger = logger; }

    public Task Handle(InvestmentApprovedEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Investment {InvNo} approved by {Approver} on {Date}",
            notification.InvestmentNo, notification.ApproverSysId, notification.ApprovedOn);
        return Task.CompletedTask;
    }
}
