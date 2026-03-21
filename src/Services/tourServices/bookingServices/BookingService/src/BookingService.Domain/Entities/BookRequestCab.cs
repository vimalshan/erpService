using BookingService.Domain.Common;

namespace BookingService.Domain.Entities;

public class BookRequestCab : BaseEntity
{
    public string BookCabId { get; set; } = null!;
    public string MainId { get; set; } = null!;
    public string PickupLocation { get; set; } = null!;
    public string DropLocation { get; set; } = null!;
    public DateTime PickupDate { get; set; }
    public string? CarType { get; set; }
    public string Preference { get; set; } = null!;
    public string TripType { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string ConfirmationNo { get; set; } = null!;
    public string LastModifiedBy { get; set; } = null!;
    public string? Nature { get; set; }

    // Navigation
    public BookRequestMain Main { get; set; } = null!;
}
