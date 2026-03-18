using GSTComplianceService.Domain.Common;

namespace GSTComplianceService.Domain.Events;

public sealed record GstRegisteredEvent(
    long GstId,
    string PanNo,
    DateTime OccurredOn) : IDomainEvent;

public sealed record GstStatusChangedEvent(
    long GstId,
    char? PreviousStatus,
    char NewStatus,
    DateTime OccurredOn) : IDomainEvent;

public sealed record GstHsnAddedEvent(
    long GstId,
    long HsnId,
    string? HsnCode,
    DateTime OccurredOn) : IDomainEvent;

public sealed record GstStateRegistrationAddedEvent(
    long GstId,
    long TinId,
    string State,
    string? GstinNo,
    DateTime OccurredOn) : IDomainEvent;
