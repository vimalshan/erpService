using MediatR;
using Microsoft.Extensions.Logging;
using LoanApplication.Domain.Events;
using LoanApplication.Domain.Interfaces;
using LoanApplication.Domain.IntegrationEvents;

namespace LoanApplication.Application.EventHandlers;

/// <summary>
/// Handles LoanApplicationCreatedEvent â€” logs and publishes integration event to message bus
/// </summary>
public class LoanApplicationCreatedEventHandler : INotificationHandler<LoanApplicationCreatedEvent>
{
    private readonly IMessageBus _messageBus;
    private readonly ILogger<LoanApplicationCreatedEventHandler> _logger;

    public LoanApplicationCreatedEventHandler(IMessageBus messageBus, ILogger<LoanApplicationCreatedEventHandler> logger)
    {
        _messageBus = messageBus;
        _logger = logger;
    }

    public async Task Handle(LoanApplicationCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Domain event received: LoanApplicationCreated. Id={Id}, EmployeeId={EmployeeId}, Amount={Amount}",
            notification.LoanApplicationId, notification.EmployeeId, notification.Amount);

        var integrationEvent = new LoanApplicationCreatedIntegrationEvent
        {
            LoanApplicationId = notification.LoanApplicationId,
            EmployeeId = notification.EmployeeId,
            LoanId = notification.LoanId,
            Amount = notification.Amount,
            Reason = notification.Reason,
            OccurredOn = notification.OccurredAt
        };

        await _messageBus.PublishAsync(integrationEvent, "loan.application.created", cancellationToken);
    }
}

/// <summary>
/// Handles LoanApplicationSubmittedEvent â€” notifies downstream systems
/// </summary>
public class LoanApplicationSubmittedEventHandler : INotificationHandler<LoanApplicationSubmittedEvent>
{
    private readonly ILogger<LoanApplicationSubmittedEventHandler> _logger;

    public LoanApplicationSubmittedEventHandler(ILogger<LoanApplicationSubmittedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(LoanApplicationSubmittedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Domain event received: LoanApplicationSubmitted. Id={Id}, EmployeeId={EmployeeId}, SubmittedAt={SubmittedAt}",
            notification.LoanApplicationId, notification.EmployeeId, notification.SubmittedAt);

        // Additional downstream processing (e.g., send approval email) can be added here
        return Task.CompletedTask;
    }
}

/// <summary>
/// Handles LoanApplicationApprovedEvent â€” publishes integration event and triggers notifications
/// </summary>
public class LoanApplicationApprovedEventHandler : INotificationHandler<LoanApplicationApprovedEvent>
{
    private readonly IMessageBus _messageBus;
    private readonly ILogger<LoanApplicationApprovedEventHandler> _logger;

    public LoanApplicationApprovedEventHandler(IMessageBus messageBus, ILogger<LoanApplicationApprovedEventHandler> logger)
    {
        _messageBus = messageBus;
        _logger = logger;
    }

    public async Task Handle(LoanApplicationApprovedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Domain event received: LoanApplicationApproved. Id={Id}, ApprovedBy={ApprovedBy}, ApprovedAt={ApprovedAt}",
            notification.LoanApplicationId, notification.ApprovedBy, notification.ApprovedAt);

        var integrationEvent = new LoanApplicationApprovedIntegrationEvent
        {
            LoanApplicationId = notification.LoanApplicationId,
            ApprovedBy = notification.ApprovedBy,
            ApprovedAt = notification.ApprovedAt,
            Remarks = notification.Remarks,
            OccurredOn = notification.OccurredAt
        };

        await _messageBus.PublishAsync(integrationEvent, "loan.application.approved", cancellationToken);
    }
}

/// <summary>
/// Handles LoanApplicationRejectedEvent â€” publishes integration event
/// </summary>
public class LoanApplicationRejectedEventHandler : INotificationHandler<LoanApplicationRejectedEvent>
{
    private readonly IMessageBus _messageBus;
    private readonly ILogger<LoanApplicationRejectedEventHandler> _logger;

    public LoanApplicationRejectedEventHandler(IMessageBus messageBus, ILogger<LoanApplicationRejectedEventHandler> logger)
    {
        _messageBus = messageBus;
        _logger = logger;
    }

    public async Task Handle(LoanApplicationRejectedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Domain event received: LoanApplicationRejected. Id={Id}, RejectedBy={RejectedBy}, Remarks={Remarks}",
            notification.LoanApplicationId, notification.RejectedBy, notification.Remarks);

        var integrationEvent = new LoanApplicationRejectedIntegrationEvent
        {
            LoanApplicationId = notification.LoanApplicationId,
            RejectedBy = notification.RejectedBy,
            RejectedAt = notification.RejectedAt,
            Remarks = notification.Remarks,
            OccurredOn = notification.OccurredAt
        };

        await _messageBus.PublishAsync(integrationEvent, "loan.application.rejected", cancellationToken);
    }
}

/// <summary>
/// Handles LoanApplicationDisbursedEvent â€” publishes integration event for payroll / finance systems
/// </summary>
public class LoanApplicationDisbursedEventHandler : INotificationHandler<LoanApplicationDisbursedEvent>
{
    private readonly IMessageBus _messageBus;
    private readonly ILogger<LoanApplicationDisbursedEventHandler> _logger;

    public LoanApplicationDisbursedEventHandler(IMessageBus messageBus, ILogger<LoanApplicationDisbursedEventHandler> logger)
    {
        _messageBus = messageBus;
        _logger = logger;
    }

    public async Task Handle(LoanApplicationDisbursedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Domain event received: LoanApplicationDisbursed. Id={Id}, Amount={Amount}, DisbursedAt={DisbursedAt}",
            notification.LoanApplicationId, notification.DisbursedAmount, notification.DisbursedAt);

        var integrationEvent = new LoanDisbursedIntegrationEvent
        {
            LoanApplicationId = notification.LoanApplicationId,
            DisbursedAmount = notification.DisbursedAmount,
            DisbursedAt = notification.DisbursedAt,
            OccurredOn = notification.OccurredAt
        };

        await _messageBus.PublishAsync(integrationEvent, "loan.disbursed", cancellationToken);
    }
}
