using TourPlanService.Domain.Common;

namespace TourPlanService.Domain.Entities;

/// <summary>TOURPLAN_ADVANCE - Travel Advance</summary>
public sealed class TourAdvance : BaseEntity
{
    private TourAdvance() { }

    public string AdvId { get; private set; } = default!;
    public string AdvTpId { get; private set; } = default!;
    public string AdvAmount { get; private set; } = default!;
    public string AdvJvId { get; private set; } = default!;
    public string AdvRemarks { get; private set; } = default!;
    public string AdvAppStatus { get; private set; } = default!;
    public string? AdvAppBy { get; private set; }
    public DateTime? AdvAppOn { get; private set; }
    public string AdvCurrency { get; private set; } = default!;
    public string AdvRate { get; private set; } = default!;
    public string AdvTotal { get; private set; } = default!;
    public DateTime AdvModifiedOn { get; private set; }
    public string? AdvAppRemarks { get; private set; }
    public string? AdvFinRemarks { get; private set; }
    public string? AdvType { get; private set; }
    public string? AdvPayMode { get; private set; }

    // Navigation
    public TourPlan TourPlan { get; private set; } = default!;

    public static TourAdvance Create(
        string advId, string tpId, string amount, string jvId,
        string remarks, string appStatus, string currency, string rate, string total)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(advId);
        return new TourAdvance
        {
            AdvId = advId,
            AdvTpId = tpId,
            AdvAmount = amount,
            AdvJvId = jvId,
            AdvRemarks = remarks,
            AdvAppStatus = appStatus,
            AdvCurrency = currency,
            AdvRate = rate,
            AdvTotal = total,
            AdvModifiedOn = DateTime.UtcNow
        };
    }
}
