using SciTransactional.Domain.Common;
using SciTransactional.Domain.Events;

namespace SciTransactional.Domain.Entities;

public sealed class AdvanceLicenseEntity : AggregateRoot<long>
{
    public string? LicenseNo { get; private set; }
    public int? FgCode { get; private set; }
    public decimal? ExportObligationAmount { get; private set; }
    public decimal? ExportAmount { get; private set; }

    private readonly List<AdvanceLicenseEntitlementEntity> _entitlements = [];
    public IReadOnlyCollection<AdvanceLicenseEntitlementEntity> Entitlements => _entitlements.AsReadOnly();

    private AdvanceLicenseEntity() { }

    public static AdvanceLicenseEntity Create(
        long licenseId, string? licenseNo, int? fgCode,
        decimal? eoAmount, decimal? expAmount)
    {
        var entity = new AdvanceLicenseEntity
        {
            Id = licenseId,
            LicenseNo = licenseNo,
            FgCode = fgCode,
            ExportObligationAmount = eoAmount,
            ExportAmount = expAmount
        };
        entity.AddDomainEvent(new LicenseCreatedEvent(licenseId, licenseNo));
        return entity;
    }

    public void Update(string? licenseNo, int? fgCode, decimal? eoAmount, decimal? expAmount)
    {
        LicenseNo = licenseNo ?? LicenseNo;
        FgCode = fgCode ?? FgCode;
        ExportObligationAmount = eoAmount ?? ExportObligationAmount;
        ExportAmount = expAmount ?? ExportAmount;
        AddDomainEvent(new LicenseUpdatedEvent(Id, LicenseNo));
    }

    public void AddEntitlement(AdvanceLicenseEntitlementEntity entitlement)
    {
        _entitlements.Add(entitlement);
    }
}
