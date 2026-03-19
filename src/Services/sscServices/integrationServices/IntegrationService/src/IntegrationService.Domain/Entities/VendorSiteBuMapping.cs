namespace IntegrationService.Domain.Entities;

public class VendorSiteBuMapping
{
    public long VendorSiteId { get; private set; }
    public long BuId { get; private set; }

    private VendorSiteBuMapping() { }

    public static VendorSiteBuMapping Create(long vendorSiteId, long buId)
    {
        return new VendorSiteBuMapping
        {
            VendorSiteId = vendorSiteId,
            BuId = buId
        };
    }
}
