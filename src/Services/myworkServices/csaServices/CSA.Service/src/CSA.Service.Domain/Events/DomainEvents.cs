using MediatR;

namespace CSA.Service.Domain.Events;

public record ControlCreatedEvent(long ControlId, string Title) : INotification;
public record ControlUpdatedEvent(long ControlId, string Title) : INotification;
public record ControlDeletedEvent(long ControlId) : INotification;

public record SurveyCreatedEvent(long SurveyId, string Title) : INotification;
public record SurveyCompletedEvent(long SurveyId) : INotification;

public record SurveyFeedbackSubmittedEvent(long FeedbackId, long SurveyQuestionId, char Status) : INotification;
public record SurveyFeedbackApprovedEvent(long FeedbackId, long ApprovedBy) : INotification;
public record SurveyFeedbackRejectedEvent(long FeedbackId, long RejectedBy) : INotification;

public record EvidenceUploadedEvent(long EvidenceId, long ControlId, string? Name) : INotification;

public record UnitMappingCreatedEvent(long MapId, long ControlId, long UnitId) : INotification;
