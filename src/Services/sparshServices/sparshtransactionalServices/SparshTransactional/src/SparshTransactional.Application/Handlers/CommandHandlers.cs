using AutoMapper;
using MediatR;
using SparshTransactional.Application.Commands;
using SparshTransactional.Application.DTOs;
using SparshTransactional.Domain.Entities;
using SparshTransactional.Domain.Interfaces;

namespace SparshTransactional.Application.Handlers;

public class CreateScholarshipHandler(
    IScholarshipMasterRepository repo,
    IUnitOfWork uow,
    IMapper mapper) : IRequestHandler<CreateScholarshipCommand, ScholarshipMasterDto>
{
    public async Task<ScholarshipMasterDto> Handle(CreateScholarshipCommand request, CancellationToken ct)
    {
        var scholarship = ScholarshipMaster.Create(
            request.Name, request.Description, request.Type,
            request.CoveragePercent, request.MaxAmount, request.CreatedBy);

        await repo.AddAsync(scholarship, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<ScholarshipMasterDto>(scholarship);
    }
}

public class UpdateScholarshipHandler(
    IScholarshipMasterRepository repo,
    IUnitOfWork uow,
    IMapper mapper) : IRequestHandler<UpdateScholarshipCommand, ScholarshipMasterDto>
{
    public async Task<ScholarshipMasterDto> Handle(UpdateScholarshipCommand request, CancellationToken ct)
    {
        var scholarship = await repo.GetByIdAsync(request.ScholarshipId, ct)
            ?? throw new KeyNotFoundException($"Scholarship {request.ScholarshipId} not found.");

        scholarship.ScholarshipName = request.Name;
        scholarship.ScholarshipDescription = request.Description;
        scholarship.ScholarshipType = request.Type;
        scholarship.CoveragePercent = request.CoveragePercent;
        scholarship.MaxAmount = request.MaxAmount;
        scholarship.UpdatedBy = request.UpdatedBy;
        scholarship.UpdatedOn = DateTime.UtcNow;

        await repo.UpdateAsync(scholarship, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<ScholarshipMasterDto>(scholarship);
    }
}

public class DeactivateScholarshipHandler(
    IScholarshipMasterRepository repo,
    IUnitOfWork uow) : IRequestHandler<DeactivateScholarshipCommand, bool>
{
    public async Task<bool> Handle(DeactivateScholarshipCommand request, CancellationToken ct)
    {
        var scholarship = await repo.GetByIdAsync(request.ScholarshipId, ct)
            ?? throw new KeyNotFoundException($"Scholarship {request.ScholarshipId} not found.");

        scholarship.Deactivate(request.UpdatedBy);
        await repo.UpdateAsync(scholarship, ct);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}

public class AddEligibilityCriteriaHandler(
    IScholarshipMasterRepository scholarshipRepo,
    IEligibilityCriteriaRepository criteriaRepo,
    IUnitOfWork uow,
    IMapper mapper) : IRequestHandler<AddEligibilityCriteriaCommand, EligibilityCriteriaDto>
{
    public async Task<EligibilityCriteriaDto> Handle(AddEligibilityCriteriaCommand request, CancellationToken ct)
    {
        _ = await scholarshipRepo.GetByIdAsync(request.ScholarshipId, ct)
            ?? throw new KeyNotFoundException($"Scholarship {request.ScholarshipId} not found.");

        var criteria = new EligibilityCriteria
        {
            ScholarshipId = request.ScholarshipId,
            CriteriaName = request.CriteriaName,
            CriteriaDescription = request.CriteriaDescription,
            MinScore = request.MinScore,
            MaxFamilyIncome = request.MaxFamilyIncome,
            EligibilityStatus = "A",
            CreatedBy = request.CreatedBy,
            CreatedOn = DateTime.UtcNow
        };

        await criteriaRepo.AddAsync(criteria, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<EligibilityCriteriaDto>(criteria);
    }
}

public class SubmitApplicationHandler(
    IScholarshipMasterRepository scholarshipRepo,
    IScholarshipApplicationRepository appRepo,
    IUnitOfWork uow,
    IMapper mapper) : IRequestHandler<SubmitApplicationCommand, ScholarshipApplicationDto>
{
    public async Task<ScholarshipApplicationDto> Handle(SubmitApplicationCommand request, CancellationToken ct)
    {
        var scholarship = await scholarshipRepo.GetByIdAsync(request.ScholarshipId, ct)
            ?? throw new KeyNotFoundException($"Scholarship {request.ScholarshipId} not found.");

        if (scholarship.Status != "A")
            throw new InvalidOperationException("Scholarship is not active.");

        var application = ScholarshipApplication.Submit(
            request.StudentId, request.ScholarshipId, request.FamilyIncome, request.CreatedBy);

        await appRepo.AddAsync(application, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<ScholarshipApplicationDto>(application);
    }
}

public class ApproveApplicationHandler(
    IScholarshipApplicationRepository appRepo,
    IScholarshipDisbursementRepository disbRepo,
    IUnitOfWork uow,
    IMapper mapper) : IRequestHandler<ApproveApplicationCommand, ScholarshipApplicationDto>
{
    public async Task<ScholarshipApplicationDto> Handle(ApproveApplicationCommand request, CancellationToken ct)
    {
        var application = await appRepo.GetByIdAsync(request.ApplicationId, ct)
            ?? throw new KeyNotFoundException($"Application {request.ApplicationId} not found.");

        if (application.ApplicationStatus != "S")
            throw new InvalidOperationException("Only submitted applications can be approved.");

        application.Approve(request.ApprovedBy, request.ApprovedAmount);
        await appRepo.UpdateAsync(application, ct);

        // Auto-create pending disbursement
        var disbursement = ScholarshipDisbursement.Create(
            application.ApplicationId, application.StudentId,
            application.ScholarshipId, request.ApprovedAmount, request.ApprovedBy);

        await disbRepo.AddAsync(disbursement, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<ScholarshipApplicationDto>(application);
    }
}

public class RejectApplicationHandler(
    IScholarshipApplicationRepository appRepo,
    IUnitOfWork uow,
    IMapper mapper) : IRequestHandler<RejectApplicationCommand, ScholarshipApplicationDto>
{
    public async Task<ScholarshipApplicationDto> Handle(RejectApplicationCommand request, CancellationToken ct)
    {
        var application = await appRepo.GetByIdAsync(request.ApplicationId, ct)
            ?? throw new KeyNotFoundException($"Application {request.ApplicationId} not found.");

        if (application.ApplicationStatus != "S")
            throw new InvalidOperationException("Only submitted applications can be rejected.");

        application.Reject(request.RejectedBy, request.Reason);
        await appRepo.UpdateAsync(application, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<ScholarshipApplicationDto>(application);
    }
}

public class CreateDisbursementHandler(
    IScholarshipApplicationRepository appRepo,
    IScholarshipDisbursementRepository disbRepo,
    IUnitOfWork uow,
    IMapper mapper) : IRequestHandler<CreateDisbursementCommand, ScholarshipDisbursementDto>
{
    public async Task<ScholarshipDisbursementDto> Handle(CreateDisbursementCommand request, CancellationToken ct)
    {
        var application = await appRepo.GetByIdAsync(request.ApplicationId, ct)
            ?? throw new KeyNotFoundException($"Application {request.ApplicationId} not found.");

        if (application.ApplicationStatus != "A")
            throw new InvalidOperationException("Disbursement requires an approved application.");

        var disbursement = ScholarshipDisbursement.Create(
            application.ApplicationId, application.StudentId,
            application.ScholarshipId, request.Amount, request.CreatedBy);

        await disbRepo.AddAsync(disbursement, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<ScholarshipDisbursementDto>(disbursement);
    }
}

public class CompleteDisbursementHandler(
    IScholarshipDisbursementRepository disbRepo,
    IUnitOfWork uow,
    IMapper mapper) : IRequestHandler<CompleteDisbursementCommand, ScholarshipDisbursementDto>
{
    public async Task<ScholarshipDisbursementDto> Handle(CompleteDisbursementCommand request, CancellationToken ct)
    {
        var disbursement = await disbRepo.GetByIdAsync(request.DisbursementId, ct)
            ?? throw new KeyNotFoundException($"Disbursement {request.DisbursementId} not found.");

        if (disbursement.DisbursementStatus != "P")
            throw new InvalidOperationException("Only pending disbursements can be completed.");

        disbursement.Complete(request.PaymentReference);
        await disbRepo.UpdateAsync(disbursement, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<ScholarshipDisbursementDto>(disbursement);
    }
}
