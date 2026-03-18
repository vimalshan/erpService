using AlertsNotifications.Domain.Common;

namespace AlertsNotifications.Domain.Entities;

public class CircularSignatory : BaseEntity
{
    public long CircularSignatoryId { get; set; }
    public long CircularSignatoryUnitId { get; set; }
    public long CircularSignatoryTypeId { get; set; }
    public long CircularSignatorySignId { get; set; }
    public char CircularSignatoryStatus { get; set; }
    public long CircularSignatoryCreatedBy { get; set; }
    public DateTime CircularSignatoryCreatedOn { get; set; }
}
