using MediatR;
using Microsoft.Extensions.Logging;
using TrainingDevelopment.Domain.Events;
using TrainingDevelopment.Infrastructure.Messaging;

namespace TrainingDevelopment.Infrastructure.DomainEventHandlers;

public class TrainingCreatedEventHandler : INotificationHandler<TrainingCreatedEvent>
{
    private readonly ILogger<TrainingCreatedEventHandler> _logger;
    private readonly RabbitMQProducer _producer;

    public TrainingCreatedEventHandler(ILogger<TrainingCreatedEventHandler> logger, RabbitMQProducer producer)
    {
        _logger = logger;
        _producer = producer;
    }

    public async Task Handle(TrainingCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Training Created: ID={Id}, Employee={EmployeeId}",
            notification.Training.Id, notification.Training.EmployeeSysId);

        try
        {
            await _producer.PublishAsync("training.created", new
            {
                notification.Training.Id,
                notification.Training.EmployeeSysId,
                notification.Training.FinancialYear,
                notification.OccurredOn
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish TrainingCreatedEvent to RabbitMQ");
        }
    }
}

public class TrainingCompletedEventHandler : INotificationHandler<TrainingCompletedEvent>
{
    private readonly ILogger<TrainingCompletedEventHandler> _logger;
    private readonly RabbitMQProducer _producer;

    public TrainingCompletedEventHandler(ILogger<TrainingCompletedEventHandler> logger, RabbitMQProducer producer)
    {
        _logger = logger;
        _producer = producer;
    }

    public async Task Handle(TrainingCompletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Training Completed: ID={Id}", notification.Training.Id);

        try
        {
            await _producer.PublishAsync("training.completed", new
            {
                notification.Training.Id,
                notification.Training.ActualFrom,
                notification.Training.ActualTo,
                notification.Training.Cost,
                notification.OccurredOn
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish TrainingCompletedEvent to RabbitMQ");
        }
    }
}

public class TrainingDroppedEventHandler : INotificationHandler<TrainingDroppedEvent>
{
    private readonly ILogger<TrainingDroppedEventHandler> _logger;
    private readonly RabbitMQProducer _producer;

    public TrainingDroppedEventHandler(ILogger<TrainingDroppedEventHandler> logger, RabbitMQProducer producer)
    {
        _logger = logger;
        _producer = producer;
    }

    public async Task Handle(TrainingDroppedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Training Dropped: ID={Id}, Remarks={Remarks}",
            notification.Training.Id, notification.Training.DroppedRemarks);

        try
        {
            await _producer.PublishAsync("training.dropped", new
            {
                notification.Training.Id,
                notification.Training.DroppedRemarks,
                notification.OccurredOn
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish TrainingDroppedEvent to RabbitMQ");
        }
    }
}
