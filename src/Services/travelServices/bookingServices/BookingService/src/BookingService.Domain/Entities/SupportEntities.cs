namespace BookingService.Domain.Entities;

public class CabPick
{
    public string? CityFrom { get; set; }
    public string? CityTo { get; set; }
    public string? PickFlag { get; set; }
}

public class RoomAvailTemp
{
    public long? BkGhcode { get; set; }
    public string? BkRoomno { get; set; }
    public DateTime? BkFrodat { get; set; }
    public DateTime? BkTodat { get; set; }
    public int? TotalHrOccupied { get; set; }
}
