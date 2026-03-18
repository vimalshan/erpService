using CalendarService.Domain.Common;
using CalendarService.Domain.Events;
using CalendarService.Domain.ValueObjects;

namespace CalendarService.Domain.Entities;

public class HolidayMaster : BaseEntity
{
    public int HolidayId { get; private set; }
    public DateTime HolidayDate { get; private set; }
    public string HolidayDescription { get; private set; } = string.Empty;
    public HolidayType HolidayType { get; private set; }
    public int? HolidayUnit { get; private set; }
    public long LastModifiedBy { get; private set; }
    public DateTime LastModifiedOn { get; private set; }

    private HolidayMaster() { }

    public static HolidayMaster Create(int id, DateTime date, string description, HolidayType type, long modifiedBy, int? unitId = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        var entity = new HolidayMaster
        {
            HolidayId = id,
            HolidayDate = date,
            HolidayDescription = description,
            HolidayType = type,
            HolidayUnit = unitId,
            LastModifiedBy = modifiedBy,
            LastModifiedOn = DateTime.UtcNow
        };

        entity.RaiseDomainEvent(new HolidayCreatedEvent(entity.HolidayId, entity.HolidayDate, entity.HolidayDescription));
        return entity;
    }

    public void Update(string description, HolidayType type, long modifiedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        HolidayDescription = description;
        HolidayType = type;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }
}
