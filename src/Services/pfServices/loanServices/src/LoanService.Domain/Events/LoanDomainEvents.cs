using LoanService.Domain.Common;

namespace LoanService.Domain.Events;

public record LoanCreatedEvent(long LoanNo, long MemberId, decimal Amount) : DomainEvent;
public record LoanApprovedEvent(long LoanNo, DateTime ApprovalDate) : DomainEvent;
public record LoanClosedEvent(long LoanNo, DateTime ClosureDate) : DomainEvent;
public record RepaymentMadeEvent(long LoanNo, long RepaymentId, decimal PaidAmount) : DomainEvent;
