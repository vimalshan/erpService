namespace InventoryManagement.Domain.Entities;

public class AdvanceLicenseMaster
{
    public long AdvLicId { get; set; }
    public string? AdvLicNo { get; set; }
    public int? AdvLicFg { get; set; }
    public decimal? AdvLicEoAmt { get; set; }
    public decimal? AdvLicExpAmt { get; set; }

    public ICollection<AdvanceLicenseEntitlement> Entitlements { get; set; } = new List<AdvanceLicenseEntitlement>();
}
