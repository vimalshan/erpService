using LoanDefinition.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LoanDefinition.Application.Features.DomainEventHandlers;

public class LoanTypeCreatedEventHandler(ILogger<LoanTypeCreatedEventHandler> logger)
    : INotificationHandler<LoanTypeCreatedEvent>
{
    public Task Handle(LoanTypeCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event: LoanType {LoanTypeId} '{LoanName}' created", notification.LoanTypeId, notification.LoanName);
        return Task.CompletedTask;
    }
}

public class LoanMasterCreatedEventHandler(ILogger<LoanMasterCreatedEventHandler> logger)
    : INotificationHandler<LoanMasterCreatedEvent>
{
    public Task Handle(LoanMasterCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event: Loan {LoanId} '{LoanName}' created", notification.LoanId, notification.LoanName);
        return Task.CompletedTask;
    }
}

public class LoanMasterUpdatedEventHandler(ILogger<LoanMasterUpdatedEventHandler> logger)
    : INotificationHandler<LoanMasterUpdatedEvent>
{
    public Task Handle(LoanMasterUpdatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event: Loan {LoanId} '{LoanName}' updated", notification.LoanId, notification.LoanName);
        return Task.CompletedTask;
    }
}

public class InterestRateChangedEventHandler(ILogger<InterestRateChangedEventHandler> logger)
    : INotificationHandler<InterestRateChangedEvent>
{
    public Task Handle(InterestRateChangedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event: Interest rate changed for Loan {LoanId}, Rate {RateId} to {NewRate}%",
            notification.LoanId, notification.RateId, notification.NewRate);
        return Task.CompletedTask;
    }
}
