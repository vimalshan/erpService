using MedicalVisit.Application.Common.Interfaces;
using MedicalVisit.Application.Common.Models;
using MedicalVisit.Application.DTOs;

namespace MedicalVisit.Application.Visits.Commands.CreateVisit;

public record CreateVisitCommand : ICommand<Result<VisitDto>>
{
    public string CompanyCode { get; init; } = string.Empty;
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
    public string CreatedBy { get; init; } = string.Empty;
    public decimal? CreatedByPin { get; init; }
    public List<SubRecordCommand> SubRecords { get; init; } = new();
}

public record SubRecordCommand
{
    public string? TestType { get; init; }
    public string? TestValue { get; init; }
    public long? SerialNumber { get; init; }
}
