using MasterService.Domain.Events;
using MasterService.Infrastructure.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MasterService.Infrastructure.Messaging.Consumers;

/// <summary>Publishes SkillCreated domain events to RabbitMQ for downstream consumers.</summary>
public sealed class SkillCreatedRabbitMqHandler(IMessagePublisher publisher, ILogger<SkillCreatedRabbitMqHandler> logger)
    : INotificationHandler<SkillCreatedEvent>
{
    public async Task Handle(SkillCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Publishing SkillCreatedEvent to RabbitMQ for SkillCode={Code}", notification.SkillCode);
        await publisher.PublishAsync(
            exchange: "master-service",
            routingKey: "skill.created",
            message: new { notification.SkillCode, notification.SkillName, notification.SkillType });
    }
}

/// <summary>Publishes TrainingProviderCreated domain events to RabbitMQ.</summary>
public sealed class TrainingProviderCreatedRabbitMqHandler(IMessagePublisher publisher, ILogger<TrainingProviderCreatedRabbitMqHandler> logger)
    : INotificationHandler<TrainingProviderCreatedEvent>
{
    public async Task Handle(TrainingProviderCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Publishing TrainingProviderCreatedEvent to RabbitMQ for Code={Code}", notification.TrainingCode);
        await publisher.PublishAsync(
            exchange: "master-service",
            routingKey: "training.created",
            message: new { notification.TrainingCode, notification.TrainingName });
    }
}
