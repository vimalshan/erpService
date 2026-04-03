namespace LoanTransaction.Domain.Entities;

/// <summary>Maps to LOAN_LEDGER table – debit/credit ledger entries for a loan</summary>
public class LoanLedger
{
    public long Id { get; set; }               // LOAN_LEDGERID
    public long LoanNo { get; set; }           // LOAN_NO
    public long EmployeeId { get; set; }       // LOAN_EMPSYSID
    public long UnitId { get; set; }           // LOAN_UNITID
    public long EmployeeNo { get; set; }       // LOAN_EMPNO
    public DateTime TransactionDate { get; set; } // LOAN_TRNDATE
    public char DCFlag { get; set; }           // LOAN_DCFLAG: D=Debit, C=Credit
    public string Description { get; set; } = string.Empty; // LOAN_DESCRIPTION
    public decimal TransactionAmount { get; set; } // LOAN_TRNAMT
    public string TransactionType { get; set; } = string.Empty; // LOAN_TRNTYPE
    public long TransactionRefNo { get; set; }    // LOAN_TRNREFNUM
    public long ScheduleId { get; set; }           // LOAN_SCHEDULEID
    public long UpdatedBy { get; set; }            // LOAN_UPDATEDBY
    public DateTime UpdatedOn { get; set; }        // LOAN_UPDATEDON

    public bool IsDebit => DCFlag == 'D';
    public bool IsCredit => DCFlag == 'C';
}
