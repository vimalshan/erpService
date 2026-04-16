using MediatR;

namespace FindingsAPI.Gateway.Domain.Events;

public record FindingCreatedEvent(int FindingId, string FindingNumber, int AuditId, string Title, string FindingType) : INotification;
public record FindingStatusChangedEvent(int FindingId, int OldStatusId, int NewStatusId) : INotification;
public record FindingClosedEvent(int FindingId, DateTime ClosedDate, int? ClosedBy) : INotification;
public record FindingAssignedEvent(int FindingId, int? AssignedTo) : INotification;
public record FindingResponseAddedEvent(int FindingResponseId, int FindingId, string ResponseType) : INotification;
public record FindingVerifiedEvent(int FindingId, int? VerifiedBy, DateTime VerificationDate) : INotification;
