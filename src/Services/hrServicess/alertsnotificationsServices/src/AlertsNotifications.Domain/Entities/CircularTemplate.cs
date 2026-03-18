using AlertsNotifications.Domain.Common;

namespace AlertsNotifications.Domain.Entities;

public class CircularTemplate : BaseEntity
{
    public long CircularTemplateId { get; set; }
    public long CircularTemplateApplyToUnit { get; set; }
    public long CircularTemplateUnitId { get; set; }
    public long CircularTemplateTypeId { get; set; }
    public string CircularTemplateName { get; set; } = string.Empty;
    public string CircularTemplateHtml { get; set; } = string.Empty;
    public DateTime? CircularTemplateClsDate { get; set; }
    public long CircularTemplateModifiedBy { get; set; }
    public DateTime CircularTemplateModifiedOn { get; set; }
}
