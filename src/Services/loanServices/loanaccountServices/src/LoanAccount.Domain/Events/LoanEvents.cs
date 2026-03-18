using LoanAccount.Domain.Common;

namespace LoanAccount.Domain.Events;

/// <summary>
/// Raised when a loan is created
/// </summary>
public sealed class LoanCreatedEvent : DomainEvent
{
    public long LoanNo { get; }
    public long ApplicationId { get; }
    public long EmployeeId { get; }
    public decimal PrincipalAmount { get; }
    public DateTime LoanDate { get; }

    public LoanCreatedEvent(long loanNo, long applicationId, long employeeId, decimal principalAmount, DateTime loanDate)
    {
        AggregateId = loanNo;
        LoanNo = loanNo;
        ApplicationId = applicationId;
        EmployeeId = employeeId;
        PrincipalAmount = principalAmount;
        LoanDate = loanDate;
    }
}

/// <summary>
/// Raised when a loan is approved
/// </summary>
public sealed class LoanApprovedEvent : DomainEvent
{
    public long LoanNo { get; }
    public decimal InterestRate { get; }
    public DateTime ApprovedOn { get; }
    public long ApprovedBy { get; }

    public LoanApprovedEvent(long loanNo, decimal interestRate, DateTime approvedOn, long approvedBy)
    {
        AggregateId = loanNo;
        LoanNo = loanNo;
        InterestRate = interestRate;
        ApprovedOn = approvedOn;
        ApprovedBy = approvedBy;
    }
}

/// <summary>
/// Raised when a loan is disbursed
/// </summary>
public sealed class LoanDisbursedEvent : DomainEvent
{
    public long LoanNo { get; }
    public decimal DisbursedAmount { get; }
    public DateTime DisbursedOn { get; }

    public LoanDisbursedEvent(long loanNo, decimal disbursedAmount, DateTime disbursedOn)
    {
        AggregateId = loanNo;
        LoanNo = loanNo;
        DisbursedAmount = disbursedAmount;
        DisbursedOn = disbursedOn;
    }
}

/// <summary>
/// Raised when an EMI payment is recorded
/// </summary>
public sealed class EMIPaymentRecordedEvent : DomainEvent
{
    public long LoanNo { get; }
    public long InstallmentId { get; }
    public decimal PrincipalPaid { get; }
    public decimal InterestPaid { get; }
    public DateTime PaymentDate { get; }

    public EMIPaymentRecordedEvent(long loanNo, long installmentId, decimal principalPaid, decimal interestPaid, DateTime paymentDate)
    {
        AggregateId = loanNo;
        LoanNo = loanNo;
        InstallmentId = installmentId;
        PrincipalPaid = principalPaid;
        InterestPaid = interestPaid;
        PaymentDate = paymentDate;
    }
}

/// <summary>
/// Raised when a loan is settled
/// </summary>
public sealed class LoanSettledEvent : DomainEvent
{
    public long LoanNo { get; }
    public decimal RemainingBalance { get; }
    public DateTime SettledOn { get; }

    public LoanSettledEvent(long loanNo, decimal remainingBalance, DateTime settledOn)
    {
        AggregateId = loanNo;
        LoanNo = loanNo;
        RemainingBalance = remainingBalance;
        SettledOn = settledOn;
    }
}

/// <summary>
/// Raised when a loan is closed
/// </summary>
public sealed class LoanClosedEvent : DomainEvent
{
    public long LoanNo { get; }
    public DateTime ClosedOn { get; }
    public string Reason { get; }

    public LoanClosedEvent(long loanNo, DateTime closedOn, string reason)
    {
        AggregateId = loanNo;
        LoanNo = loanNo;
        ClosedOn = closedOn;
        Reason = reason;
    }
}
