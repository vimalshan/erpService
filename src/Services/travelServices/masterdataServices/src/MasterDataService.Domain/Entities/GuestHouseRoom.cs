using MasterDataService.Domain.Common;

namespace MasterDataService.Domain.Entities;

public class GuestHouseRoom : AuditableEntity
{
    public long GuestHouseCode { get; private set; }
    public long RoomSerial { get; private set; }
    public long NumberOfPersons { get; private set; }
    public long RoomNumber { get; private set; }
    public long Floor { get; private set; }

    public GuestHouse? GuestHouse { get; private set; }

    private GuestHouseRoom() { }

    public GuestHouseRoom(long guestHouseCode, long roomSerial, long numberOfPersons, long roomNumber, long floor)
    {
        GuestHouseCode = guestHouseCode;
        RoomSerial = roomSerial;
        NumberOfPersons = numberOfPersons;
        RoomNumber = roomNumber;
        Floor = floor;
    }

    public void UpdateCapacity(long numberOfPersons) => NumberOfPersons = numberOfPersons;
}
