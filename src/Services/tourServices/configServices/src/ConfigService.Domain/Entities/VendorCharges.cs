using ConfigService.Domain.Common;

namespace ConfigService.Domain.Entities;

public class VendorCharges : BaseEntity<string>
{
    public string? VendorId { get; private set; }
    public string? Rate { get; private set; }
    public DateTime? EffectiveDate { get; private set; }
    public DateTime? ClosureDate { get; private set; }
    public string? EnteredBy { get; private set; }
    public DateTime? EnteredOn { get; private set; }

    private VendorCharges() { }

    public static VendorCharges Create(string chargesId, string? vendorId, string? rate,
        DateTime? effDate, DateTime? clsDate, string? entBy)
    {
        return new VendorCharges
        {
            Id = chargesId, VendorId = vendorId, Rate = rate,
            EffectiveDate = effDate, ClosureDate = clsDate,
            EnteredBy = entBy, EnteredOn = DateTime.UtcNow
        };
    }
}
