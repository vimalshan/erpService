using AlertsNotifications.Domain.Common;

namespace AlertsNotifications.Domain.Entities;

public class AlertGroup : AuditableEntity
{
    public decimal AlertGroupId { get; set; }
    public string AlertGroupName { get; set; } = string.Empty;
    public char AlertGroupType { get; set; }
}
