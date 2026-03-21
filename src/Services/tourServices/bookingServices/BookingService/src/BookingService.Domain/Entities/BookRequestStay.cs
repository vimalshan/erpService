using BookingService.Domain.Common;

namespace BookingService.Domain.Entities;

public class BookRequestStay : BaseEntity
{
    public string BookStayId { get; set; } = null!;
    public string MainId { get; set; } = null!;
    public string CityId { get; set; } = null!;
    public string City { get; set; } = null!;
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public string ConfirmationNo { get; set; } = null!;
    public string LastModifiedBy { get; set; } = null!;

    // Navigation
    public BookRequestMain Main { get; set; } = null!;
}
