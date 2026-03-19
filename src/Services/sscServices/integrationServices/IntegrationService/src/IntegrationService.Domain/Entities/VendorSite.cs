using IntegrationService.Domain.Common;

namespace IntegrationService.Domain.Entities;

public class VendorSite : BaseEntity<long>
{
    public int VendorId { get; private set; }
    public string SiteCode { get; private set; } = string.Empty;
    public string OracleOuId { get; private set; } = string.Empty;

    private readonly List<VendorSiteBuMapping> _buMappings = [];
    public IReadOnlyCollection<VendorSiteBuMapping> BuMappings => _buMappings.AsReadOnly();

    private VendorSite() { }

    public static VendorSite Create(long vendorSiteId, int vendorId, string siteCode, string oracleOuId)
    {
        if (string.IsNullOrWhiteSpace(siteCode))
            throw new ArgumentException("Site code is required.", nameof(siteCode));

        return new VendorSite
        {
            Id = vendorSiteId,
            VendorId = vendorId,
            SiteCode = siteCode,
            OracleOuId = oracleOuId
        };
    }

    public void AddBuMapping(VendorSiteBuMapping mapping)
    {
        _buMappings.Add(mapping);
    }
}
