namespace MedicalVisit.Application.DTOs;

public record VisitDto
{
    public string CompanyCode { get; init; } = string.Empty;
    public long VisitNumber { get; init; }
    public string MedicalUserId { get; init; } = string.Empty;
    public decimal? MedicalPinNumber { get; init; }
    public string? WorkerName { get; init; }
    public string? ContractorId { get; init; }
    public string? ContractorName { get; init; }
    public DateTime VisitDate { get; init; }
    public string? OtherHospital { get; init; }
    public char? Shift { get; init; }
    public char? Type { get; init; }
    public string? AttendantCode { get; init; }
    public string DoctorCode { get; init; } = string.Empty;
    public string PatientDiagnosis { get; init; } = string.Empty;
    public string TreatmentRemarks { get; init; } = string.Empty;
    public string? TestAdvice { get; init; }
    public string? DoctorRemarks { get; init; }
    public string? DiagnosisCategory { get; init; }
    public long? DiagnosisSubCategory { get; init; }
    public string? MedicineGiven { get; init; }
    public DateTime? NextReviewDate { get; init; }
    public bool IsCancelled { get; init; }
    public List<VisitSubRecordDto> SubRecords { get; init; } = new();
    public string CreatedBy { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public string? ModifiedBy { get; init; }
    public DateTime? ModifiedAt { get; init; }
}

public record VisitSubRecordDto
{
    public string CompanyCode { get; init; } = string.Empty;
    public long VisitNumber { get; init; }
    public string? TestType { get; init; }
    public string? TestValue { get; init; }
    public long? SerialNumber { get; init; }
}
