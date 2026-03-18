using CompensationBenefits.Application.Common.Behaviours;
using CompensationBenefits.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CompensationBenefits.Application.EventHandlers;

public class SalaryCreatedDomainEventHandler(ILogger<SalaryCreatedDomainEventHandler> logger)
    : INotificationHandler<SalaryCreatedDomainEvent>
{
    public Task Handle(SalaryCreatedDomainEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Salary {SalaryId} created with CTC {CTC}", notification.SalaryId, notification.CtcAmount);
        return Task.CompletedTask;
    }
}

public class SalaryStructureCreatedDomainEventHandler(ILogger<SalaryStructureCreatedDomainEventHandler> logger)
    : INotificationHandler<SalaryStructureCreatedDomainEvent>
{
    public Task Handle(SalaryStructureCreatedDomainEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Salary Structure {Id} '{Name}' created", notification.StructureId, notification.Name);
        return Task.CompletedTask;
    }
}

public class MediclaimUpdatedDomainEventHandler(ILogger<MediclaimUpdatedDomainEventHandler> logger)
    : INotificationHandler<MediclaimUpdatedDomainEvent>
{
    public Task Handle(MediclaimUpdatedDomainEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Mediclaim {Id} updated: {RefName}", notification.MediclaimId, notification.RefName);
        return Task.CompletedTask;
    }
}
