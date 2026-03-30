using CompensationBenefits.Application.Contracts;
using CompensationBenefits.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CompensationBenefits.Application.EventHandlers;

public class SalaryCreatedDomainEventHandler(
    IMessagePublisher publisher,
    ILogger<SalaryCreatedDomainEventHandler> logger)
    : INotificationHandler<SalaryCreatedDomainEvent>
{
    public async Task Handle(SalaryCreatedDomainEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Salary {SalaryId} created with CTC {CTC}", notification.SalaryId, notification.CtcAmount);
        await publisher.PublishAsync(
            "compensation.events",
            "salary.created",
            new { notification.SalaryId, notification.CtcAmount, OccurredAt = DateTime.UtcNow });
    }
}

public class SalaryStructureCreatedDomainEventHandler(
    IMessagePublisher publisher,
    ILogger<SalaryStructureCreatedDomainEventHandler> logger)
    : INotificationHandler<SalaryStructureCreatedDomainEvent>
{
    public async Task Handle(SalaryStructureCreatedDomainEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Salary Structure {Id} '{Name}' created", notification.StructureId, notification.Name);
        await publisher.PublishAsync(
            "compensation.events",
            "salary-structure.created",
            new { notification.StructureId, notification.Name, OccurredAt = DateTime.UtcNow });
    }
}

public class MediclaimUpdatedDomainEventHandler(
    IMessagePublisher publisher,
    ILogger<MediclaimUpdatedDomainEventHandler> logger)
    : INotificationHandler<MediclaimUpdatedDomainEvent>
{
    public async Task Handle(MediclaimUpdatedDomainEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Mediclaim {Id} updated: {RefName}", notification.MediclaimId, notification.RefName);
        await publisher.PublishAsync(
            "compensation.events",
            "mediclaim.updated",
            new { notification.MediclaimId, notification.RefName, OccurredAt = DateTime.UtcNow });
    }
}
