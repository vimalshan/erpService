using AuditService.Domain.Common;

namespace AuditService.Domain.Entities;

/// <summary>
/// AUDIT_OBSERVATIONAPP - Observation approval record.
/// </summary>
public sealed class AuditObservationApp : BaseEntity
{
    private AuditObservationApp() { }

    public decimal AppId { get; private set; }
    public decimal AppObvId { get; private set; }
    public decimal AppEscSysId { get; private set; }
    public char AppStatus { get; private set; }
    public DateTime AppOn { get; private set; }
    public string? AppRemarks { get; private set; }
    public char AppObvStatus { get; private set; }
    public DateTime AppDueDate { get; private set; }
    public DateTime AppRevDueDate { get; private set; }

    public static AuditObservationApp Create(
        decimal appId, decimal obvId, decimal escSysId,
        char appStatus, char obvStatus, DateTime dueDate, DateTime revDueDate)
    {
        return new AuditObservationApp
        {
            AppId = appId,
            AppObvId = obvId,
            AppEscSysId = escSysId,
            AppStatus = appStatus,
            AppOn = DateTime.UtcNow,
            AppObvStatus = obvStatus,
            AppDueDate = dueDate,
            AppRevDueDate = revDueDate
        };
    }

    public void Approve(string? remarks)
    {
        AppStatus = 'A';
        AppRemarks = remarks;
        AppObvStatus = 'C';
        AppOn = DateTime.UtcNow;
    }

    public void Reject(string? remarks)
    {
        AppStatus = 'R';
        AppRemarks = remarks;
        AppOn = DateTime.UtcNow;
    }
}
