using AlertsNotifications.Domain.Common;

namespace AlertsNotifications.Domain.Entities;

public class AlertMaster : BaseEntity
{
    public decimal AlertId { get; set; }
    public string AlertApps { get; set; } = string.Empty;
    public string AlertName { get; set; } = string.Empty;
    public string AlertType { get; set; } = string.Empty;
    public string AlertDesc { get; set; } = string.Empty;
    public string? AlertToDesc { get; set; }
    public string? AlertCcDesc { get; set; }
    public string? AlertGradeCat { get; set; }
    public char? AlertUnitSpecific { get; set; }
}
