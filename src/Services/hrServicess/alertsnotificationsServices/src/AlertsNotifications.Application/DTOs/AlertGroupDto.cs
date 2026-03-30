namespace AlertsNotifications.Application.DTOs;

public class AlertGroupDto
{
    public decimal AlertGroupId { get; set; }
    public string AlertGroupName { get; set; } = string.Empty;
    public string AlertGroupType { get; set; } = string.Empty;
    public long CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public long? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
}
