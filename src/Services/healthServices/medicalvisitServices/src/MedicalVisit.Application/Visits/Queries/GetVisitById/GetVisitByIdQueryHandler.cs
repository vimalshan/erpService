using MediatR;
using MedicalVisit.Application.Common.Interfaces;
using MedicalVisit.Application.Common.Models;
using MedicalVisit.Application.DTOs;

namespace MedicalVisit.Application.Visits.Queries.GetVisitById;

public class GetVisitByIdQueryHandler : IRequestHandler<GetVisitByIdQuery, Result<VisitDto>>
{
    private readonly IVisitRepository _repository;

    public GetVisitByIdQueryHandler(IVisitRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<VisitDto>> Handle(GetVisitByIdQuery request, CancellationToken cancellationToken)
    {
        var visit = await _repository.GetByIdAsync(request.CompanyCode, request.VisitNumber, cancellationToken);

        if (visit == null)
        {
            return Result<VisitDto>.Failure("Visit not found");
        }

        var dto = new VisitDto
        {
            CompanyCode = visit.CompanyCode,
            VisitNumber = visit.VisitNumber,
            MedicalUserId = visit.MedicalUserId,
            MedicalPinNumber = visit.MedicalPinNumber,
            WorkerName = visit.WorkerName,
            ContractorId = visit.ContractorId,
            ContractorName = visit.ContractorName,
            VisitDate = visit.VisitDate,
            OtherHospital = visit.OtherHospital,
            Shift = visit.Shift.HasValue ? (char)visit.Shift.Value : null,
            Type = visit.Type.HasValue ? (char)visit.Type.Value : null,
            AttendantCode = visit.AttendantCode,
            DoctorCode = visit.DoctorCode,
            PatientDiagnosis = visit.Diagnosis.PatientDiagnosis,
            TreatmentRemarks = visit.Diagnosis.TreatmentRemarks,
            TestAdvice = visit.Diagnosis.TestAdvice,
            DoctorRemarks = visit.Diagnosis.DoctorRemarks,
            DiagnosisCategory = visit.Diagnosis.DiagnosisCategory,
            DiagnosisSubCategory = visit.Diagnosis.DiagnosisSubCategory,
            MedicineGiven = visit.MedicineGiven,
            NextReviewDate = visit.NextReviewDate,
            IsCancelled = visit.IsCancelled,
            CreatedBy = visit.CreatedInfo.UserId,
            CreatedAt = visit.CreatedInfo.Timestamp,
            ModifiedBy = visit.ModifiedInfo?.UserId,
            ModifiedAt = visit.ModifiedInfo?.Timestamp,
            SubRecords = visit.SubRecords.Select(sr => new VisitSubRecordDto
            {
                CompanyCode = sr.CompanyCode,
                VisitNumber = sr.VisitNumber,
                TestType = sr.TestType,
                TestValue = sr.TestValue,
                SerialNumber = sr.SerialNumber
            }).ToList()
        };

        return Result<VisitDto>.Success(dto);
    }
}
