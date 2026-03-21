namespace BookingService.Domain.Entities;

public class BookConfirmationTicket
{
    public string? ConfTicketId { get; set; }
    public string? BookId { get; set; }
    public string? TicketId { get; set; }
    public DateTime? EntryDate { get; set; }
    public DateTime? DepartureDate { get; set; }
    public string Cost { get; set; } = null!;
    public string? ConfirmationMainId { get; set; }
}
