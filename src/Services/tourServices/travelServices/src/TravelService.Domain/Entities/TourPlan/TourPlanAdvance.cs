using TravelService.Domain.Common;

namespace TravelService.Domain.Entities.TourPlan;

public class TourPlanAdvance : Entity<string>
{
    public string TourPlanId { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public string JvId { get; private set; } = string.Empty;
    public string Remarks { get; private set; } = string.Empty;
    public string ApprovalStatus { get; private set; } = string.Empty;
    public string? ApprovedBy { get; private set; }
    public DateTime? ApprovedOn { get; private set; }
    public string Currency { get; private set; } = "INR";
    public decimal Rate { get; private set; }
    public decimal TotalInr { get; private set; }
    public DateTime LastModifiedOn { get; private set; }
    public string? ApproverRemarks { get; private set; }
    public string? FinanceRemarks { get; private set; }
    public string? Type { get; private set; }
    public string? PayMode { get; private set; }

    protected TourPlanAdvance() { }

    public static TourPlanAdvance Create(
        string id, string tourPlanId, decimal amount, string jvId,
        string remarks, string currency, decimal rate, decimal totalInr,
        string? type = "N", string? payMode = null)
        => new()
        {
            Id = id,
            TourPlanId = tourPlanId,
            Amount = amount,
            JvId = jvId,
            Remarks = remarks,
            ApprovalStatus = "P",
            Currency = currency,
            Rate = rate,
            TotalInr = totalInr,
            LastModifiedOn = DateTime.UtcNow,
            Type = type,
            PayMode = payMode
        };
}
