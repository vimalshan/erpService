using travelTransactionService.Domain.Common;

namespace travelTransactionService.Domain.Entities;

public class TaxComponent : BaseEntity
{
    public long VendorCode { get; private set; }
    public string? Component { get; private set; }

    private TaxComponent() { }

    public static TaxComponent Create(long vendorCode, string? component)
    {
        return new TaxComponent
        {
            VendorCode = vendorCode,
            Component = component
        };
    }
}
