using DeductionService.Domain.Common;

namespace DeductionService.Domain.Entities;

/// <summary>
/// Maps to ADHOC_PAY_DED_HIS table — history of payroll deductions.
/// </summary>
public class AdhocPayDeductionHistory : BaseEntity
{
    public long SystemId { get; private set; }           // PY_SYS_ID (NOT NULL)
    public long CanteenUnit { get; private set; }        // PY_CAN_UNT (NOT NULL)
    public long? SerialNumber { get; private set; }
    public long? BatchNumber { get; private set; }
    public DateTime? TransactionDate { get; private set; }
    public string? EarningDeductionCode { get; private set; }
    public double? ReferenceNumber { get; private set; }
    public decimal? PayAmount { get; private set; }
    public long? OppositeAmount { get; private set; }
    public DateTime? EntryDate { get; private set; }
    public long? EnteredByUserId { get; private set; }
    public string? CancelFlag { get; private set; }
    public long? AttachmentNumber { get; private set; }
    public string? CompanyCode { get; private set; }
    public long? EmployeeNumber { get; private set; }
    public string? UpdateFlag { get; private set; }

    private AdhocPayDeductionHistory() { }

    public static AdhocPayDeductionHistory CreateFromDeduction(AdhocPayDeduction deduction)
    {
        return new AdhocPayDeductionHistory
        {
            SystemId = deduction.SystemId,
            CanteenUnit = deduction.CanteenUnit ?? 0,
            SerialNumber = deduction.SerialNumber,
            BatchNumber = deduction.BatchNumber,
            TransactionDate = deduction.TransactionDate,
            EarningDeductionCode = deduction.EarningDeductionCode,
            ReferenceNumber = deduction.ReferenceNumber,
            PayAmount = deduction.PayAmount,
            OppositeAmount = deduction.OppositeAmount,
            EntryDate = DateTime.UtcNow,
            EnteredByUserId = deduction.EnteredByUserId,
            CancelFlag = deduction.CancelFlag,
            AttachmentNumber = deduction.AttachmentNumber,
            CompanyCode = deduction.CompanyCode,
            EmployeeNumber = deduction.EmployeeNumber,
            UpdateFlag = deduction.UpdateFlag
        };
    }
}
