using ConfigService.Domain.Common;

namespace ConfigService.Domain.Entities;

public class VendorUnitMap : BaseEntity<string>
{
    public string VendorId { get; private set; } = string.Empty;
    public string PayUnitId { get; private set; } = string.Empty;
    public string OracleSiteId { get; private set; } = string.Empty;
    public string TermId { get; private set; } = string.Empty;

    private VendorUnitMap() { }

    public static VendorUnitMap Create(string mapId, string vendorId, string payUnitId, string oracleSiteId, string termId)
    {
        return new VendorUnitMap
        {
            Id = mapId, VendorId = vendorId, PayUnitId = payUnitId,
            OracleSiteId = oracleSiteId, TermId = termId
        };
    }
}
