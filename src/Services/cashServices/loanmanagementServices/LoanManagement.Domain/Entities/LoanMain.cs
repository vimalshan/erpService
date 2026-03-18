using LoanManagement.Domain.Common;
using LoanManagement.Domain.Enums;
using LoanManagement.Domain.Events;
using LoanManagement.Domain.Exceptions;

namespace LoanManagement.Domain.Entities;

/// <summary>
/// Aggregate root for a Loan — maps to LOAN_MAIN table.
/// </summary>
public class LoanMain : BaseEntity
{
    public decimal LoanId { get; private set; }
    public string LoanKey { get; private set; } = default!;
    public decimal LoanOrgId { get; private set; }
    public decimal? LoanOrgCurr { get; private set; }
    public decimal? LoanCurr { get; private set; }
    public DateTime LoanDate { get; private set; }
    public decimal LoanTypeId { get; private set; }
    public decimal LoanBankId { get; private set; }
    public decimal LoanCreatedBy { get; private set; }
    public DateTime LoanCreatedOn { get; private set; }
    public decimal? LoanModifiedBy { get; private set; }
    public DateTime? LoanModifiedOn { get; private set; }
    public decimal LoanAmount { get; private set; }
    public string? LoanStatus { get; private set; }

    private readonly List<LoanDisbursementSchedule> _disbursements = new();
    private readonly List<LoanInterest> _interests = new();
    private readonly List<LoanRepaymentSchedule> _repayments = new();

    public IReadOnlyCollection<LoanDisbursementSchedule> Disbursements => _disbursements.AsReadOnly();
    public IReadOnlyCollection<LoanInterest> Interests => _interests.AsReadOnly();
    public IReadOnlyCollection<LoanRepaymentSchedule> Repayments => _repayments.AsReadOnly();

    private LoanMain() { }

    public static LoanMain Create(
        decimal loanId,
        string loanKey,
        decimal orgId,
        decimal loanAmount,
        decimal loanTypeId,
        decimal bankId,
        decimal createdBy,
        DateTime loanDate,
        decimal? orgCurr = null,
        decimal? loanCurr = null)
    {
        if (loanAmount <= 0)
            throw new LoanDomainException("Loan amount must be positive.");

        if (string.IsNullOrWhiteSpace(loanKey))
            throw new LoanDomainException("Loan key cannot be empty.");

        var loan = new LoanMain
        {
            LoanId = loanId,
            LoanKey = loanKey.ToUpperInvariant(),
            LoanOrgId = orgId,
            LoanAmount = loanAmount,
            LoanTypeId = loanTypeId,
            LoanBankId = bankId,
            LoanCreatedBy = createdBy,
            LoanCreatedOn = DateTime.UtcNow,
            LoanDate = loanDate,
            LoanOrgCurr = orgCurr,
            LoanCurr = loanCurr,
            LoanStatus = "A"
        };

        loan.AddDomainEvent(new LoanCreatedEvent(loanId, loanKey, loanAmount));
        return loan;
    }

    public void AddDisbursement(LoanDisbursementSchedule disbursement)
    {
        if (LoanStatus != "A")
            throw new LoanDomainException("Cannot add disbursement to a non-active loan.");

        _disbursements.Add(disbursement);
        AddDomainEvent(new LoanDisbursedEvent(LoanId, disbursement.DisbId, disbursement.DisbAmount ?? 0));
    }

    public void AddInterest(LoanInterest interest)
    {
        if (LoanStatus != "A")
            throw new LoanDomainException("Cannot add interest configuration to a non-active loan.");

        _interests.Add(interest);
    }

    public void AddRepayment(LoanRepaymentSchedule repayment)
    {
        if (LoanStatus != "A")
            throw new LoanDomainException("Cannot add repayment schedule to a non-active loan.");

        _repayments.Add(repayment);
        AddDomainEvent(new LoanRepaymentScheduledEvent(LoanId, repayment.RepayId, repayment.RepayDate ?? DateTime.UtcNow, repayment.RepayAmt ?? 0));
    }

    public void CloseLoan(decimal modifiedBy)
    {
        if (LoanStatus == "C")
            throw new LoanDomainException("Loan is already closed.");

        LoanStatus = "C";
        LoanModifiedBy = modifiedBy;
        LoanModifiedOn = DateTime.UtcNow;
        AddDomainEvent(new LoanStatusChangedEvent(LoanId, "C"));
    }

    public void MarkDefault(decimal modifiedBy)
    {
        if (LoanStatus == "D")
            throw new LoanDomainException("Loan is already in default.");

        LoanStatus = "D";
        LoanModifiedBy = modifiedBy;
        LoanModifiedOn = DateTime.UtcNow;
        AddDomainEvent(new LoanStatusChangedEvent(LoanId, "D"));
    }

    public bool IsActive => LoanStatus == "A";
}
