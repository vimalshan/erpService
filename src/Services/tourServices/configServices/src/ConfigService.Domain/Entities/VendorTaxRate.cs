using ConfigService.Domain.Common;

namespace ConfigService.Domain.Entities;

public class VendorTaxRate : BaseEntity<string>
{
    public string? VendorId { get; private set; }
    public string TaxNature { get; private set; } = string.Empty;
    public string TaxRate { get; private set; } = string.Empty;
    public DateTime EffectiveDate { get; private set; }
    public DateTime ClosureDate { get; private set; }
    public string EnteredBy { get; private set; } = string.Empty;
    public DateTime EnteredOn { get; private set; }

    private VendorTaxRate() { }

    public static VendorTaxRate Create(string taxId, string? vendorId, string taxNature,
        string taxRate, DateTime effDate, DateTime clsDate, string entBy)
    {
        return new VendorTaxRate
        {
            Id = taxId, VendorId = vendorId, TaxNature = taxNature,
            TaxRate = taxRate, EffectiveDate = effDate, ClosureDate = clsDate,
            EnteredBy = entBy, EnteredOn = DateTime.UtcNow
        };
    }
}
