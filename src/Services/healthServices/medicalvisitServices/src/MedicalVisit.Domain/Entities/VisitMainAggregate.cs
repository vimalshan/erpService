using MedicalVisit.Domain.Common;
using MedicalVisit.Domain.Enums;
using MedicalVisit.Domain.ValueObjects;
using MedicalVisit.Domain.Events;

namespace MedicalVisit.Domain.Entities;

public class VisitMainAggregate : BaseEntity
{
    public string CompanyCode { get; private set; } = null!;
    public long VisitNumber { get; private set; }
    public string MedicalUserId { get; private set; }
    public decimal? MedicalPinNumber { get; private set; }
    public string? WorkerName { get; private set; }
    public string? ContractorId { get; private set; }
    public string? ContractorName { get; private set; }
    public DateTime VisitDate { get; private set; }
    public string? OtherHospital { get; private set; }
    public VisitShift? Shift { get; private set; }
    public VisitType? Type { get; private set; }
    public string? AttendantCode { get; private set; }
    public string DoctorCode { get; private set; }
    public DiagnosisInfo Diagnosis { get; private set; }
    public string? MedicineGiven { get; private set; }
    public DateTime? NextReviewDate { get; private set; }
    public AuditInfo CreatedInfo { get; private set; }
    public AuditInfo? ModifiedInfo { get; private set; }
    public bool IsCancelled { get; private set; }

    private readonly List<VisitSubRecord> _subRecords = new();
    public IReadOnlyCollection<VisitSubRecord> SubRecords => _subRecords.AsReadOnly();

    private VisitMainAggregate() { }

    public static VisitMainAggregate Create(
        string companyCode,
        long visitNumber,
        string medicalUserId,
        string doctorCode,
        DiagnosisInfo diagnosis,
        DateTime visitDate,
        string createdByUserId,
        decimal? medicalPinNumber = null,
        string? workerName = null,
        string? contractorId = null,
        string? contractorName = null,
        string? otherHospital = null,
        VisitShift? shift = null,
        VisitType? type = null,
        string? attendantCode = null,
        string? medicineGiven = null,
        DateTime? nextReviewDate = null,
        decimal? createdByPin = null)
    {
        if (string.IsNullOrWhiteSpace(companyCode) || companyCode.Length > 3)
            throw new ArgumentException("Company code must be 1-3 characters", nameof(companyCode));

        if (string.IsNullOrWhiteSpace(medicalUserId))
            throw new ArgumentException("Medical user ID is required", nameof(medicalUserId));

        if (string.IsNullOrWhiteSpace(doctorCode))
            throw new ArgumentException("Doctor code is required", nameof(doctorCode));

        var visit = new VisitMainAggregate
        {
            CompanyCode = companyCode.ToUpperInvariant(),

            VisitNumber = visitNumber,
            MedicalUserId = medicalUserId,
            MedicalPinNumber = medicalPinNumber,
            WorkerName = workerName,
            ContractorId = contractorId,
            ContractorName = contractorName,
            VisitDate = visitDate,
            OtherHospital = otherHospital,
            Shift = shift,
            Type = type,
            AttendantCode = attendantCode,
            DoctorCode = doctorCode,
            Diagnosis = diagnosis,
            MedicineGiven = medicineGiven,
            NextReviewDate = nextReviewDate,
            CreatedInfo = AuditInfo.Create(createdByUserId, createdByPin),
            IsCancelled = false
        };

        visit.AddDomainEvent(new VisitCreatedEvent(visit));

        return visit;
    }

    public void AddSubRecord(string testType, string testValue, long? serialNumber = null)
    {
        var subRecord = VisitSubRecord.Create(
            CompanyCode,   // string
            VisitNumber,
            testType,
            testValue,
            serialNumber);

        _subRecords.Add(subRecord);

        AddDomainEvent(new VisitSubRecordAddedEvent(this, subRecord));
    }

    public void UpdateDiagnosis(DiagnosisInfo newDiagnosis, string modifiedByUserId, decimal? modifiedByPin = null)
    {
        Diagnosis = newDiagnosis;
        ModifiedInfo = AuditInfo.Create(modifiedByUserId, modifiedByPin);

        AddDomainEvent(new VisitUpdatedEvent(this));
    }

    public void UpdateNextReviewDate(DateTime? nextReviewDate, string modifiedByUserId, decimal? modifiedByPin = null)
    {
        NextReviewDate = nextReviewDate;
        ModifiedInfo = AuditInfo.Create(modifiedByUserId, modifiedByPin);

        AddDomainEvent(new VisitUpdatedEvent(this));
    }

    public void Cancel(string cancelledByUserId, decimal? cancelledByPin = null)
    {
        if (IsCancelled)
            throw new InvalidOperationException("Visit is already cancelled");

        IsCancelled = true;
        ModifiedInfo = AuditInfo.Create(cancelledByUserId, cancelledByPin);

        AddDomainEvent(new VisitCancelledEvent(this));
    }

    public void UpdateMedicineGiven(string medicineGiven, string modifiedByUserId, decimal? modifiedByPin = null)
    {
        MedicineGiven = medicineGiven;
        ModifiedInfo = AuditInfo.Create(modifiedByUserId, modifiedByPin);

        AddDomainEvent(new VisitUpdatedEvent(this));
    }
}
