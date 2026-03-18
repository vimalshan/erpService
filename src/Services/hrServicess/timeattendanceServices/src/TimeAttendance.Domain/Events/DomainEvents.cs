using TimeAttendance.Domain.Common;

namespace TimeAttendance.Domain.Events;

public record AbsenteeismDetailCreatedEvent(
    long AbsenteeismId,
    long UnitId,
    int Year,
    int Month) : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public record AbsenteeismDetailUpdatedEvent(
    long AbsenteeismId,
    long UnitId) : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public record AbsenteeismDetailDeletedEvent(
    long AbsenteeismId) : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public record AbsenteeismMisCreatedEvent(
    long MisId,
    int? UnitId,
    string? Month) : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public record AbsenteeismMisUpdatedEvent(
    long MisId,
    int? UnitId,
    string? Month) : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public record AbsenteeismMisDeletedEvent(
    long MisId) : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);
