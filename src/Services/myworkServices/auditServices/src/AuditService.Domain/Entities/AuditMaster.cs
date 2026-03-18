using AuditService.Domain.Common;
using AuditService.Domain.Enums;
using AuditService.Domain.Events;

namespace AuditService.Domain.Entities;

/// <summary>
/// AUDIT_MASTER - Core audit record (Aggregate Root).
/// </summary>
public sealed class AuditMaster : AggregateRoot
{
    private readonly List<AuditObservation> _observations = new();

    private AuditMaster() { }

    public long AuditId { get; private set; }
    public string AuditName { get; private set; } = string.Empty;
    public long AuditUnit { get; private set; }
    public DateTime AuditFrom { get; private set; }
    public DateTime AuditTo { get; private set; }
    public string AuditDefLocation { get; private set; } = string.Empty;
    public char AuditStatus { get; private set; }
    public decimal AuditCreatedBy { get; private set; }
    public DateTime AuditCreatedOn { get; private set; }
    public decimal? AuditUpdatedBy { get; private set; }
    public DateTime? AuditUpdatedOn { get; private set; }
    public decimal? AuditPlanYear { get; private set; }
    public string? AuditFile1 { get; private set; }
    public string? AuditFile2 { get; private set; }
    public string? AuditFile3 { get; private set; }
    public DateTime AuditPlanFrom { get; private set; }
    public DateTime AuditPlanTo { get; private set; }
    public char? AuditCompleted { get; private set; }
    public string? AuditFirmName { get; private set; }
    public DateTime? AuditFieldFrom { get; private set; }
    public DateTime? AuditFieldTo { get; private set; }
    public decimal? AuditCordId { get; private set; }
    public long? AuditProcess { get; private set; }

    public IReadOnlyCollection<AuditObservation> Observations => _observations.AsReadOnly();

    public static AuditMaster Create(
        long auditId, string auditName, long auditUnit,
        DateTime auditFrom, DateTime auditTo, string defLocation,
        DateTime planFrom, DateTime planTo, decimal createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(auditName);
        if (auditFrom >= auditTo) throw new ArgumentException("AuditFrom must be before AuditTo.");

        var audit = new AuditMaster
        {
            AuditId = auditId,
            AuditName = auditName,
            AuditUnit = auditUnit,
            AuditFrom = auditFrom,
            AuditTo = auditTo,
            AuditDefLocation = defLocation,
            AuditStatus = 'A',
            AuditCreatedBy = createdBy,
            AuditCreatedOn = DateTime.UtcNow,
            AuditPlanFrom = planFrom,
            AuditPlanTo = planTo
        };

        audit.AddDomainEvent(new AuditCreatedEvent(audit.AuditId, audit.AuditName));
        return audit;
    }

    public void Update(string auditName, string defLocation, DateTime auditFrom, DateTime auditTo, decimal updatedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(auditName);
        AuditName = auditName;
        AuditDefLocation = defLocation;
        AuditFrom = auditFrom;
        AuditTo = auditTo;
        AuditUpdatedBy = updatedBy;
        AuditUpdatedOn = DateTime.UtcNow;
    }

    public void MarkCompleted(decimal updatedBy)
    {
        AuditCompleted = 'Y';
        AuditStatus = 'C';
        AuditUpdatedBy = updatedBy;
        AuditUpdatedOn = DateTime.UtcNow;
        AddDomainEvent(new AuditStatusChangedEvent(AuditId, 'C'));
    }

    public void AttachFiles(string? file1, string? file2, string? file3)
    {
        AuditFile1 = file1;
        AuditFile2 = file2;
        AuditFile3 = file3;
    }

    public void AddObservation(AuditObservation observation)
    {
        ArgumentNullException.ThrowIfNull(observation);
        _observations.Add(observation);
    }
}
