using BookingService.Domain.Common;

namespace BookingService.Domain.Entities;

public class BookRequestTicket : BaseEntity
{
    public string BookTicketId { get; set; } = null!;
    public string MainId { get; set; } = null!;
    public string ModeId { get; set; } = null!;
    public string ClassId { get; set; } = null!;
    public string Type { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public string StartTime { get; set; } = null!;
    public string StartCityId { get; set; } = null!;
    public string StartCity { get; set; } = null!;
    public string EndCityId { get; set; } = null!;
    public string EndCity { get; set; } = null!;
    public string ConfirmationNo { get; set; } = null!;
    public string ApprovalStatus { get; set; } = null!;
    public string LastModifiedBy { get; set; } = null!;
    public string BudgetCost { get; set; } = null!;
    public string AdminRemarks { get; set; } = null!;
    public string SpecialSanction { get; set; } = null!;
    public string SpecialSanctionReason { get; set; } = null!;

    // Navigation
    public BookRequestMain Main { get; set; } = null!;
}
