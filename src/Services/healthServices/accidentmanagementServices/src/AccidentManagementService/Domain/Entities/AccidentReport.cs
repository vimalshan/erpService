using System;
using System.Collections.Generic;

namespace AccidentManagementService.Domain.Entities
{
    /// <summary>
    /// Aggregate Root: Represents a complete accident report (DAILY_ACC_FIR)
    /// Contains all information about an accident incident
    /// </summary>
    public class AccidentReport : DomainEntity
    {
        #region Properties

        // Core identity
        public AccidentNumber AccidentNumber { get; private set; } = null!;
        public string CompanyCode { get; private set; } = null!;

        // Personnel information
        public EmployeeInfo? EmployeeInfo { get; private set; }
        public ContractorInfo? ContractorInfo { get; private set; }
        public InjuredPersonInfo InjuredPersonInfo { get; private set; } = null!;

        // Accident details
        public AccidentCircumstances AccidentCircumstances { get; private set; } = null!;
        public InjuryDetails InjuryDetails { get; private set; } = null!;

        // Medical/Treatment
        public TreatmentInfo TreatmentInfo { get; private set; } = null!;

        // Status & Severity
        public long SeverityId { get; private set; }
        public long StatusId { get; private set; }

        // Reporting
        public string EnteredUserId { get; private set; } = null!;
        public long EnteredUserNumber { get; private set; }
        public DateTime EnteredDate { get; private set; }

        // Domain events
        private readonly List<DomainEvent> _domainEvents = new();
        public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        #endregion

        #region Constructors

        private AccidentReport() { }

        public AccidentReport(
            AccidentNumber accidentNumber,
            string companyCode,
            InjuredPersonInfo injuredPersonInfo,
            AccidentCircumstances accidentCircumstances,
            InjuryDetails injuryDetails,
            TreatmentInfo treatmentInfo,
            long severityId,
            long statusId,
            string enteredUserId,
            long enteredUserNumber,
            EmployeeInfo? employeeInfo = null,
            ContractorInfo? contractorInfo = null)
        {
            // Validate required fields
            if (accidentNumber == null)
                throw new ArgumentNullException(nameof(accidentNumber));
            if (string.IsNullOrWhiteSpace(companyCode))
                throw new ArgumentException("Company code is required", nameof(companyCode));
            if (injuredPersonInfo == null)
                throw new ArgumentNullException(nameof(injuredPersonInfo));
            if (accidentCircumstances == null)
                throw new ArgumentNullException(nameof(accidentCircumstances));
            if (injuryDetails == null)
                throw new ArgumentNullException(nameof(injuryDetails));
            if (treatmentInfo == null)
                throw new ArgumentNullException(nameof(treatmentInfo));
            if (severityId <= 0)
                throw new ArgumentException("Severity ID must be greater than zero", nameof(severityId));
            if (statusId <= 0)
                throw new ArgumentException("Status ID must be greater than zero", nameof(statusId));
            if (string.IsNullOrWhiteSpace(enteredUserId))
                throw new ArgumentException("Entered user ID is required", nameof(enteredUserId));
            if (enteredUserNumber <= 0)
                throw new ArgumentException("Entered user number must be greater than zero", nameof(enteredUserNumber));

            AccidentNumber = accidentNumber;
            CompanyCode = companyCode;
            InjuredPersonInfo = injuredPersonInfo;
            AccidentCircumstances = accidentCircumstances;
            InjuryDetails = injuryDetails;
            TreatmentInfo = treatmentInfo;
            SeverityId = severityId;
            StatusId = statusId;
            EnteredUserId = enteredUserId;
            EnteredUserNumber = enteredUserNumber;
            EmployeeInfo = employeeInfo;
            ContractorInfo = contractorInfo;
            EnteredDate = DateTime.UtcNow;

            // Raise domain event
            AddDomainEvent(new AccidentReportCreatedEvent(this));
        }

        #endregion

        #region Business Methods

        /// <summary>
        /// Change the status of the accident report
        /// </summary>
        public void ChangeStatus(long newStatusId, string changedBy)
        {
            if (newStatusId <= 0)
                throw new ArgumentException("Status ID must be greater than zero", nameof(newStatusId));
            if (string.IsNullOrWhiteSpace(changedBy))
                throw new ArgumentException("Changed by is required", nameof(changedBy));

            if (StatusId != newStatusId)
            {
                var oldStatusId = StatusId;
                StatusId = newStatusId;
                UpdatedDate = DateTime.UtcNow;
                UpdatedBy = changedBy;

                AddDomainEvent(new AccidentStatusChangedEvent(this, oldStatusId, newStatusId));
            }
        }

        /// <summary>
        /// Change the severity of the accident report
        /// </summary>
        public void ChangeSeverity(long newSeverityId, string changedBy)
        {
            if (newSeverityId <= 0)
                throw new ArgumentException("Severity ID must be greater than zero", nameof(newSeverityId));
            if (string.IsNullOrWhiteSpace(changedBy))
                throw new ArgumentException("Changed by is required", nameof(changedBy));

            if (SeverityId != newSeverityId)
            {
                var oldSeverityId = SeverityId;
                SeverityId = newSeverityId;
                UpdatedDate = DateTime.UtcNow;
                UpdatedBy = changedBy;

                AddDomainEvent(new AccidentSeverityChangedEvent(this, oldSeverityId, newSeverityId));
            }
        }

        /// <summary>
        /// Update accident details
        /// </summary>
        public void UpdateAccidentDetails(
            AccidentCircumstances newCircumstances,
            InjuryDetails newInjuryDetails,
            TreatmentInfo newTreatmentInfo,
            string changedBy)
        {
            if (newCircumstances == null)
                throw new ArgumentNullException(nameof(newCircumstances));
            if (newInjuryDetails == null)
                throw new ArgumentNullException(nameof(newInjuryDetails));
            if (newTreatmentInfo == null)
                throw new ArgumentNullException(nameof(newTreatmentInfo));
            if (string.IsNullOrWhiteSpace(changedBy))
                throw new ArgumentException("Changed by is required", nameof(changedBy));

            AccidentCircumstances = newCircumstances;
            InjuryDetails = newInjuryDetails;
            TreatmentInfo = newTreatmentInfo;
            UpdatedDate = DateTime.UtcNow;
            UpdatedBy = changedBy;

            AddDomainEvent(new AccidentDetailsUpdatedEvent(this));
        }

        #endregion

        #region Domain Events

        /// <summary>
        /// Add a domain event to the aggregate
        /// </summary>
        public void AddDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        /// <summary>
        /// Clear all domain events after they have been published
        /// </summary>
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        #endregion

        public override string ToString()
        {
            return $"AccidentReport: {AccidentNumber} - {InjuredPersonInfo.PersonName} ({CompanyCode})";
        }
    }

    #region Domain Events

    /// <summary>
    /// Base class for domain events
    /// </summary>
    public abstract class DomainEvent
    {
        public DateTime OccurredAt { get; protected set; }
        public Guid EventId { get; protected set; }

        protected DomainEvent()
        {
            OccurredAt = DateTime.UtcNow;
            EventId = Guid.NewGuid();
        }
    }

    /// <summary>
    /// Event raised when an accident report is created
    /// </summary>
    public class AccidentReportCreatedEvent : DomainEvent
    {
        public AccidentReport AccidentReport { get; }

        public AccidentReportCreatedEvent(AccidentReport accidentReport)
        {
            AccidentReport = accidentReport ?? throw new ArgumentNullException(nameof(accidentReport));
        }
    }

    /// <summary>
    /// Event raised when accident status is changed
    /// </summary>
    public class AccidentStatusChangedEvent : DomainEvent
    {
        public AccidentReport AccidentReport { get; }
        public long OldStatusId { get; }
        public long NewStatusId { get; }

        public AccidentStatusChangedEvent(AccidentReport accidentReport, long oldStatusId, long newStatusId)
        {
            AccidentReport = accidentReport ?? throw new ArgumentNullException(nameof(accidentReport));
            OldStatusId = oldStatusId;
            NewStatusId = newStatusId;
        }
    }

    /// <summary>
    /// Event raised when accident severity is changed
    /// </summary>
    public class AccidentSeverityChangedEvent : DomainEvent
    {
        public AccidentReport AccidentReport { get; }
        public long OldSeverityId { get; }
        public long NewSeverityId { get; }

        public AccidentSeverityChangedEvent(AccidentReport accidentReport, long oldSeverityId, long newSeverityId)
        {
            AccidentReport = accidentReport ?? throw new ArgumentNullException(nameof(accidentReport));
            OldSeverityId = oldSeverityId;
            NewSeverityId = newSeverityId;
        }
    }

    /// <summary>
    /// Event raised when accident details are updated
    /// </summary>
    public class AccidentDetailsUpdatedEvent : DomainEvent
    {
        public AccidentReport AccidentReport { get; }

        public AccidentDetailsUpdatedEvent(AccidentReport accidentReport)
        {
            AccidentReport = accidentReport ?? throw new ArgumentNullException(nameof(accidentReport));
        }
    }

    #endregion
}
