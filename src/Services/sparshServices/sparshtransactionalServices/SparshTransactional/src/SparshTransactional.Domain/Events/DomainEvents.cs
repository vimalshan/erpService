using SparshTransactional.Domain.Common;
using SparshTransactional.Domain.Entities;

namespace SparshTransactional.Domain.Events;

public sealed record ScholarshipCreatedEvent(ScholarshipMaster Scholarship) : IDomainEvent;

public sealed record ScholarshipDeactivatedEvent(ScholarshipMaster Scholarship, long DeactivatedBy) : IDomainEvent;

public sealed record ApplicationSubmittedEvent(ScholarshipApplication Application) : IDomainEvent;

public sealed record ApplicationApprovedEvent(ScholarshipApplication Application, long ApprovedBy) : IDomainEvent;

public sealed record ApplicationRejectedEvent(ScholarshipApplication Application, long RejectedBy, string? Reason) : IDomainEvent;

public sealed record DisbursementCreatedEvent(ScholarshipDisbursement Disbursement) : IDomainEvent;

public sealed record DisbursementCompletedEvent(ScholarshipDisbursement Disbursement) : IDomainEvent;
