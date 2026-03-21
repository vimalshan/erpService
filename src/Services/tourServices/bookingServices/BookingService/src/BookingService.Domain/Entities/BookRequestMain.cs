using BookingService.Domain.Common;

namespace BookingService.Domain.Entities;

public class BookRequestMain : BaseEntity, IAuditableEntity
{
    public string BookMainId { get; set; } = null!;
    public string TpStatus { get; set; } = null!;
    public string TpId { get; set; } = null!;
    public string EmployeeSysId { get; set; } = null!;
    public string Through { get; set; } = null!;
    public string AdminId { get; set; } = null!;
    public string Remarks { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string ApprovalStatus { get; set; } = null!;
    public string ConfirmationStatus { get; set; } = null!;
    public string? ProofType { get; set; }
    public string? FoodPreference { get; set; }
    public string? BudgetedCost { get; set; }
    public string? EnteredBy { get; set; }
    public DateTime? EnteredOn { get; set; }
    public string? EmployeeCalendarId { get; set; }

    // Navigation properties
    public ICollection<BookRequestTicket> Tickets { get; set; } = [];
    public ICollection<BookRequestStay> Stays { get; set; } = [];
    public ICollection<BookRequestCab> Cabs { get; set; } = [];
    public ICollection<BookRequestCostCentre> CostCentres { get; set; } = [];
    public ICollection<BookRequestOther> Others { get; set; } = [];
    public ICollection<BookRequestConfirmation> Confirmations { get; set; } = [];
}
