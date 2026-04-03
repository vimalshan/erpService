using LoanTransaction.Domain.Common;

namespace LoanTransaction.Domain.Events;

public class LoanDisbursedEvent : DomainEvent
{
    public long LoanNo { get; set; }
    public long ApplicationId { get; set; }
    public long EmployeeId { get; set; }
    public decimal PrincipalAmount { get; set; }
    public DateTime DisbursedAt { get; set; }
}

public class EmiPaymentRecordedEvent : DomainEvent
{
    public long LoanNo { get; set; }
    public long InstallmentId { get; set; }
    public long InstallmentNo { get; set; }
    public decimal PrincipalPaid { get; set; }
    public decimal InterestPaid { get; set; }
    public decimal PrincipalOutstanding { get; set; }
    public long PaidBy { get; set; }
    public DateTime PaidAt { get; set; }
}

public class LoanClosedEvent : DomainEvent
{
    public long LoanNo { get; set; }
    public long EmployeeId { get; set; }
    public string ClosureType { get; set; } = string.Empty;
    public DateTime ClosedAt { get; set; }
}

public class LoanAdjustedEvent : DomainEvent
{
    public long LoanNo { get; set; }
    public long AdjLoanNo { get; set; }
    public decimal AdjPrincipalAmount { get; set; }
    public decimal AdjInterestAmount { get; set; }
}

public class EmiScheduleCreatedEvent : DomainEvent
{
    public long LoanNo { get; set; }
    public long EmployeeId { get; set; }
    public int NumberOfInstallments { get; set; }
    public decimal EmiAmount { get; set; }
    public DateTime FirstInstallmentDate { get; set; }
}
