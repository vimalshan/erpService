using MediatR;
using Microsoft.Extensions.Logging;
using HRService.Domain.Events;
using HRService.Infrastructure.MessageBroker;

namespace HRService.Application.Handlers;

/// <summary>
/// Handlers for domain events
/// </summary>
public class EmployeeCreatedEventHandler : INotificationHandler<EmployeeCreatedEvent>
{
    private readonly IMessageBrokerService _messageBrokerService;
    private readonly ILogger<EmployeeCreatedEventHandler> _logger;

    public EmployeeCreatedEventHandler(IMessageBrokerService messageBrokerService, ILogger<EmployeeCreatedEventHandler> logger)
    {
        _messageBrokerService = messageBrokerService;
        _logger = logger;
    }

    public async Task Handle(EmployeeCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Employee created event published for {EmployeeCode}", notification.EmployeeCode);

        // Publish to message broker
        await _messageBrokerService.PublishMessageAsync(
            "hr.events",
            "employee.created",
            notification,
            cancellationToken);
    }
}

public class LeaveApprovedEventHandler : INotificationHandler<LeaveApprovedEvent>
{
    private readonly IMessageBrokerService _messageBrokerService;
    private readonly ILogger<LeaveApprovedEventHandler> _logger;

    public LeaveApprovedEventHandler(IMessageBrokerService messageBrokerService, ILogger<LeaveApprovedEventHandler> logger)
    {
        _messageBrokerService = messageBrokerService;
        _logger = logger;
    }

    public async Task Handle(LeaveApprovedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Leave approved event for employee {EmployeeId}", notification.EmployeeId);

        await _messageBrokerService.PublishMessageAsync(
            "hr.events",
            "leave.approved",
            notification,
            cancellationToken);
    }
}

public class SalaryUpdatedEventHandler : INotificationHandler<SalaryUpdatedEvent>
{
    private readonly IMessageBrokerService _messageBrokerService;
    private readonly ILogger<SalaryUpdatedEventHandler> _logger;

    public SalaryUpdatedEventHandler(IMessageBrokerService messageBrokerService, ILogger<SalaryUpdatedEventHandler> logger)
    {
        _messageBrokerService = messageBrokerService;
        _logger = logger;
    }

    public async Task Handle(SalaryUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Salary updated event for employee {EmployeeId}", notification.EmployeeId);

        await _messageBrokerService.PublishMessageAsync(
            "hr.events",
            "salary.updated",
            notification,
            cancellationToken);
    }
}
