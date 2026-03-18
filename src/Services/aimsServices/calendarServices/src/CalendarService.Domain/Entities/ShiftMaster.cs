using CalendarService.Domain.Common;
using CalendarService.Domain.Events;
using CalendarService.Domain.ValueObjects;

namespace CalendarService.Domain.Entities;

public class ShiftMaster : BaseEntity
{
    public int ShiftId { get; private set; }
    public string ShiftCode { get; private set; } = string.Empty;
    public string ShiftName { get; private set; } = string.Empty;
    public TimeOnly ShiftInTime { get; private set; }
    public TimeOnly ShiftOutTime { get; private set; }
    public decimal ShiftDuration { get; private set; }
    public long LastModifiedBy { get; private set; }
    public DateTime LastModifiedOn { get; private set; }

    public ICollection<ShiftTimeMaster> TimeMasters { get; private set; } = [];
    public ICollection<ShiftException> Exceptions { get; private set; } = [];

    private ShiftMaster() { }

    public static ShiftMaster Create(int id, string code, string name, TimeOnly inTime, TimeOnly outTime, long modifiedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        var duration = (decimal)outTime.ToTimeSpan().Subtract(inTime.ToTimeSpan()).TotalHours;

        var entity = new ShiftMaster
        {
            ShiftId = id,
            ShiftCode = code,
            ShiftName = name,
            ShiftInTime = inTime,
            ShiftOutTime = outTime,
            ShiftDuration = Math.Round(duration, 2),
            LastModifiedBy = modifiedBy,
            LastModifiedOn = DateTime.UtcNow
        };

        entity.RaiseDomainEvent(new ShiftCreatedEvent(entity.ShiftId, entity.ShiftCode, entity.ShiftName));
        return entity;
    }

    public void Update(string name, TimeOnly inTime, TimeOnly outTime, long modifiedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ShiftName = name;
        ShiftInTime = inTime;
        ShiftOutTime = outTime;
        ShiftDuration = Math.Round((decimal)outTime.ToTimeSpan().Subtract(inTime.ToTimeSpan()).TotalHours, 2);
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }
}
