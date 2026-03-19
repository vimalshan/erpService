using MasterDataService.Domain.Common;
using MasterDataService.Domain.Events;
using MasterDataService.Domain.ValueObjects;

namespace MasterDataService.Domain.Entities;

public class LocationScanParam : AuditableEntity<long>
{
    public long LocationId { get; private set; }
    public DateRange EffectivePeriod { get; private set; } = null!;

    private LocationScanParam() { }

    public static LocationScanParam Create(long id, long locationId, DateTime effectiveDate, DateTime? closingDate = null)
    {
        var entity = new LocationScanParam
        {
            Id = id,
            LocationId = locationId,
            EffectivePeriod = DateRange.Create(effectiveDate, closingDate),
            CreatedAt = DateTime.UtcNow
        };

        entity.AddDomainEvent(new LocationScanParamCreatedEvent(entity.Id, entity.LocationId));
        return entity;
    }

    public void UpdatePeriod(DateTime effectiveDate, DateTime? closingDate)
    {
        EffectivePeriod = DateRange.Create(effectiveDate, closingDate);
        LastModifiedAt = DateTime.UtcNow;
    }
}
