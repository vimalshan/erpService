namespace BookingService.Domain.Entities;

public class BookConfirmationStay
{
    public string? StayId { get; set; }
    public string? BookId { get; set; }
    public DateTime? CheckInDate { get; set; }
    public DateTime? CheckOutDate { get; set; }
    public string GuestHouseSiteId { get; set; } = null!;
    public string ConfirmationMainId { get; set; } = null!;
}
