using AuditService.Domain.Common;
using AuditService.Domain.Events;

namespace AuditService.Domain.Entities;

/// <summary>
/// AUDIT_OBSERVATION - Audit observation record.
/// </summary>
public sealed class AuditObservation : BaseEntity
{
    private AuditObservation() { }

    public long ObvId { get; private set; }
    public long ObvAuditId { get; private set; }
    public string ObvTitle { get; private set; } = string.Empty;
    public string ObvDescription { get; private set; } = string.Empty;
    public char ObvRisk { get; private set; }
    public long ObvAuditee { get; private set; }
    public long ObvEsc1 { get; private set; }
    public long ObvEsc2 { get; private set; }
    public string ObvManComments { get; private set; } = string.Empty;
    public string? ObvImplication { get; private set; }
    public char ObvStatus { get; private set; }
    public DateTime ObvOrgDueDate { get; private set; }
    public DateTime? ObvOrgRev1Date { get; private set; }
    public DateTime? ObvOrgRev2Date { get; private set; }
    public string? ObvDelay1Remarks { get; private set; }
    public string? ObvDelay2Remarks { get; private set; }
    public long ObvCreatedBy { get; private set; }
    public DateTime ObvCreatedOn { get; private set; }
    public long ObvModifiedBy { get; private set; }
    public DateTime ObvModifiedOn { get; private set; }
    public DateTime ObvCompletedOn { get; private set; }
    public string ObvLocation { get; private set; } = string.Empty;
    public string ObvAuditorName { get; private set; } = string.Empty;
    public string ObvRemarks { get; private set; } = string.Empty;
    public char? ObvAppStatus { get; private set; }
    public char? ObvEntryStatus { get; private set; }
    public char? ObvRepeatFlag { get; private set; }
    public char? ObvDupFlag { get; private set; }
    public decimal? ObvProcess { get; private set; }

    public static AuditObservation Create(
        long obvId, long auditId, string title, string description,
        char risk, long auditee, long esc1, long esc2,
        string manComments, DateTime orgDueDate, string location,
        string auditorName, string remarks, long createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        if (!new[] { 'A', 'B', 'C', 'D' }.Contains(risk))
            throw new ArgumentException("Risk must be A, B, C, or D.");

        var observation = new AuditObservation
        {
            ObvId = obvId,
            ObvAuditId = auditId,
            ObvTitle = title,
            ObvDescription = description,
            ObvRisk = risk,
            ObvAuditee = auditee,
            ObvEsc1 = esc1,
            ObvEsc2 = esc2,
            ObvManComments = manComments,
            ObvStatus = 'P',
            ObvOrgDueDate = orgDueDate,
            ObvCreatedBy = createdBy,
            ObvCreatedOn = DateTime.UtcNow,
            ObvModifiedBy = createdBy,
            ObvModifiedOn = DateTime.UtcNow,
            ObvCompletedOn = DateTime.MaxValue,
            ObvLocation = location,
            ObvAuditorName = auditorName,
            ObvRemarks = remarks
        };

        observation.AddDomainEvent(new ObservationCreatedEvent(observation.ObvId, observation.ObvAuditId, observation.ObvTitle));
        return observation;
    }

    public void UpdateStatus(char newStatus, long modifiedBy)
    {
        if (!new[] { 'P', 'R', 'C' }.Contains(newStatus))
            throw new ArgumentException("Status must be P, R, or C.");

        var oldStatus = ObvStatus;
        ObvStatus = newStatus;
        ObvModifiedBy = modifiedBy;
        ObvModifiedOn = DateTime.UtcNow;

        if (newStatus == 'C') ObvCompletedOn = DateTime.UtcNow;

        AddDomainEvent(new ObservationStatusChangedEvent(ObvId, ObvAuditId, oldStatus, newStatus));
    }

    public void ReviseDate(DateTime rev1Date, string delay1Remarks, long modifiedBy)
    {
        ObvOrgRev1Date = rev1Date;
        ObvDelay1Remarks = delay1Remarks;
        ObvStatus = 'R';
        ObvModifiedBy = modifiedBy;
        ObvModifiedOn = DateTime.UtcNow;
    }
}
