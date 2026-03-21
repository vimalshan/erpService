namespace BookingService.Domain.Entities;

public class BookRequestOther
{
    public string BookOtherId { get; set; } = null!;
    public string BookId { get; set; } = null!;
    public string BookingFor { get; set; } = null!;
    public string Gender { get; set; } = null!;
    public string Age { get; set; } = null!;
    public string ContactNo { get; set; } = null!;
    public string ApprovedBy { get; set; } = null!;
    public DateTime? ApprovedOn { get; set; }

    // Navigation
    public BookRequestMain Main { get; set; } = null!;
}
