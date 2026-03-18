using ItemMasterService.Domain.Common;
using ItemMasterService.Domain.Events;

namespace ItemMasterService.Domain.Entities;

/// <summary>Maps to CANTEEN_ITEM_PRICE_MASTER table.</summary>
public class CanteenItemPriceMaster : BaseEntity
{
    public long CanteenUnitCode { get; private set; }   // CN_COM_COD
    public long ItemCode { get; private set; }           // CN_ITM_COD
    public decimal? EmployeeContribution { get; private set; } // CN_EMP_CON
    public decimal? EmployerContribution { get; private set; } // CN_EPR_CON
    public DateTime EffectiveDate { get; private set; }  // CN_EFF_DAT
    public DateTime? ClosureDate { get; private set; }   // CN_CLS_DAT
    public DateTime? EnteredOn { get; private set; }     // CN_ENT_DAT
    public string? EnteredBy { get; private set; }       // CN_ENT_USR

    // Navigation
    public CanteenItemMaster? ItemMaster { get; private set; }

    private CanteenItemPriceMaster() { }

    public static CanteenItemPriceMaster Create(
        long canteenUnitCode,
        long itemCode,
        decimal? employeeContribution,
        decimal? employerContribution,
        DateTime effectiveDate,
        string enteredBy)
    {
        return new CanteenItemPriceMaster
        {
            CanteenUnitCode = canteenUnitCode,
            ItemCode = itemCode,
            EmployeeContribution = employeeContribution,
            EmployerContribution = employerContribution,
            EffectiveDate = effectiveDate,
            EnteredOn = DateTime.UtcNow,
            EnteredBy = enteredBy?.Trim().Length > 50 ? enteredBy.Trim()[..50] : enteredBy?.Trim()
        };
    }

    public void Close(DateTime closureDate)
    {
        ClosureDate = closureDate;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePrice(decimal? employeeContribution, decimal? employerContribution)
    {
        EmployeeContribution = employeeContribution;
        EmployerContribution = employerContribution;
        UpdatedAt = DateTime.UtcNow;
    }
}
