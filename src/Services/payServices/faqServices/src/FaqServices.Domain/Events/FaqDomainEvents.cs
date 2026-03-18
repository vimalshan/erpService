using FaqServices.Domain.Common;

namespace FaqServices.Domain.Events;

public sealed record FaqGradeCreatedEvent(string GradeId, string GradeName) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record FaqGradeUpdatedEvent(string GradeId, string GradeName) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record FaqQuestionCreatedEvent(string QuestionId, string GradeId, string QuestionText) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record FaqQuestionUpdatedEvent(string QuestionId, string QuestionText) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record FaqAnswerCreatedEvent(string AnswerId, string QuestionId, string AnswerText) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record FaqAnswerUpdatedEvent(string AnswerId, string AnswerText) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
