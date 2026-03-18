using MediatR;

namespace EmployeeManagement.Domain.Events;

public sealed record EmployeeCreatedEvent(long EmployeeId, string EmployeeNo, long CreatedBy) : INotification;

public sealed record EmployeePromotedEvent(long EmployeeId, long PromotionNo, long OldGradeId, long NewGradeId, long PromotedBy) : INotification;

public sealed record EmployeeTransferredEvent(long EmployeeId, long TransferId, string OldUnit, string NewUnit, long TransferredBy) : INotification;

public sealed record ProbationReviewedEvent(long EmployeeId, long ProbationId, char Status, string? Rating, long ReviewedBy) : INotification;

public sealed record EmployeeDeactivatedEvent(long EmployeeId, long DeactivatedBy) : INotification;
