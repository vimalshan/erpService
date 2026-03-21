using BookingService.Domain.Common;

namespace BookingService.Domain.Entities;

public class BookRequestConfirmation : BaseEntity
{
    public string BookConfId { get; set; } = null!;
    public string Mode { get; set; } = null!;
    public string BookId { get; set; } = null!;
    public string RefId { get; set; } = null!;
    public DateTime ConfirmationDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Cost { get; set; } = null!;
    public string ClassId { get; set; } = null!;
    public string VendorId { get; set; } = null!;
    public string GuestHouseSiteId { get; set; } = null!;
    public string CabConfirmationId { get; set; } = null!;
    public string RefundCost { get; set; } = null!;
    public DateTime CancelDate { get; set; }
    public string DebitMemoBatch { get; set; } = null!;
    public string CreditMemoBatch { get; set; } = null!;
    public string AdminRemarks { get; set; } = null!;
    public string LastModifiedBy { get; set; } = null!;
    public string ConfirmedBy { get; set; } = null!;
    public string? VendorSelf { get; set; }
    public string? Attachment { get; set; }
    public string ApprovalStatus { get; set; } = null!;
    public string? EnteredById { get; set; }
    public string OldRequestId { get; set; } = null!;
    public string? AirlineVendorId { get; set; }
    public string? AirlinePnrNumber { get; set; }

    // Navigation
    public BookRequestMain Main { get; set; } = null!;
}
