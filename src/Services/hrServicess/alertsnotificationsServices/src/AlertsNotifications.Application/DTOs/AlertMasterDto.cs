namespace AlertsNotifications.Application.DTOs;

public class AlertMasterDto
{
    public decimal AlertId { get; set; }
    public string AlertApps { get; set; } = string.Empty;
    public string AlertName { get; set; } = string.Empty;
    public string AlertType { get; set; } = string.Empty;
    public string AlertDesc { get; set; } = string.Empty;
    public string? AlertToDesc { get; set; }
    public string? AlertCcDesc { get; set; }
    public string? AlertGradeCat { get; set; }
    public string? AlertUnitSpecific { get; set; }
}
