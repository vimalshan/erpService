using LoanTransaction.Domain.Common;
using LoanTransaction.Domain.Entities;
using LoanTransaction.Domain.Events;
using LoanTransaction.Domain.ValueObjects;

namespace LoanTransaction.Domain.Aggregates;

/// <summary>
/// Aggregate root for a disbursed loan. Maps to LOAN_MAIN table.
/// Owns LoanInstallments, LoanSettlements, LoanLedger entries,
/// LoanEmpInterestRate, and LoanAdjustment.
/// </summary>
public class LoanAggregate : Entity
{
    // ── Core identifiers ──────────────────────────────────────────────
    public long ApplicationId { get; private set; }    // LOAN_APPID
    public long EmployeeId { get; private set; }       // LOAN_EMPSYSID
    public long LoanDefinitionId { get; private set; } // LOAN_ID
    public long GradeId { get; private set; }          // LOAN_GRADEID
    public long UnitId { get; private set; }           // LOAN_UNITID
    public long SubclassId { get; private set; }       // LOAN_SUBCLASSID
    public long GuarantorId { get; private set; }      // LOAN_GUARANTOR

    // ── Disbursement ──────────────────────────────────────────────────
    public DisbursementType DisbursementType { get; private set; } = null!; // LOAN_DISBTYPE
    public Money PrincipalAmount { get; private set; } = null!;   // LOAN_PRNAMT
    public Money OldPrincipalAdj { get; private set; } = null!;   // LOAN_OLDPRNADJ
    public Money AmountPaid { get; private set; } = null!;        // LOAN_PAID
    public Money PrincipalOutstanding { get; private set; } = null!; // LOAN_PRNOUT
    public DateTime EffectiveDate { get; private set; }           // LOAN_DATE
    public DateTime FirstInstallmentDate { get; private set; }    // LOAN_FIRSTINSDATE
    public DateTime LastInstallmentDate { get; private set; }     // LOAN_LASTINSDATE
    public DateTime? ClosureDate { get; private set; }            // LOAN_CLSDATE

    // ── Loan terms ────────────────────────────────────────────────────
    public string Reason { get; private set; } = string.Empty;    // LOAN_REASON
    public string? ApprovalRemarks { get; private set; }          // LOAN_APRREMARKS
    public ClosureType ClosureType { get; private set; } = null!; // LOAN_CLOSURETYPE
    public long NewLoanNo { get; private set; }                   // LOAN_NEWLOANNO
    public bool HasEmployeeInterestRate { get; private set; }     // LOAN_EMPINTRATE
    public char CompoundingFactor { get; private set; }           // LOAN_COMFACTOR
    public char InterestFrequency { get; private set; }           // LOAN_INTFREQUENCY
    public RecoveryMethod RecoveryMethod { get; private set; } = null!; // LOAN_RECTYPE

    // ── ED (Earning Deduction) IDs ────────────────────────────────────
    public long AmountEdId { get; private set; }  // LOAN_AMTEDID
    public long PrnEdId { get; private set; }     // LOAN_PRNEDID
    public long IntEdId { get; private set; }     // LOAN_INTEDID

    // ── Employee-specific overrides ───────────────────────────────────
    public int? EmpInstallmentNos { get; private set; }    // LOAN_EMPINSNOS
    public Money? EmpInstallmentAmount { get; private set; } // LOAN_EMPINSAMT

    // ── Child collections ─────────────────────────────────────────────
    private readonly List<LoanInstallment> _installments = new();
    private readonly List<LoanSettlement> _settlements = new();
    private readonly List<LoanLedger> _ledgerEntries = new();
    private readonly List<LoanEmpInterestRate> _interestRates = new();
    private readonly List<LoanAdjustment> _adjustments = new();

    public IReadOnlyList<LoanInstallment> Installments => _installments.AsReadOnly();
    public IReadOnlyList<LoanSettlement> Settlements => _settlements.AsReadOnly();
    public IReadOnlyList<LoanLedger> LedgerEntries => _ledgerEntries.AsReadOnly();
    public IReadOnlyList<LoanEmpInterestRate> InterestRates => _interestRates.AsReadOnly();
    public IReadOnlyList<LoanAdjustment> Adjustments => _adjustments.AsReadOnly();

    public bool IsClosed => ClosureDate.HasValue;
    public bool IsActive => !IsClosed;

    // ─── EF Core constructor ─────────────────────────────────────────
    private LoanAggregate() { }

    // ─── Factory method: Disburse ─────────────────────────────────────
    public static LoanAggregate Disburse(
        long applicationId,
        long employeeId,
        long loanDefinitionId,
        long gradeId,
        long unitId,
        long subclassId,
        long guarantorId,
        DisbursementType disbursementType,
        Money principalAmount,
        Money amountPaid,
        RecoveryMethod recoveryMethod,
        DateTime effectiveDate,
        DateTime firstInstallmentDate,
        DateTime lastInstallmentDate,
        string reason,
        char compoundingFactor,
        char interestFrequency,
        bool hasEmployeeInterestRate,
        long amountEdId,
        long prnEdId,
        long intEdId,
        long createdBy)
    {
        if (applicationId <= 0) throw new ArgumentException("Application ID must be > 0.");
        if (employeeId <= 0) throw new ArgumentException("Employee ID must be > 0.");
        if (principalAmount is null || !principalAmount.IsPositive) throw new ArgumentException("Principal amount must be positive.");
        if (string.IsNullOrWhiteSpace(reason)) throw new ArgumentException("Reason is required.");
        if (reason.Length > 200) throw new ArgumentException("Reason cannot exceed 200 characters.");

        var now = DateTime.UtcNow;
        var loan = new LoanAggregate
        {
            ApplicationId = applicationId,
            EmployeeId = employeeId,
            LoanDefinitionId = loanDefinitionId,
            GradeId = gradeId,
            UnitId = unitId,
            SubclassId = subclassId,
            GuarantorId = guarantorId,
            DisbursementType = disbursementType,
            PrincipalAmount = principalAmount,
            OldPrincipalAdj = Money.Zero(),
            AmountPaid = amountPaid,
            PrincipalOutstanding = principalAmount,
            RecoveryMethod = recoveryMethod,
            EffectiveDate = effectiveDate,
            FirstInstallmentDate = firstInstallmentDate,
            LastInstallmentDate = lastInstallmentDate,
            ClosureType = ValueObjects.ClosureType.FromValue("LIV"),
            NewLoanNo = 0,
            Reason = reason,
            CompoundingFactor = compoundingFactor,
            InterestFrequency = interestFrequency,
            HasEmployeeInterestRate = hasEmployeeInterestRate,
            AmountEdId = amountEdId,
            PrnEdId = prnEdId,
            IntEdId = intEdId,
            CreatedBy = createdBy,
            CreatedAt = now,
            ModifiedBy = createdBy,
            ModifiedAt = now
        };

        loan.RaiseDomainEvent(new LoanDisbursedEvent
        {
            LoanNo = loan.Id,
            ApplicationId = applicationId,
            EmployeeId = employeeId,
            PrincipalAmount = principalAmount.Amount,
            DisbursedAt = now
        });

        return loan;
    }

    // ─── Add installment schedule ─────────────────────────────────────
    public void AddInstallment(LoanInstallment installment)
    {
        if (installment is null) throw new ArgumentNullException(nameof(installment));
        _installments.Add(installment);
    }

    // ─── Record EMI payment ───────────────────────────────────────────
    public void RecordEmiPayment(long installmentId, decimal principalPaid, decimal interestPaid, long paidBy)
    {
        if (IsClosed) throw new InvalidOperationException("Cannot record payment on a closed loan.");

        var installment = _installments.FirstOrDefault(i => i.Id == installmentId)
            ?? throw new KeyNotFoundException($"Installment {installmentId} not found on loan {Id}.");

        installment.RecordPayment(principalPaid, interestPaid, paidBy);

        // Update principal outstanding
        var newOutstanding = PrincipalOutstanding.Amount - principalPaid;
        PrincipalOutstanding = Money.Create(Math.Max(newOutstanding, 0));

        // Add settlement record
        var settlement = new LoanSettlement
        {
            LoanNo = Id,
            UnitId = UnitId,
            SettlementType = "INS",
            InstallmentNo = installment.InstallmentNo,
            InstallmentDate = installment.InstallmentDate,
            RecoveryDate = DateTime.UtcNow,
            RecoveryType = "PRN",
            InstallmentAmount = installment.InstallmentAmount,
            PayType = "PAY",
            UpdatedBy = paidBy,
            UpdatedOn = DateTime.UtcNow
        };
        _settlements.Add(settlement);

        // Add debit ledger entry
        _ledgerEntries.Add(CreateLedgerEntry('D', $"EMI #{installment.InstallmentNo} Principal Recovery",
            principalPaid, "PRN", 0, paidBy));

        if (interestPaid > 0)
            _ledgerEntries.Add(CreateLedgerEntry('D', $"EMI #{installment.InstallmentNo} Interest Recovery",
                interestPaid, "INT", 0, paidBy));

        ModifiedBy = paidBy;
        ModifiedAt = DateTime.UtcNow;

        RaiseDomainEvent(new EmiPaymentRecordedEvent
        {
            LoanNo = Id,
            InstallmentId = installmentId,
            InstallmentNo = installment.InstallmentNo,
            PrincipalPaid = principalPaid,
            InterestPaid = interestPaid,
            PrincipalOutstanding = PrincipalOutstanding.Amount,
            PaidBy = paidBy,
            PaidAt = DateTime.UtcNow
        });

        // Auto-close if fully paid
        if (PrincipalOutstanding.IsZero)
            CloseLoan(paidBy, "LIV");
    }

    // ─── Close loan ───────────────────────────────────────────────────
    public void CloseLoan(long closedBy, string closureTypeCode)
    {
        if (IsClosed) throw new InvalidOperationException("Loan is already closed.");

        ClosureType = ValueObjects.ClosureType.FromValue(closureTypeCode);
        ClosureDate = DateTime.UtcNow;
        ModifiedBy = closedBy;
        ModifiedAt = DateTime.UtcNow;

        RaiseDomainEvent(new LoanClosedEvent
        {
            LoanNo = Id,
            EmployeeId = EmployeeId,
            ClosureType = closureTypeCode,
            ClosedAt = ClosureDate.Value
        });
    }

    // ─── Add adjustment ───────────────────────────────────────────────
    public void AddAdjustment(long adjLoanNo, decimal adjPrincipal, decimal adjInterest, long updatedBy)
    {
        if (IsClosed) throw new InvalidOperationException("Cannot adjust a closed loan.");

        _adjustments.Add(new LoanAdjustment
        {
            LoanNo = Id,
            AdjLoanNo = adjLoanNo,
            AdjPrincipalAmount = adjPrincipal,
            AdjInterestAmount = adjInterest,
            UpdatedBy = updatedBy,
            UpdatedOn = DateTime.UtcNow
        });

        ModifiedBy = updatedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    // ─── Set employee-specific interest rate ─────────────────────────
    public void SetEmployeeInterestRate(int rate, decimal emiAmount, int numberOfInstallments, long modifiedBy)
    {
        // Close existing active rates
        foreach (var r in _interestRates.Where(x => x.IsActive))
            r.Close(modifiedBy);

        _interestRates.Add(new LoanEmpInterestRate
        {
            LoanNo = Id,
            Rate = rate,
            EmiAmount = emiAmount,
            NumberOfInstallments = numberOfInstallments,
            EffectiveDate = DateTime.UtcNow,
            LastModifiedBy = modifiedBy,
            LastModifiedOn = DateTime.UtcNow
        });

        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }

    // ─── Helpers ─────────────────────────────────────────────────────
    private LoanLedger CreateLedgerEntry(char dcFlag, string desc, decimal amount, string trnType, long refNo, long updatedBy)
        => new()
        {
            LoanNo = Id,
            EmployeeId = EmployeeId,
            UnitId = UnitId,
            EmployeeNo = EmployeeId,
            TransactionDate = DateTime.UtcNow,
            DCFlag = dcFlag,
            Description = desc,
            TransactionAmount = amount,
            TransactionType = trnType,
            TransactionRefNo = refNo,
            UpdatedBy = updatedBy,
            UpdatedOn = DateTime.UtcNow
        };
}
