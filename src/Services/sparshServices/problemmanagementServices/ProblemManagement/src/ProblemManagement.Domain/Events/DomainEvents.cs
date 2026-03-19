using ProblemManagement.Domain.Common;
using ProblemManagement.Domain.Entities;

namespace ProblemManagement.Domain.Events;

public sealed record ProblemCreatedEvent(ProblemMain Problem) : IDomainEvent;

public sealed record ProblemApprovedEvent(ProblemMain Problem, long ApprovedBy, string? Reason) : IDomainEvent;

public sealed record ProblemRejectedEvent(ProblemMain Problem, long RejectedBy, string? Reason) : IDomainEvent;

public sealed record SolutionAddedEvent(ProblemMain Problem, ProblemSolution Solution) : IDomainEvent;

public sealed record SolutionApprovedEvent(ProblemSolution Solution, long ApprovedBy) : IDomainEvent;

public sealed record SolutionCommentAddedEvent(SolutionComment Comment) : IDomainEvent;
