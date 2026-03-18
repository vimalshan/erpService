namespace InventoryManagement.Domain.Entities;

public class AdvanceLicenseEntitlement
{
    public long AdvLicId { get; set; }
    public int AdvLicEntitlement { get; set; }

    public AdvanceLicenseMaster? AdvanceLicense { get; set; }
}
