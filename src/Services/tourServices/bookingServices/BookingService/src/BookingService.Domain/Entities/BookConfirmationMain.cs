namespace BookingService.Domain.Entities;

public class BookConfirmationMain
{
    public string? ConfId { get; set; }
    public string? Mode { get; set; }
    public string? BookId { get; set; }
    public string? RefId { get; set; }
    public DateTime? ConfirmationDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? BookType { get; set; }
    public string? Status { get; set; }
    public string? Remarks { get; set; }
    public string? AdminUnit { get; set; }
    public string? PaymentBatchNo { get; set; }
    public string? ContractId { get; set; }
    public string? VendorId { get; set; }
    public string? TripCode { get; set; }
}
