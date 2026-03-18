using MasterService.Domain.Events;
using MasterService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MasterService.Application.EventHandlers;

public sealed class SkillCreatedEventHandler(ILogger<SkillCreatedEventHandler> logger)
    : INotificationHandler<SkillCreatedEvent>
{
    public Task Handle(SkillCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event: SkillCreated - Code={Code}, Name={Name}, Type={Type}",
            notification.SkillCode, notification.SkillName, notification.SkillType);
        return Task.CompletedTask;
    }
}

public sealed class TrainingProviderCreatedEventHandler(ILogger<TrainingProviderCreatedEventHandler> logger)
    : INotificationHandler<TrainingProviderCreatedEvent>
{
    public Task Handle(TrainingProviderCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event: TrainingProviderCreated - Code={Code}, Name={Name}",
            notification.TrainingCode, notification.TrainingName);
        return Task.CompletedTask;
    }
}

public sealed class JobCreatedEventHandler(ILogger<JobCreatedEventHandler> logger)
    : INotificationHandler<JobCreatedEvent>
{
    public Task Handle(JobCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event: JobCreated - Code={Code}, Name={Name}, Category={Category}",
            notification.JobCode, notification.JobName, notification.CategoryCode);
        return Task.CompletedTask;
    }
}
