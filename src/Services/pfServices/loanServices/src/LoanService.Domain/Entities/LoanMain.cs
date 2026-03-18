using LoanService.Domain.Common;
using LoanService.Domain.Events;

namespace LoanService.Domain.Entities;

public class LoanMain : AggregateRoot
{
    public long LoanNo { get; private set; }
    public string? TrustCode { get; private set; }
    public long? MemberId { get; private set; }
    public DateTime? LoanDate { get; private set; }
    public decimal? LoanAmount { get; private set; }
    public long? LoanType { get; private set; }
    public string? LoanReason { get; private set; }
    public string? LoanTenure { get; private set; }
    public decimal? PrincipalOutstanding { get; private set; }
    public char? ClsFlag { get; private set; }
    public long? UpdatedByEmpId { get; private set; }
    public DateTime? UpdatedOn { get; private set; }
    public char Status { get; private set; } = 'A';
    public decimal? Rate { get; private set; }
    public DateTime? ApprovalDate { get; private set; }
    public DateTime? ClosureDate { get; private set; }

    // Navigation properties
    private readonly List<LoanRepayment> _repayments = [];
    public IReadOnlyCollection<LoanRepayment> Repayments => _repayments.AsReadOnly();

    private readonly List<LoanDeduction> _deductions = [];
    public IReadOnlyCollection<LoanDeduction> Deductions => _deductions.AsReadOnly();

    private LoanMain() { } // EF

    public static LoanMain Create(long loanNo, long memberId, decimal amount, long loanType,
        string reason, long createdBy)
    {
        var loan = new LoanMain
        {
            LoanNo = loanNo,
            MemberId = memberId,
            LoanAmount = amount,
            LoanType = loanType,
            LoanReason = reason,
            LoanDate = DateTime.UtcNow,
            UpdatedByEmpId = createdBy,
            UpdatedOn = DateTime.UtcNow,
            Status = 'A',
            PrincipalOutstanding = amount
        };

        loan.AddDomainEvent(new LoanCreatedEvent(loanNo, memberId, amount));
        return loan;
    }

    public void Approve(DateTime approvalDate)
    {
        ApprovalDate = approvalDate;
        UpdatedOn = DateTime.UtcNow;
        AddDomainEvent(new LoanApprovedEvent(LoanNo, approvalDate));
    }

    public void Close(DateTime closureDate)
    {
        Status = 'C';
        ClsFlag = 'Y';
        ClosureDate = closureDate;
        UpdatedOn = DateTime.UtcNow;
        AddDomainEvent(new LoanClosedEvent(LoanNo, closureDate));
    }

    public void SetRate(decimal rate)
    {
        Rate = rate;
        UpdatedOn = DateTime.UtcNow;
    }

    public void SetTenure(string tenure)
    {
        LoanTenure = tenure;
        UpdatedOn = DateTime.UtcNow;
    }

    public void SetTrustCode(string trustCode)
    {
        TrustCode = trustCode;
        UpdatedOn = DateTime.UtcNow;
    }

    public void AddRepayment(LoanRepayment repayment) => _repayments.Add(repayment);
    public void AddDeduction(LoanDeduction deduction) => _deductions.Add(deduction);

    public void UpdatePrincipalOutstanding(decimal amount)
    {
        PrincipalOutstanding = amount;
        UpdatedOn = DateTime.UtcNow;
    }

    public void SetUpdatedBy(long empId)
    {
        UpdatedByEmpId = empId;
        UpdatedOn = DateTime.UtcNow;
    }
}
