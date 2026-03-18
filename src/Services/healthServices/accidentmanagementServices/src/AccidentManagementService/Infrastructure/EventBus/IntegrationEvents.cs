using System;

namespace AccidentManagementService.Infrastructure.EventBus.Integration
{
    /// <summary>
    /// Base class for integration events
    /// </summary>
    public abstract class IntegrationEvent
    {
        public Guid EventId { get; protected set; }
        public DateTime CreatedTime { get; protected set; }

        protected IntegrationEvent()
        {
            EventId = Guid.NewGuid();
            CreatedTime = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Event published when an accident report is created
    /// </summary>
    public class AccidentReportCreatedIntegrationEvent : IntegrationEvent
    {
        public long AccidentReportId { get; set; }
        public Guid AccidentReportGuid { get; set; }
        public long AccidentNumber { get; set; }
        public string CompanyCode { get; set; } = null!;
        public string InjuredPersonName { get; set; } = null!;
        public string AccidentLocation { get; set; } = null!;
        public DateTime AccidentDateTime { get; set; }
        public long SeverityId { get; set; }
        public long StatusId { get; set; }
        public string EnteredUserId { get; set; } = null!;
    }

    /// <summary>
    /// Event published when accident status changes
    /// </summary>
    public class AccidentStatusChangedIntegrationEvent : IntegrationEvent
    {
        public long AccidentReportId { get; set; }
        public Guid AccidentReportGuid { get; set; }
        public long AccidentNumber { get; set; }
        public long OldStatusId { get; set; }
        public long NewStatusId { get; set; }
        public string? OldStatusName { get; set; }
        public string? NewStatusName { get; set; }
        public string ChangedBy { get; set; } = null!;
        public DateTime ChangedAt { get; set; }
    }

    /// <summary>
    /// Event published when accident severity changes
    /// </summary>
    public class AccidentSeverityChangedIntegrationEvent : IntegrationEvent
    {
        public long AccidentReportId { get; set; }
        public Guid AccidentReportGuid { get; set; }
        public long AccidentNumber { get; set; }
        public long OldSeverityId { get; set; }
        public long NewSeverityId { get; set; }
        public string? OldSeverityName { get; set; }
        public string? NewSeverityName { get; set; }
        public string ChangedBy { get; set; } = null!;
        public DateTime ChangedAt { get; set; }
    }

    /// <summary>
    /// Event published when accident details are updated
    /// </summary>
    public class AccidentDetailsUpdatedIntegrationEvent : IntegrationEvent
    {
        public long AccidentReportId { get; set; }
        public Guid AccidentReportGuid { get; set; }
        public long AccidentNumber { get; set; }
        public string AccidentLocation { get; set; } = null!;
        public DateTime AccidentDateTime { get; set; }
        public string BodyPart { get; set; } = null!;
        public long InjuryCategoryId { get; set; }
        public long InjuryNatureId { get; set; }
        public string MedicalCentreName { get; set; } = null!;
        public string TreatmentGiven { get; set; } = null!;
        public string UpdatedBy { get; set; } = null!;
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// Event published when an accident report is deleted
    /// </summary>
    public class AccidentReportDeletedIntegrationEvent : IntegrationEvent
    {
        public long AccidentReportId { get; set; }
        public Guid AccidentReportGuid { get; set; }
        public long AccidentNumber { get; set; }
        public string DeletedBy { get; set; } = null!;
        public DateTime DeletedAt { get; set; }
    }

    /// <summary>
    /// Event published when an accident report is restored
    /// </summary>
    public class AccidentReportRestoredIntegrationEvent : IntegrationEvent
    {
        public long AccidentReportId { get; set; }
        public Guid AccidentReportGuid { get; set; }
        public long AccidentNumber { get; set; }
        public string RestoredBy { get; set; } = null!;
        public DateTime RestoredAt { get; set; }
    }

    /// <summary>
    /// Event published when a new injury category is created
    /// </summary>
    public class InjuryCategoryCreatedIntegrationEvent : IntegrationEvent
    {
        public long CategoryId { get; set; }
        public Guid CategoryGuid { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }

    /// <summary>
    /// Event published when a new injury nature is created
    /// </summary>
    public class InjuryNatureCreatedIntegrationEvent : IntegrationEvent
    {
        public long NatureId { get; set; }
        public Guid NatureGuid { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }

    /// <summary>
    /// Event published when an accident severity is created
    /// </summary>
    public class AccidentSeverityCreatedIntegrationEvent : IntegrationEvent
    {
        public long SeverityId { get; set; }
        public Guid SeverityGuid { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }

    /// <summary>
    /// Event published when an accident status is created
    /// </summary>
    public class AccidentStatusCreatedIntegrationEvent : IntegrationEvent
    {
        public long StatusId { get; set; }
        public Guid StatusGuid { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
