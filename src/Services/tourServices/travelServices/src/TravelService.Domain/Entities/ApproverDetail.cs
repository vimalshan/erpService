using TravelService.Domain.Common;

namespace TravelService.Domain.Entities;

public class ApproverDetail : Entity<string>
{
    public string TourPlanId { get; private set; } = string.Empty;
    public string Source { get; private set; } = string.Empty;
    public string SourceId { get; private set; } = string.Empty;
    public string ApprovedStatus { get; private set; } = string.Empty;
    public string ApproverSysId { get; private set; } = string.Empty;
    public DateTime ApprovedOn { get; private set; }
    public string Remarks { get; private set; } = string.Empty;
    public string ApproverType { get; private set; } = string.Empty;

    protected ApproverDetail() { }

    public static ApproverDetail Create(
        string id, string tourPlanId, string source, string sourceId,
        string approvedStatus, string approverSysId, string remarks, string approverType)
        => new()
        {
            Id = id,
            TourPlanId = tourPlanId,
            Source = source,
            SourceId = sourceId,
            ApprovedStatus = approvedStatus,
            ApproverSysId = approverSysId,
            ApprovedOn = DateTime.UtcNow,
            Remarks = remarks,
            ApproverType = approverType
        };
}
