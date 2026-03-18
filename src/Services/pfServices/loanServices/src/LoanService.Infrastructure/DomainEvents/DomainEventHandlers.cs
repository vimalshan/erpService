using LoanService.Domain.Common;
using LoanService.Domain.Events;
using LoanService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LoanService.Infrastructure.DomainEvents;

public class LoanCreatedEventHandler : INotificationHandler<LoanCreatedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<LoanCreatedEventHandler> _logger;

    public LoanCreatedEventHandler(IMessagePublisher publisher, ILogger<LoanCreatedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(LoanCreatedEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Domain Event: Loan {LoanNo} created for member {MemberId}", notification.LoanNo, notification.MemberId);
        await _publisher.PublishAsync("loan-exchange", "loan.created", notification, ct);
    }
}

public class LoanApprovedEventHandler : INotificationHandler<LoanApprovedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<LoanApprovedEventHandler> _logger;

    public LoanApprovedEventHandler(IMessagePublisher publisher, ILogger<LoanApprovedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(LoanApprovedEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Domain Event: Loan {LoanNo} approved", notification.LoanNo);
        await _publisher.PublishAsync("loan-exchange", "loan.approved", notification, ct);
    }
}

public class LoanClosedEventHandler : INotificationHandler<LoanClosedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<LoanClosedEventHandler> _logger;

    public LoanClosedEventHandler(IMessagePublisher publisher, ILogger<LoanClosedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(LoanClosedEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Domain Event: Loan {LoanNo} closed", notification.LoanNo);
        await _publisher.PublishAsync("loan-exchange", "loan.closed", notification, ct);
    }
}

public class RepaymentMadeEventHandler : INotificationHandler<RepaymentMadeEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<RepaymentMadeEventHandler> _logger;

    public RepaymentMadeEventHandler(IMessagePublisher publisher, ILogger<RepaymentMadeEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(RepaymentMadeEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Domain Event: Payment made for Loan {LoanNo}", notification.LoanNo);
        await _publisher.PublishAsync("loan-exchange", "loan.payment.made", notification, ct);
    }
}
