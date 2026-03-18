namespace EligibilityService.Domain.Entities;

/// <summary>Maps to CAN_ELIGIBILITY_MASTER_HIS table.</summary>
public class EligibilityMasterHistory
{
    public long CanteenUnit { get; private set; }            // CN_COM_COD
    public string ShiftCode { get; private set; } = default!; // CN_SFT_COD
    public decimal ItemCode { get; private set; }             // CN_ITM_COD
    public int? EligibleLimit { get; private set; }           // CN_ELG_LMT
    public decimal? ModifiedUser { get; private set; }        // CN_MOD_USR
    public DateTime? ModifiedOn { get; private set; }         // CN_MOD_DAT

    private EligibilityMasterHistory() { }

    public static EligibilityMasterHistory CreateFrom(EligibilityMaster master, decimal? modifiedUser)
    {
        return new EligibilityMasterHistory
        {
            CanteenUnit = master.CanteenUnit,
            ShiftCode = master.ShiftCode,
            ItemCode = master.ItemCode,
            EligibleLimit = master.EligibleLimit,
            ModifiedUser = modifiedUser,
            ModifiedOn = DateTime.UtcNow
        };
    }
}
