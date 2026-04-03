namespace LoanTransaction.Domain.IntegrationEvents;

public abstract class IntegrationEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; set; } = DateTime.UtcNow;
}

public class LoanDisbursedIntegrationEvent : IntegrationEvent
{
    public long LoanNo { get; set; }
    public long ApplicationId { get; set; }
    public long EmployeeId { get; set; }
    public decimal PrincipalAmount { get; set; }
    public DateTime DisbursedAt { get; set; }
}

public class EmiPaidIntegrationEvent : IntegrationEvent
{
    public long LoanNo { get; set; }
    public long InstallmentNo { get; set; }
    public decimal PrincipalPaid { get; set; }
    public decimal InterestPaid { get; set; }
    public decimal PrincipalOutstanding { get; set; }
}

public class LoanClosedIntegrationEvent : IntegrationEvent
{
    public long LoanNo { get; set; }
    public long EmployeeId { get; set; }
    public string ClosureType { get; set; } = string.Empty;
    public DateTime ClosedAt { get; set; }
}
