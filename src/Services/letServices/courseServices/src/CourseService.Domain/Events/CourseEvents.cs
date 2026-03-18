using CourseService.Domain.Common;

namespace CourseService.Domain.Events;

public sealed record CourseCreatedEvent(long CourseId, string CourseDescription) : DomainEvent;

public sealed record CourseCancelledEvent(long CourseId, string? Reason) : DomainEvent;

public sealed record CourseScheduleAddedEvent(long CourseId, long SerialNumber, DateTime ScheduleDate) : DomainEvent;

public sealed record ParticipantRegisteredEvent(long CourseId, string UserCode, DateTime EnrollmentDate) : DomainEvent;

public sealed record ParticipantCancelledEvent(long CourseId, string UserCode, DateTime CancellationDate) : DomainEvent;

public sealed record AttendanceUpdatedEvent(long CourseId, string UserCode, char AttendanceStatus) : DomainEvent;

public sealed record CourseRatingsUpdatedEvent(long CourseId, decimal? TrainerRating, decimal? ContentRating) : DomainEvent;
