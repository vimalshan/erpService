using MasterDataService.Domain.Common;

namespace MasterDataService.Domain.Entities;

public class GuestRoomAvailability : BaseEntity
{
    public long FloorNumber { get; private set; }
    public long RoomNumber { get; private set; }
    public char RoomStatus { get; private set; }
    public string? FloorValue { get; private set; }

    private GuestRoomAvailability() { }

    public GuestRoomAvailability(long floorNumber, long roomNumber, char roomStatus, string? floorValue = null)
    {
        FloorNumber = floorNumber;
        RoomNumber = roomNumber;
        RoomStatus = roomStatus;
        FloorValue = floorValue;
    }

    public void SetAvailable() => RoomStatus = 'A';
    public void SetOccupied() => RoomStatus = 'O';
}
