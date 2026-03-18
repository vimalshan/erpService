using MediatR;
using MedicalVisit.Application.DTOs;
using MedicalVisit.Application.Visits.Commands.CreateVisit;

namespace MedicalVisit.API.GraphQL;

public class VisitMutation
{
    public async Task<VisitDto?> CreateVisitAsync(
        CreateVisitInput input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateVisitCommand
        {
            CompanyCode = input.CompanyCode,
            MedicalUserId = input.MedicalUserId,
            DoctorCode = input.DoctorCode,
            PatientDiagnosis = input.PatientDiagnosis,
            TreatmentRemarks = input.TreatmentRemarks,
            VisitDate = input.VisitDate,
            CreatedBy = input.CreatedBy,
            WorkerName = input.WorkerName,
            AttendantCode = input.AttendantCode,
            DoctorRemarks = input.DoctorRemarks,
            MedicineGiven = input.MedicineGiven
        };

        var result = await mediator.Send(command, cancellationToken);

        return result.IsSuccess ? result.Data : null;
    }
}

public record CreateVisitInput
{
    public string CompanyCode { get; init; } = string.Empty;
    public string MedicalUserId { get; init; } = string.Empty;
    public string DoctorCode { get; init; } = string.Empty;
    public string PatientDiagnosis { get; init; } = string.Empty;
    public string TreatmentRemarks { get; init; } = string.Empty;
    public DateTime VisitDate { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
    public string? WorkerName { get; init; }
    public string? AttendantCode { get; init; }
    public string? DoctorRemarks { get; init; }
    public string? MedicineGiven { get; init; }
}
