using CSA.Service.Application.Interfaces;
using CSA.Service.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CSA.Service.Application.Common;

public class ControlCreatedEventHandler(IMessagePublisher publisher, ILogger<ControlCreatedEventHandler> logger)
    : INotificationHandler<ControlCreatedEvent>
{
    public async Task Handle(ControlCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Control {ControlId} created - {Title}", notification.ControlId, notification.Title);
        await publisher.PublishAsync("csa.events", "control.created", notification, ct);
    }
}

public class ControlUpdatedEventHandler(IMessagePublisher publisher, ILogger<ControlUpdatedEventHandler> logger)
    : INotificationHandler<ControlUpdatedEvent>
{
    public async Task Handle(ControlUpdatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Control {ControlId} updated - {Title}", notification.ControlId, notification.Title);
        await publisher.PublishAsync("csa.events", "control.updated", notification, ct);
    }
}

public class ControlDeletedEventHandler(IMessagePublisher publisher, ILogger<ControlDeletedEventHandler> logger)
    : INotificationHandler<ControlDeletedEvent>
{
    public async Task Handle(ControlDeletedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Control {ControlId} deleted", notification.ControlId);
        await publisher.PublishAsync("csa.events", "control.deleted", notification, ct);
    }
}

public class SurveyCreatedEventHandler(IMessagePublisher publisher, ILogger<SurveyCreatedEventHandler> logger)
    : INotificationHandler<SurveyCreatedEvent>
{
    public async Task Handle(SurveyCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Survey {SurveyId} created - {Title}", notification.SurveyId, notification.Title);
        await publisher.PublishAsync("csa.events", "survey.created", notification, ct);
    }
}

public class SurveyFeedbackSubmittedEventHandler(IMessagePublisher publisher, ILogger<SurveyFeedbackSubmittedEventHandler> logger)
    : INotificationHandler<SurveyFeedbackSubmittedEvent>
{
    public async Task Handle(SurveyFeedbackSubmittedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Feedback {FeedbackId} submitted for question {QuestionId}", notification.FeedbackId, notification.SurveyQuestionId);
        await publisher.PublishAsync("csa.events", "survey.feedback.submitted", notification, ct);
    }
}

public class SurveyFeedbackApprovedEventHandler(IMessagePublisher publisher, ILogger<SurveyFeedbackApprovedEventHandler> logger)
    : INotificationHandler<SurveyFeedbackApprovedEvent>
{
    public async Task Handle(SurveyFeedbackApprovedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Feedback {FeedbackId} approved by {ApprovedBy}", notification.FeedbackId, notification.ApprovedBy);
        await publisher.PublishAsync("csa.events", "survey.feedback.approved", notification, ct);
    }
}

public class EvidenceUploadedEventHandler(IMessagePublisher publisher, ILogger<EvidenceUploadedEventHandler> logger)
    : INotificationHandler<EvidenceUploadedEvent>
{
    public async Task Handle(EvidenceUploadedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Evidence {EvidenceId} uploaded for control {ControlId}", notification.EvidenceId, notification.ControlId);
        await publisher.PublishAsync("csa.events", "evidence.uploaded", notification, ct);
    }
}
