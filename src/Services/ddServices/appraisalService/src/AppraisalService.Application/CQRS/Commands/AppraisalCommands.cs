using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using AppraisalService.Domain.Repositories;
using AppraisalService.Domain;
using AppraisalService.Domain.Entities;
using AppraisalService.Application.DTOs;

namespace AppraisalService.Application.CQRS.Commands;

// Create Appraisal Command
public class CreateAppraisalCommand : IRequest<long>
{
    public CreateOrUpdateAppraisalDto AppraisalData { get; set; }

    public CreateAppraisalCommand(CreateOrUpdateAppraisalDto appraisalData)
    {
        AppraisalData = appraisalData ?? throw new ArgumentNullException(nameof(appraisalData));
    }
}

public class CreateAppraisalCommandHandler : IRequestHandler<CreateAppraisalCommand, long>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateAppraisalCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<long> Handle(CreateAppraisalCommand request, CancellationToken cancellationToken)
    {
        var requestNumber = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        
        var appraisal = new AppraisalMainEntity(
            requestNumber,
            request.AppraisalData.UserCode,
            DateTime.UtcNow,
            request.AppraisalData.GradeId,
            request.AppraisalData.UnitId,
            request.AppraisalData.YearId);

        appraisal.SetEmployeeDetails(
            request.AppraisalData.Salute,
            request.AppraisalData.FirstName,
            request.AppraisalData.MiddleName,
            request.AppraisalData.LastName,
            request.AppraisalData.Designation,
            null,
            request.AppraisalData.PinNumber);

        if (request.AppraisalData.AppraisalStartDate.HasValue && request.AppraisalData.AppraisalEndDate.HasValue)
        {
            appraisal.SetAppraisalPeriod(
                request.AppraisalData.AppraisalStartDate.Value,
                request.AppraisalData.AppraisalEndDate.Value);
        }

        await _unitOfWork.Appraisals.AddAsync(appraisal, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return requestNumber;
    }
}

// Update Appraisal Command
// Update Appraisal Command
public class UpdateAppraisalCommand : IRequest<Unit>
{
    public long RequestNumber { get; set; }
    public CreateOrUpdateAppraisalDto AppraisalData { get; set; }

    public UpdateAppraisalCommand(long requestNumber, CreateOrUpdateAppraisalDto appraisalData)
    {
        RequestNumber = requestNumber;
        AppraisalData = appraisalData ?? throw new ArgumentNullException(nameof(appraisalData));
    }
}

public class UpdateAppraisalCommandHandler : IRequestHandler<UpdateAppraisalCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAppraisalCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateAppraisalCommand request, CancellationToken cancellationToken)
    {
        var appraisal = await _unitOfWork.Appraisals.GetByRequestNumberAsync(request.RequestNumber, cancellationToken);
        if (appraisal == null)
            throw new InvalidOperationException($"Appraisal with request number {request.RequestNumber} not found");

        appraisal.SetEmployeeDetails(
            request.AppraisalData.Salute,
            request.AppraisalData.FirstName,
            request.AppraisalData.MiddleName,
            request.AppraisalData.LastName,
            request.AppraisalData.Designation,
            null,
            request.AppraisalData.PinNumber);

        if (request.AppraisalData.AppraisalStartDate.HasValue && request.AppraisalData.AppraisalEndDate.HasValue)
        {
            appraisal.SetAppraisalPeriod(
                request.AppraisalData.AppraisalStartDate.Value,
                request.AppraisalData.AppraisalEndDate.Value);
        }

        await _unitOfWork.Appraisals.UpdateAsync(appraisal, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

// Submit Appraisal Command
public class SubmitAppraisalCommand : IRequest<Unit>
{
    public long RequestNumber { get; set; }
    public string? FinalVtcRating { get; set; }

    public SubmitAppraisalCommand(long requestNumber, string? finalVtcRating = null)
    {
        RequestNumber = requestNumber;
        FinalVtcRating = finalVtcRating;
    }
}

public class SubmitAppraisalCommandHandler : IRequestHandler<SubmitAppraisalCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public SubmitAppraisalCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(SubmitAppraisalCommand request, CancellationToken cancellationToken)
    {
        var appraisal = await _unitOfWork.Appraisals.GetByRequestNumberAsync(request.RequestNumber, cancellationToken);
        if (appraisal == null)
            throw new InvalidOperationException($"Appraisal with request number {request.RequestNumber} not found");

        appraisal.SubmitByAppraisee();

        if (!string.IsNullOrEmpty(request.FinalVtcRating))
        {
            appraisal.GetType().GetProperty(nameof(AppraisalMainEntity.FinalVtcRating))?.SetValue(appraisal, request.FinalVtcRating);
        }

        await _unitOfWork.Appraisals.UpdateAsync(appraisal, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

// Cancel Appraisal Command
public class CancelAppraisalCommand : IRequest<Unit>
{
    public long RequestNumber { get; set; }
    public string Remarks { get; set; }
    public long ApproverIdCancelledBy { get; set; }

    public CancelAppraisalCommand(long requestNumber, string remarks, long approverIdCancelledBy)
    {
        RequestNumber = requestNumber;
        Remarks = remarks ?? throw new ArgumentNullException(nameof(remarks));
        ApproverIdCancelledBy = approverIdCancelledBy;
    }
}

public class CancelAppraisalCommandHandler : IRequestHandler<CancelAppraisalCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public CancelAppraisalCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(CancelAppraisalCommand request, CancellationToken cancellationToken)
    {
        var appraisal = await _unitOfWork.Appraisals.GetByRequestNumberAsync(request.RequestNumber, cancellationToken);
        if (appraisal == null)
            throw new InvalidOperationException($"Appraisal with request number {request.RequestNumber} not found");

        appraisal.Cancel(request.Remarks, request.ApproverIdCancelledBy);
        
        await _unitOfWork.Appraisals.UpdateAsync(appraisal, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

// Add Competency Assessment Command
public class AddCompetencyAssessmentCommand : IRequest<long>
{
    public long RequestNumber { get; set; }
    public CompetencyAssessmentDto AssessmentData { get; set; }

    public AddCompetencyAssessmentCommand(long requestNumber, CompetencyAssessmentDto assessmentData)
    {
        RequestNumber = requestNumber;
        AssessmentData = assessmentData ?? throw new ArgumentNullException(nameof(assessmentData));
    }
}

public class AddCompetencyAssessmentCommandHandler : IRequestHandler<AddCompetencyAssessmentCommand, long>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddCompetencyAssessmentCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<long> Handle(AddCompetencyAssessmentCommand request, CancellationToken cancellationToken)
    {
        var appraisal = await _unitOfWork.Appraisals.GetByRequestNumberAsync(request.RequestNumber, cancellationToken);
        if (appraisal == null)
            throw new InvalidOperationException($"Appraisal with request number {request.RequestNumber} not found");

        var serialNumber = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var assessment = new CompetencyAssessmentEntity(
            request.RequestNumber,
            request.AssessmentData.CompetencyNumber,
            serialNumber,
            request.AssessmentData.AppraiserUserCode ?? string.Empty);

        assessment.SetAssessmentDetails(
            request.AssessmentData.AssessmentRating,
            request.AssessmentData.CompetencyRating,
            request.AssessmentData.Remarks,
            null,
            null,
            null);

        appraisal.AddCompetencyAssessment(assessment);
        await _unitOfWork.CompetencyAssessments.AddAsync(assessment, cancellationToken);
        await _unitOfWork.Appraisals.UpdateAsync(appraisal, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return serialNumber;
    }
}
