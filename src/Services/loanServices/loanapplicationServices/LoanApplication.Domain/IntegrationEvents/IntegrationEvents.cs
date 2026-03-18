namespace LoanApplication.Domain.IntegrationEvents;

/// <summary>
/// Base class for integration events published to the message bus
/// </summary>
public abstract class IntegrationEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Published when a new loan application is created
/// </summary>
public class LoanApplicationCreatedIntegrationEvent : IntegrationEvent
{
    public long LoanApplicationId { get; set; }
    public long EmployeeId { get; set; }
    public long LoanId { get; set; }
    public decimal Amount { get; set; }
    public string Reason { get; set; } = string.Empty;
}

/// <summary>
/// Published when a loan application is approved
/// </summary>
public class LoanApplicationApprovedIntegrationEvent : IntegrationEvent
{
    public long LoanApplicationId { get; set; }
    public long ApprovedBy { get; set; }
    public DateTime ApprovedAt { get; set; }
    public string? Remarks { get; set; }
}

/// <summary>
/// Published when a loan application is rejected
/// </summary>
public class LoanApplicationRejectedIntegrationEvent : IntegrationEvent
{
    public long LoanApplicationId { get; set; }
    public long RejectedBy { get; set; }
    public DateTime RejectedAt { get; set; }
    public string? Remarks { get; set; }
}

/// <summary>
/// Published when a loan is disbursed — consumed by payroll/finance systems
/// </summary>
public class LoanDisbursedIntegrationEvent : IntegrationEvent
{
    public long LoanApplicationId { get; set; }
    public decimal DisbursedAmount { get; set; }
    public DateTime DisbursedAt { get; set; }
}
