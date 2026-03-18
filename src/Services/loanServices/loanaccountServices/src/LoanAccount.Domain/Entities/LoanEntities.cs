using Ardalis.GuardClauses;
using LoanAccount.Domain.Common;
using LoanAccount.Domain.Events;
using LoanAccount.Domain.ValueObjects;

namespace LoanAccount.Domain.Entities;

/// <summary>
/// Represents a loan main entity (aggregate root)
/// </summary>
public class LoanMain : Entity
{
    public long LoanNo { get; private set; }
    public long LoanAppId { get; private set; }
    public long EmpSysId { get; private set; }
    public long LoanId { get; private set; }
    public long GradeId { get; private set; }
    
    public Money PrincipalAmount { get; private set; } = null!;
    public Money OldPrincipalAdjustment { get; private set; } = null!;
    public Money DisbursedAmount { get; private set; } = null!;
    public Money PrincipalOutstanding { get; private set; } = null!;
    
    public DisbursementType DisbursementType { get; private set; } = null!;
    public LoanStatus LoanStatus { get; private set; } = null!;
    public RecoveryMethod RecoveryMethod { get; private set; } = null!;
    
    public DateTime LoanDate { get; private set; }
    public DateTime FirstInstallmentDate { get; private set; }
    public DateTime LastInstallmentDate { get; private set; }
    public DateTime? LoanClosureDate { get; private set; }
    
    public long UnitId { get; private set; }
    public long SubClassId { get; private set; }
    public string Reason { get; private set; } = null!;
    public long GuarantorId { get; private set; }
    public string? ApprovalRemarks { get; private set; }
    public long? NewLoanNo { get; private set; }
    
    public bool EmployeeWiseInterestRate { get; private set; }
    public bool CompoundingFactor { get; private set; }
    public char InterestFrequency { get; private set; }
    
    public long? EmployeeSpecificInstallmentNos { get; private set; }
    public Money? EmployeeSpecificInstallmentAmount { get; private set; }

    public long AcountDisbursementEDId { get; private set; }
    public long PrincipalRecoveryEDId { get; private set; }
    public long InterestRecoveryEDId { get; private set; }

    private readonly List<LoanInstallment> _installments = [];
    public IReadOnlyList<LoanInstallment> Installments => _installments.AsReadOnly();

    private LoanMain() { }

    public static LoanMain Create(
        long loanNo,
        long loanAppId,
        long empSysId,
        long loanId,
        long gradeId,
        decimal principalAmount,
        DisbursementType disbursementType,
        DateTime loanDate,
        DateTime firstInstallmentDate,
        long unitId,
        long subClassId,
        string reason,
        long guarantorId,
        long createdBy)
    {
        Guard.Against.NegativeOrZero(loanNo, nameof(loanNo));
        Guard.Against.NegativeOrZero(principalAmount, nameof(principalAmount));
        Guard.Against.NullOrWhiteSpace(reason, nameof(reason));

        var loan = new LoanMain
        {
            Id = loanNo,
            LoanNo = loanNo,
            LoanAppId = loanAppId,
            EmpSysId = empSysId,
            LoanId = loanId,
            GradeId = gradeId,
            PrincipalAmount = new Money(principalAmount),
            OldPrincipalAdjustment = new Money(0),
            DisbursedAmount = new Money(0),
            PrincipalOutstanding = new Money(principalAmount),
            DisbursementType = disbursementType,
            LoanStatus = LoanStatus.Active,
            RecoveryMethod = RecoveryMethod.RBM, // Default recovery method
            LoanDate = loanDate,
            FirstInstallmentDate = firstInstallmentDate,
            LastInstallmentDate = firstInstallmentDate,
            UnitId = unitId,
            SubClassId = subClassId,
            Reason = reason,
            GuarantorId = guarantorId,
            EmployeeWiseInterestRate = false,
            CompoundingFactor = false,
            InterestFrequency = 'M',
            CreatedBy = createdBy,
            ModifiedBy = createdBy
        };

        loan.RaiseDomainEvent(new LoanCreatedEvent(
            loanNo, loanAppId, empSysId, principalAmount, loanDate));

        return loan;
    }

    public void Approve(InterestRate interestRate, long approvedBy, string? remarks = null)
    {
        ApprovalRemarks = remarks;
        CreatedBy = approvedBy;
        CreatedOn = DateTime.UtcNow;

        RaiseDomainEvent(new LoanApprovedEvent(LoanNo, interestRate.Rate, DateTime.UtcNow, approvedBy));
    }

    public void Disburse(decimal amount, long disbursedBy)
    {
        Guard.Against.NegativeOrZero(amount, nameof(amount));
        Guard.Against.NegativeOrZero(disbursedBy, nameof(disbursedBy));

        DisbursedAmount = new Money(amount);
        ModifiedBy = disbursedBy;
        ModifiedOn = DateTime.UtcNow;

        RaiseDomainEvent(new LoanDisbursedEvent(LoanNo, amount, DateTime.UtcNow));
    }

    public void RecordInstallment(LoanInstallment installment)
    {
        Guard.Against.Null(installment, nameof(installment));
        _installments.Add(installment);
    }

    public void Close(DateTime closureDate, string reason = "")
    {
        LoanClosureDate = closureDate;
        LoanStatus = LoanStatus.Closed;
        ModifiedOn = DateTime.UtcNow;

        RaiseDomainEvent(new LoanClosedEvent(LoanNo, closureDate, reason));
    }

    public void Settle(long settledBy)
    {
        LoanStatus = LoanStatus.Closed;
        LoanClosureDate = DateTime.UtcNow;
        ModifiedBy = settledBy;
        ModifiedOn = DateTime.UtcNow;

        RaiseDomainEvent(new LoanSettledEvent(LoanNo, PrincipalOutstanding.Amount, DateTime.UtcNow));
    }
}

/// <summary>
/// Represents a loan installment entity
/// </summary>
public class LoanInstallment : Entity
{
    public long LoanNo { get; private set; }
    public long UnitId { get; private set; }
    public long InstallmentNo { get; private set; }
    
    public Money InstallmentAmount { get; private set; } = null!;
    public Money PrincipalOutstanding { get; private set; } = null!;
    public Money PrincipalAdjustment { get; private set; } = null!;
    public Money InterestAdjustment { get; private set; } = null!;
    public Money InterestAccrued { get; private set; } = null!;
    public Money InterestRecovered { get; private set; } = null!;
    public Money PrincipalRecovered { get; private set; } = null!;
    
    public DateTime InstallmentDate { get; private set; }
    public DateTime? InterestFromDate { get; private set; }
    
    public InterestRate InterestRate { get; private set; } = null!;
    public string Remarks { get; private set; } = null!;

    private LoanInstallment() { }

    public static LoanInstallment Create(
        long loanNo,
        long unitId,
        long installmentNo,
        decimal installmentAmount,
        decimal principalOutstanding,
        int interestRatePercentage,
        DateTime installmentDate,
        long createdBy)
    {
        Guard.Against.NegativeOrZero(loanNo, nameof(loanNo));
        Guard.Against.NegativeOrZero(installmentAmount, nameof(installmentAmount));

        var installment = new LoanInstallment
        {
            LoanNo = loanNo,
            UnitId = unitId,
            InstallmentNo = installmentNo,
            InstallmentAmount = new Money(installmentAmount),
            PrincipalOutstanding = new Money(principalOutstanding),
            PrincipalAdjustment = new Money(0),
            InterestAdjustment = new Money(0),
            InterestAccrued = new Money(0),
            InterestRecovered = new Money(0),
            PrincipalRecovered = new Money(0),
            InstallmentDate = installmentDate,
            InterestRate = new InterestRate(interestRatePercentage),
            Remarks = string.Empty,
            CreatedBy = createdBy,
            ModifiedBy = createdBy
        };

        return installment;
    }

    public void RecordPayment(decimal principalPaid, decimal interestPaid, long paidBy)
    {
        Guard.Against.NegativeOrZero(principalPaid, nameof(principalPaid));
        Guard.Against.NegativeOrZero(interestPaid, nameof(interestPaid));

        PrincipalAdjustment = new Money(principalPaid);
        InterestAdjustment = new Money(interestPaid);
        PrincipalRecovered = new Money(principalPaid);
        InterestRecovered = new Money(interestPaid);
        ModifiedBy = paidBy;
        ModifiedOn = DateTime.UtcNow;
    }
}

/// <summary>
/// Represents employee-wise interest rate master
/// </summary>
public class LoanEmployeeInterestRate : Entity
{
    public long LoanNo { get; private set; }
    public InterestRate InterestRate { get; private set; } = null!;
    public DateTime EffectiveDate { get; private set; }
    public DateTime? ClosureDate { get; private set; }
    
    public Money EMIAmount { get; private set; } = null!;
    public int InstallmentNumbers { get; private set; }

    private LoanEmployeeInterestRate() { }

    public static LoanEmployeeInterestRate Create(
        long loanNo,
        decimal interestRate,
        decimal emiAmount,
        int installmentNumbers,
        long createdBy)
    {
        var rate = new LoanEmployeeInterestRate
        {
            LoanNo = loanNo,
            InterestRate = new InterestRate(interestRate),
            EMIAmount = new Money(emiAmount),
            InstallmentNumbers = installmentNumbers,
            EffectiveDate = DateTime.UtcNow,
            CreatedBy = createdBy,
            ModifiedBy = createdBy
        };

        return rate;
    }
}

/// <summary>
/// Represents loan ledger for transaction history
/// </summary>
public class LoanLedger : Entity
{
    public long LoanNo { get; private set; }
    public long EmpSysId { get; private set; }
    public long EmpNo { get; private set; }
    public long UnitId { get; private set; }
    
    public DateTime TransactionDate { get; private set; }
    public char DCFlag { get; private set; } // D = Debit, C = Credit
    public string Description { get; private set; } = null!;
    public Money TransactionAmount { get; private set; } = null!;
    public string TransactionType { get; private set; } = null!;
    public long TransactionReferenceNo { get; private set; }
    public long ScheduleId { get; private set; }

    private LoanLedger() { }

    public static LoanLedger Create(
        long loanNo,
        long empSysId,
        long empNo,
        long unitId,
        char dcFlag,
        string description,
        decimal amount,
        string transactionType,
        long referenceNo,
        long scheduleId,
        long createdBy)
    {
        var ledger = new LoanLedger
        {
            LoanNo = loanNo,
            EmpSysId = empSysId,
            EmpNo = empNo,
            UnitId = unitId,
            TransactionDate = DateTime.UtcNow,
            DCFlag = dcFlag,
            Description = description,
            TransactionAmount = new Money(Math.Abs(amount)),
            TransactionType = transactionType,
            TransactionReferenceNo = referenceNo,
            ScheduleId = scheduleId,
            CreatedBy = createdBy,
            ModifiedBy = createdBy
        };

        return ledger;
    }
}

/// <summary>
/// Represents loan settlement record
/// </summary>
public class LoanSettlement : Entity
{
    public long LoanNo { get; private set; }
    public long UnitId { get; private set; }
    public long InstallmentNo { get; private set; }
    
    public SettlementType SettlementType { get; private set; } = null!;
    public DateTime InstallmentDate { get; private set; }
    public DateTime RecoveryDate { get; private set; }
    public string RecoveryType { get; private set; } = null!; // PRN = Principal, INT = Interest
    
    public Money InstallmentAmount { get; private set; } = null!;
    public string PaymentType { get; private set; } = null!; // DIR = Direct, PAY = Payroll, ADJ = Adjustment
    public long? PayrollBatchId { get; private set; }
    public long? AdjustmentLoanNo { get; private set; }
    public DateTime? CancelledDate { get; private set; }
    public long? CancelledBy { get; private set; }

    private LoanSettlement() { }

    public static LoanSettlement Create(
        long loanNo,
        long unitId,
        long installmentNo,
        SettlementType settlementType,
        DateTime installmentDate,
        DateTime recoveryDate,
        string recoveryType,
        decimal amount,
        string paymentType,
        long createdBy)
    {
        var settlement = new LoanSettlement
        {
            LoanNo = loanNo,
            UnitId = unitId,
            InstallmentNo = installmentNo,
            SettlementType = settlementType,
            InstallmentDate = installmentDate,
            RecoveryDate = recoveryDate,
            RecoveryType = recoveryType,
            InstallmentAmount = new Money(amount),
            PaymentType = paymentType,
            CreatedBy = createdBy,
            ModifiedBy = createdBy
        };

        return settlement;
    }

    public void Cancel(long cancelledBy)
    {
        CancelledDate = DateTime.UtcNow;
        CancelledBy = cancelledBy;
        ModifiedBy = cancelledBy;
        ModifiedOn = DateTime.UtcNow;
    }
}
