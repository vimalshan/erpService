using MediatR;
using Microsoft.Extensions.Logging;
using PFTransactionalService.Domain.Events;
using PFTransactionalService.Domain.Interfaces;

namespace PFTransactionalService.Application.EventHandlers;

public class PFAccumulationCreatedEventHandler : INotificationHandler<PFAccumulationCreatedEvent>
{
    private readonly ILogger<PFAccumulationCreatedEventHandler> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public PFAccumulationCreatedEventHandler(ILogger<PFAccumulationCreatedEventHandler> logger, IMessagePublisher messagePublisher)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public async Task Handle(PFAccumulationCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("PF Accumulation created for employee {EmpSysId}, member {MemberNo}, balance: {Balance}",
            notification.EmpSysId, notification.MemberNo, notification.InitialBalance);

        await _messagePublisher.PublishAsync("pftransaction-exchange", "pftransaction.accumulation.created", notification, cancellationToken);
    }
}

public class ContributionPostedEventHandler : INotificationHandler<ContributionPostedEvent>
{
    private readonly ILogger<ContributionPostedEventHandler> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public ContributionPostedEventHandler(ILogger<ContributionPostedEventHandler> logger, IMessagePublisher messagePublisher)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public async Task Handle(ContributionPostedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Contribution posted for employee {EmpSysId}, EE: {Emp}, ER: {Er}, Month: {Month}",
            notification.EmpSysId, notification.EmpContribution, notification.ErContribution, notification.TxnMonth);

        await _messagePublisher.PublishAsync("pftransaction-exchange", "pftransaction.contribution.posted", notification, cancellationToken);
    }
}

public class WithdrawalProcessedEventHandler : INotificationHandler<WithdrawalProcessedEvent>
{
    private readonly ILogger<WithdrawalProcessedEventHandler> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public WithdrawalProcessedEventHandler(ILogger<WithdrawalProcessedEventHandler> logger, IMessagePublisher messagePublisher)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public async Task Handle(WithdrawalProcessedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Withdrawal processed for employee {EmpSysId}, amount: {Amount}",
            notification.EmpSysId, notification.Amount);

        await _messagePublisher.PublishAsync("pftransaction-exchange", "pftransaction.withdrawal.processed", notification, cancellationToken);
    }
}

public class InterestAppliedEventHandler : INotificationHandler<InterestAppliedEvent>
{
    private readonly ILogger<InterestAppliedEventHandler> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public InterestAppliedEventHandler(ILogger<InterestAppliedEventHandler> logger, IMessagePublisher messagePublisher)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public async Task Handle(InterestAppliedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Interest applied for employee {EmpSysId}, amount: {Amount}",
            notification.EmpSysId, notification.InterestAmount);

        await _messagePublisher.PublishAsync("pftransaction-exchange", "pftransaction.interest.applied", notification, cancellationToken);
    }
}

public class PFAccumulationClosedEventHandler : INotificationHandler<PFAccumulationClosedEvent>
{
    private readonly ILogger<PFAccumulationClosedEventHandler> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public PFAccumulationClosedEventHandler(ILogger<PFAccumulationClosedEventHandler> logger, IMessagePublisher messagePublisher)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public async Task Handle(PFAccumulationClosedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("PF Accumulation closed for employee {EmpSysId}, member {MemberNo}",
            notification.EmpSysId, notification.MemberNo);

        await _messagePublisher.PublishAsync("pftransaction-exchange", "pftransaction.accumulation.closed", notification, cancellationToken);
    }
}
