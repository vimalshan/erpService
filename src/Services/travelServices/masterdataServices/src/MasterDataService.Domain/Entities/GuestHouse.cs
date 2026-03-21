using MasterDataService.Domain.Common;
using MasterDataService.Domain.Events;

namespace MasterDataService.Domain.Entities;

public class GuestHouse : AuditableEntity
{
    public long AdminCode { get; private set; }
    public string GuestHouseName { get; private set; } = string.Empty;
    public string Type { get; private set; } = "S";
    public long DailyAmount { get; private set; }

    private readonly List<GuestHouseRoom> _rooms = new();
    public IReadOnlyCollection<GuestHouseRoom> Rooms => _rooms.AsReadOnly();

    private GuestHouse() { }

    public GuestHouse(long adminCode, string guestHouseName, long dailyAmount, string type = "S")
    {
        AdminCode = adminCode;
        GuestHouseName = guestHouseName ?? throw new ArgumentNullException(nameof(guestHouseName));
        DailyAmount = dailyAmount;
        Type = type;
        AddDomainEvent(new GuestHouseCreatedEvent(this));
    }

    public void UpdateDetails(string name, long dailyAmount)
    {
        GuestHouseName = name ?? throw new ArgumentNullException(nameof(name));
        DailyAmount = dailyAmount;
        AddDomainEvent(new GuestHouseUpdatedEvent(this));
    }

    public void AddRoom(GuestHouseRoom room)
    {
        _rooms.Add(room);
    }
}
