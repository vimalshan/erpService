using MasterDataService.Domain.Common;

namespace MasterDataService.Domain.Entities;

public class TaxSlab : AuditableEntity
{
    public string TaxType { get; private set; } = string.Empty;
    public DateTime EffectiveDate { get; private set; }
    public DateTime? CloseDate { get; private set; }
    public decimal TaxRate { get; private set; }
    public long VendorCode { get; private set; }

    private TaxSlab() { }

    public TaxSlab(string taxType, DateTime effectiveDate, DateTime? closeDate, decimal taxRate, long vendorCode)
    {
        TaxType = taxType ?? throw new ArgumentNullException(nameof(taxType));
        EffectiveDate = effectiveDate;
        CloseDate = closeDate;
        TaxRate = taxRate;
        VendorCode = vendorCode;
    }

    public bool IsActive() => CloseDate == null || CloseDate > DateTime.UtcNow;

    public void Close() => CloseDate = DateTime.UtcNow;
}
