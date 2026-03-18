using ItemMasterService.Domain.Common;

namespace ItemMasterService.Domain.Entities;

/// <summary>Maps to CANTEENGRADE_ITEM_PRICE table.</summary>
public class CanteenGradeItemPrice : AggregateRoot
{
    public long CanteenUnitCode { get; private set; }   // CN_COM_COD (PK)
    public long? ItemCode { get; private set; }          // CN_ITM_COD
    public decimal? EmployeeContribution { get; private set; } // CN_EMP_CON
    public decimal? EmployerContribution { get; private set; } // CN_EPR_CON
    public DateTime? EffectiveDate { get; private set; }  // CN_EFF_DAT
    public DateTime ClosureDate { get; private set; }    // CN_CLS_DAT (NOT NULL)
    public DateTime? EnteredOn { get; private set; }     // CN_ENT_DAT
    public string EnteredBy { get; private set; } = default!; // CN_ENT_USR (NOT NULL)
    public string GradeType { get; private set; } = default!; // CN_GRD_TYP (NOT NULL, 3 char)

    private CanteenGradeItemPrice() { }

    public static CanteenGradeItemPrice Create(
        long canteenUnitCode,
        long? itemCode,
        decimal? employeeContribution,
        decimal? employerContribution,
        DateTime? effectiveDate,
        DateTime closureDate,
        string enteredBy,
        string gradeType)
    {
        return new CanteenGradeItemPrice
        {
            CanteenUnitCode = canteenUnitCode,
            ItemCode = itemCode,
            EmployeeContribution = employeeContribution,
            EmployerContribution = employerContribution,
            EffectiveDate = effectiveDate,
            ClosureDate = closureDate,
            EnteredOn = DateTime.UtcNow,
            EnteredBy = enteredBy?.Trim().Length > 50 ? enteredBy.Trim()[..50] : enteredBy?.Trim() ?? string.Empty,
            GradeType = gradeType?.Trim().Length > 3 ? gradeType.Trim()[..3] : gradeType?.Trim() ?? string.Empty
        };
    }

    public void Update(decimal? employeeContribution, decimal? employerContribution, DateTime closureDate)
    {
        EmployeeContribution = employeeContribution;
        EmployerContribution = employerContribution;
        ClosureDate = closureDate;
        UpdatedAt = DateTime.UtcNow;
        IncrementVersion();
    }
}
