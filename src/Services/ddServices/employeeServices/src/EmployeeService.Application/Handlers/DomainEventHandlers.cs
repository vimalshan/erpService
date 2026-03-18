using EmployeeService.Application.Abstractions;
using EmployeeService.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using EmployeeService.Shared.Messaging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeService.Application.Handlers.DomainEvents
{
    /// <summary>
    /// Handles employee lifecycle domain events.
    /// </summary>
    public class EmployeeLifecycleDomainEventHandler :
        INotificationHandler<EmployeeCreatedEvent>,
        INotificationHandler<EmployeePersonalInfoUpdatedEvent>,
        INotificationHandler<EmployeeContactInfoUpdatedEvent>,
        INotificationHandler<EmployeeReactivatedEvent>,
        INotificationHandler<EmployeeTransferredEvent>
    {
        private readonly IEmployeeEventPublisher _publisher;
        private readonly ILogger<EmployeeLifecycleDomainEventHandler> _logger;

        public EmployeeLifecycleDomainEventHandler(
            IEmployeeEventPublisher publisher,
            ILogger<EmployeeLifecycleDomainEventHandler> logger)
        {
            _publisher = publisher;
            _logger = logger;
        }

        public Task Handle(EmployeeCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain event: Employee created {EmployeeNumber} - {EmployeeName}", notification.EmployeeNumber, notification.EmployeeName);
            return _publisher.PublishAsync(new EmployeeEventMessage
            {
                EventType = nameof(EmployeeCreatedEvent),
                EmployeeId = notification.EmployeeId,
                OccurredOn = notification.OccurredOn,
                Description = $"Employee created: {notification.EmployeeNumber} - {notification.EmployeeName}",
                Attributes = new Dictionary<string, string>
                {
                    ["employeeNumber"] = notification.EmployeeNumber,
                    ["employeeName"] = notification.EmployeeName,
                    ["joiningDate"] = notification.JoiningDate.ToString("O")
                }
            }, cancellationToken);
        }

        public Task Handle(EmployeePersonalInfoUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain event: Personal info updated for employee {EmployeeId}", notification.EmployeeId);
            return _publisher.PublishAsync(new EmployeeEventMessage
            {
                EventType = nameof(EmployeePersonalInfoUpdatedEvent),
                EmployeeId = notification.EmployeeId,
                OccurredOn = notification.OccurredOn,
                Description = notification.UpdateDetails,
                Attributes = new Dictionary<string, string>
                {
                    ["firstName"] = notification.FirstName,
                    ["lastName"] = notification.LastName
                }
            }, cancellationToken);
        }

        public Task Handle(EmployeeContactInfoUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain event: Contact info updated for employee {EmployeeId}", notification.EmployeeId);
            return _publisher.PublishAsync(new EmployeeEventMessage
            {
                EventType = nameof(EmployeeContactInfoUpdatedEvent),
                EmployeeId = notification.EmployeeId,
                OccurredOn = notification.OccurredOn,
                Description = $"Contact info updated to {notification.Email}",
                Attributes = new Dictionary<string, string>
                {
                    ["email"] = notification.Email,
                    ["phone"] = notification.Phone
                }
            }, cancellationToken);
        }

        public Task Handle(EmployeeReactivatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain event: Employee reactivated {EmployeeId}", notification.EmployeeId);
            return _publisher.PublishAsync(new EmployeeEventMessage
            {
                EventType = nameof(EmployeeReactivatedEvent),
                EmployeeId = notification.EmployeeId,
                OccurredOn = notification.OccurredOn,
                Description = $"Employee reactivated on {notification.ReactivationDate:yyyy-MM-dd}",
                Attributes = new Dictionary<string, string>
                {
                    ["reactivationDate"] = notification.ReactivationDate.ToString("O")
                }
            }, cancellationToken);
        }

        public Task Handle(EmployeeTransferredEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain event: Employee transferred {EmployeeId} from {FromUnit} to {ToUnit}", notification.EmployeeId, notification.FromUnit, notification.ToUnit);
            return _publisher.PublishAsync(new EmployeeEventMessage
            {
                EventType = nameof(EmployeeTransferredEvent),
                EmployeeId = notification.EmployeeId,
                OccurredOn = notification.OccurredOn,
                Description = $"Employee transferred from {notification.FromUnit} to {notification.ToUnit}",
                Attributes = new Dictionary<string, string>
                {
                    ["fromUnit"] = notification.FromUnit,
                    ["toUnit"] = notification.ToUnit,
                    ["fromUnitId"] = notification.FromUnitId.ToString(),
                    ["toUnitId"] = notification.ToUnitId.ToString(),
                    ["transferDate"] = notification.TransferDate.ToString("O")
                }
            }, cancellationToken);
        }
    }

    /// <summary>
    /// Handles employee compensation and status domain events.
    /// </summary>
    public class EmployeeStatusDomainEventHandler :
        INotificationHandler<EmployeeSalaryUpdatedEvent>,
        INotificationHandler<EmployeeGradeUpdatedEvent>,
        INotificationHandler<EmployeePromotedEvent>,
        INotificationHandler<EmployeeTerminatedEvent>
    {
        private readonly IEmployeeEventPublisher _publisher;
        private readonly ILogger<EmployeeStatusDomainEventHandler> _logger;

        public EmployeeStatusDomainEventHandler(
            IEmployeeEventPublisher publisher,
            ILogger<EmployeeStatusDomainEventHandler> logger)
        {
            _publisher = publisher;
            _logger = logger;
        }

        public Task Handle(EmployeeSalaryUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain event: Salary updated for employee {EmployeeId}: {OldSalary} -> {NewSalary}", notification.EmployeeId, notification.OldBasicSalary, notification.NewBasicSalary);
            return _publisher.PublishAsync(new EmployeeEventMessage
            {
                EventType = nameof(EmployeeSalaryUpdatedEvent),
                EmployeeId = notification.EmployeeId,
                OccurredOn = notification.OccurredOn,
                Description = $"Salary updated from {notification.OldBasicSalary} to {notification.NewBasicSalary}",
                Attributes = new Dictionary<string, string>
                {
                    ["oldBasicSalary"] = notification.OldBasicSalary.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    ["newBasicSalary"] = notification.NewBasicSalary.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    ["effectiveDate"] = notification.EffectiveDate.ToString("O")
                }
            }, cancellationToken);
        }

        public Task Handle(EmployeeGradeUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain event: Grade updated for employee {EmployeeId}: {OldGrade} -> {NewGrade}", notification.EmployeeId, notification.OldGrade, notification.NewGrade);
            return _publisher.PublishAsync(new EmployeeEventMessage
            {
                EventType = nameof(EmployeeGradeUpdatedEvent),
                EmployeeId = notification.EmployeeId,
                OccurredOn = notification.OccurredOn,
                Description = $"Grade updated from {notification.OldGrade} to {notification.NewGrade}",
                Attributes = new Dictionary<string, string>
                {
                    ["oldGrade"] = notification.OldGrade,
                    ["newGrade"] = notification.NewGrade,
                    ["effectiveDate"] = notification.EffectiveDate.ToString("O")
                }
            }, cancellationToken);
        }

        public Task Handle(EmployeePromotedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain event: Employee promoted {EmployeeId}", notification.EmployeeId);
            return _publisher.PublishAsync(new EmployeeEventMessage
            {
                EventType = nameof(EmployeePromotedEvent),
                EmployeeId = notification.EmployeeId,
                OccurredOn = notification.OccurredOn,
                Description = $"Employee promoted from {notification.FromDesignation} to {notification.ToDesignation}",
                Attributes = new Dictionary<string, string>
                {
                    ["fromDesignation"] = notification.FromDesignation,
                    ["toDesignation"] = notification.ToDesignation,
                    ["fromGrade"] = notification.FromGrade,
                    ["toGrade"] = notification.ToGrade,
                    ["promotionDate"] = notification.PromotionDate.ToString("O")
                }
            }, cancellationToken);
        }

        public Task Handle(EmployeeTerminatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain event: Employee terminated {EmployeeId}", notification.EmployeeId);
            return _publisher.PublishAsync(new EmployeeEventMessage
            {
                EventType = nameof(EmployeeTerminatedEvent),
                EmployeeId = notification.EmployeeId,
                OccurredOn = notification.OccurredOn,
                Description = notification.TerminationReason,
                Attributes = new Dictionary<string, string>
                {
                    ["terminationFlag"] = notification.TerminationFlag,
                    ["exitDate"] = notification.ExitDate.ToString("O")
                }
            }, cancellationToken);
        }
    }
}