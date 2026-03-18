using LoanApplication.Domain.Common;

namespace LoanApplication.Domain.Events;

/// <summary>
/// Raised when a loan application is created
/// </summary>
public class LoanApplicationCreatedEvent : DomainEvent
{
    public long LoanApplicationId { get; set; }
    public long EmployeeId { get; set; }
    public long LoanId { get; set; }
    public decimal Amount { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Raised when a loan application is submitted
/// </summary>
public class LoanApplicationSubmittedEvent : DomainEvent
{
    public long LoanApplicationId { get; set; }
    public long EmployeeId { get; set; }
    public DateTime SubmittedAt { get; set; }
}

/// <summary>
/// Raised when a loan application is approved
/// </summary>
public class LoanApplicationApprovedEvent : DomainEvent
{
    public long LoanApplicationId { get; set; }
    public long ApprovedBy { get; set; }
    public DateTime ApprovedAt { get; set; }
    public string? Remarks { get; set; }
}

/// <summary>
/// Raised when a loan application is rejected
/// </summary>
public class LoanApplicationRejectedEvent : DomainEvent
{
    public long LoanApplicationId { get; set; }
    public long RejectedBy { get; set; }
    public DateTime RejectedAt { get; set; }
    public string? Remarks { get; set; }
}

/// <summary>
/// Raised when a loan application is disbursed
/// </summary>
public class LoanApplicationDisbursedEvent : DomainEvent
{
    public long LoanApplicationId { get; set; }
    public decimal DisbursedAmount { get; set; }
    public DateTime DisbursedAt { get; set; }
}
