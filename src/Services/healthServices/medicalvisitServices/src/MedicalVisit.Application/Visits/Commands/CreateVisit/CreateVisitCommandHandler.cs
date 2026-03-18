using MediatR;
using MedicalVisit.Application.Common.Interfaces;
using MedicalVisit.Application.Common.Models;
using MedicalVisit.Application.DTOs;
using MedicalVisit.Domain.Entities;
using MedicalVisit.Domain.Enums;
using MedicalVisit.Domain.ValueObjects; // DiagnosisInfo, AuditInfo

namespace MedicalVisit.Application.Visits.Commands.CreateVisit;

public class CreateVisitCommandHandler : IRequestHandler<CreateVisitCommand, Result<VisitDto>>
{
    private readonly IVisitRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateVisitCommandHandler(IVisitRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<VisitDto>> Handle(CreateVisitCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var visitNumber = await _repository.GetNextVisitNumberAsync(request.CompanyCode, cancellationToken);

            var diagnosis = DiagnosisInfo.Create(
                request.PatientDiagnosis,
                request.TreatmentRemarks,
                request.TestAdvice,
                request.DoctorRemarks,
                request.DiagnosisCategory,
                request.DiagnosisSubCategory);

            var visit = VisitMainAggregate.Create(
                request.CompanyCode,
                visitNumber,
                request.MedicalUserId,
                request.DoctorCode,
                diagnosis,
                request.VisitDate,
                request.CreatedBy,
                request.MedicalPinNumber,
                request.WorkerName,
                request.ContractorId,
                request.ContractorName,
                request.OtherHospital,
                request.Shift.HasValue ? (VisitShift)request.Shift.Value : null,
                request.Type.HasValue ? (VisitType)request.Type.Value : null,
                request.AttendantCode,
                request.MedicineGiven,
                request.NextReviewDate,
                request.CreatedByPin);

            // Add sub records
            foreach (var subRecord in request.SubRecords)
            {
                visit.AddSubRecord(subRecord.TestType, subRecord.TestValue, subRecord.SerialNumber);
            }

            await _repository.AddAsync(visit, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var dto = MapToDto(visit);
            return Result<VisitDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Result<VisitDto>.Failure($"Error creating visit: {ex.Message}");
        }
    }

    private static VisitDto MapToDto(VisitMainAggregate visit)
    {
        return new VisitDto
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
    }
}
