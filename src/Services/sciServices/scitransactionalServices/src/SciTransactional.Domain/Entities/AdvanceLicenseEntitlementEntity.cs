using SciTransactional.Domain.Common;

namespace SciTransactional.Domain.Entities;

public sealed class AdvanceLicenseEntitlementEntity : Entity<long>
{
    public int EntitlementRm { get; private set; }

    private AdvanceLicenseEntitlementEntity() { }

    public static AdvanceLicenseEntitlementEntity Create(long licenseId, int entitlementRm)
    {
        return new AdvanceLicenseEntitlementEntity
        {
            Id = licenseId,
            EntitlementRm = entitlementRm
        };
    }
}
