using DeductionService.Domain.Common;

namespace DeductionService.Domain.Entities;

/// <summary>
/// Maps to ADHOC_PAY_DED table — ad-hoc payroll deduction records.
/// </summary>
public class AdhocPayDeduction : BaseEntity
{
    public long SystemId { get; private set; }           // PY_SYS_ID
    public long? CanteenUnit { get; private set; }       // PY_CAN_UNT
    public long? SerialNumber { get; private set; }      // PY_SRL_NUM
    public long? BatchNumber { get; private set; }       // PY_BAT_NUM
    public DateTime? TransactionDate { get; private set; } // PY_TRN_DAT
    public string? EarningDeductionCode { get; private set; } // PY_ED_COD (6)
    public double? ReferenceNumber { get; private set; } // PY_REF_NUM
    public decimal? PayAmount { get; private set; }      // PY_PAY_AMT
    public long? OppositeAmount { get; private set; }    // PY_OPP_AMT
    public DateTime? EntryDate { get; private set; }     // PY_ENT_DAT
    public long? EnteredByUserId { get; private set; }   // PY_ENT_USR
    public string? CancelFlag { get; private set; }      // PY_CAN_FLG (1)
    public long? AttachmentNumber { get; private set; }  // PY_ATT_NUM
    public string? CompanyCode { get; private set; }     // PY_COM_COD (3)
    public long? EmployeeNumber { get; private set; }    // PY_EMP_NUM
    public string? UpdateFlag { get; private set; }      // PY_UPD_FLG (1)
    public long? SequenceNumber { get; private set; }    // PY_SEQ_NUM
    public string? GradeType { get; private set; }       // PY_GRD_TYP (3)

    private AdhocPayDeduction() { }

    public static AdhocPayDeduction Create(
        long systemId,
        long? canteenUnit,
        decimal? payAmount,
        string? earningDeductionCode,
        long? employeeNumber,
        long? enteredByUserId)
    {
        var entity = new AdhocPayDeduction
        {
            SystemId = systemId,
            CanteenUnit = canteenUnit,
            PayAmount = payAmount,
            EarningDeductionCode = earningDeductionCode,
            EmployeeNumber = employeeNumber,
            EnteredByUserId = enteredByUserId,
            EntryDate = DateTime.UtcNow,
            TransactionDate = DateTime.UtcNow,
            CancelFlag = "N",
            UpdateFlag = "N"
        };

        entity.AddDomainEvent(new Events.DeductionCreatedEvent(systemId, employeeNumber, payAmount));
        return entity;
    }

    public void Cancel(long cancelledByUserId)
    {
        if (CancelFlag == "Y")
            throw new Exceptions.DeductionDomainException("Deduction is already cancelled.");

        CancelFlag = "Y";
        UpdateFlag = "Y";
        AddDomainEvent(new Events.DeductionCancelledEvent(SystemId, EmployeeNumber, cancelledByUserId));
    }

    public void UpdateAmount(decimal newAmount)
    {
        if (CancelFlag == "Y")
            throw new Exceptions.DeductionDomainException("Cannot update a cancelled deduction.");

        PayAmount = newAmount;
        UpdateFlag = "Y";
    }
}
