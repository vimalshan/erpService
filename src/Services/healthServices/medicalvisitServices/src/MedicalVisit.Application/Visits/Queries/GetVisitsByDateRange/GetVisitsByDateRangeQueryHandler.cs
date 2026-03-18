using MediatR;
using MedicalVisit.Application.Common.Interfaces;
using MedicalVisit.Application.Common.Models;
using MedicalVisit.Application.DTOs;

namespace MedicalVisit.Application.Visits.Queries.GetVisitsByDateRange;

public class GetVisitsByDateRangeQueryHandler : IRequestHandler<GetVisitsByDateRangeQuery, Result<List<VisitDto>>>
{
    private readonly IVisitRepository _repository;

    public GetVisitsByDateRangeQueryHandler(IVisitRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<VisitDto>>> Handle(GetVisitsByDateRangeQuery request, CancellationToken cancellationToken)
    {
        var visits = await _repository.GetByDateRangeAsync(
            request.CompanyCode, 
            request.StartDate, 
            request.EndDate, 
            cancellationToken);

        var dtos = visits.Select(visit => new VisitDto
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
        }).ToList();

        return Result<List<VisitDto>>.Success(dtos);
    }
}
