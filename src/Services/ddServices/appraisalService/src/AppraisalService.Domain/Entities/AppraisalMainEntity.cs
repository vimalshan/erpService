using System;
using System.Collections.Generic;
using AppraisalService.Domain.Events;

namespace AppraisalService.Domain.Entities;

/// <summary>
/// Appraisal Main aggregate root - represents an employee's appraisal
/// </summary>
public class AppraisalMainEntity : AggregateRoot
{
    public long RequestNumber { get; private set; }
    public string UserCode { get; private set; }
    public long? UserNumber { get; private set; }
    public long? PinNumber { get; private set; }
    public DateTime EntryDate { get; private set; }
    public long? GradeId { get; private set; }
    public long? UnitId { get; private set; }
    public long? YearId { get; private set; }
    public string? CancellationRemarks { get; private set; }
    public AppraisalStatus Status { get; private set; }
    public DateTime? AppraisalStartDate { get; private set; }
    public DateTime? AppraisalEndDate { get; private set; }
    public DateTime? CompletedOn { get; private set; }
    public string? AppraisalType { get; private set; }
    public long? CancelledByApproverId { get; private set; }
    public DateTime? CancelledDate { get; private set; }
    public char? HasSubordinates { get; private set; }

    // Compensation details
    public CompensationDetails? Compensation { get; private set; }

    // Benefits
    public BenefitsAvailability? Benefits { get; private set; }

    // Employee details
    public string? Salute { get; private set; }
    public string? FirstName { get; private set; }
    public string? MiddleName { get; private set; }
    public string? LastName { get; private set; }
    public string? Designation { get; private set; }
    public string? SignatoryName { get; private set; }
    public string? SignatoryDesignation { get; private set; }

    // Ratings
    public string? FinalVtcRating { get; private set; }
    public long? PromotionBand { get; private set; }
    public string? EmployeeType { get; private set; }

    // Approval status: N- Processing, A- Approved HR, C- Approved by All CEO, S- Digitally Signed, R- Released Letter, E- Release to Employee, Y- Pushed to Payroll
    public char? PayrollStatus { get; private set; }

    private List<AppraisalDetailsEntity> _appraisalDetails = new();
    public IReadOnlyCollection<AppraisalDetailsEntity> AppraisalDetails => _appraisalDetails.AsReadOnly();

    private List<CompetencyAssessmentEntity> _competencyAssessments = new();
    public IReadOnlyCollection<CompetencyAssessmentEntity> CompetencyAssessments => _competencyAssessments.AsReadOnly();

    private AppraisalMainEntity() { }

    public AppraisalMainEntity(
        long requestNumber,
        string userCode,
        DateTime entryDate,
        long? gradeId = null,
        long? unitId = null,
        long? yearId = null) : base(requestNumber)
    {
        RequestNumber = requestNumber;
        UserCode = userCode ?? throw new ArgumentNullException(nameof(userCode));
        EntryDate = entryDate;
        GradeId = gradeId;
        UnitId = unitId;
        YearId = yearId;
        Status = AppraisalStatus.Incomplete;
        CreatedOn = DateTime.UtcNow;
        ModifiedOn = DateTime.UtcNow;

        RaiseDomainEvent(new AppraisalInitiatedDomainEvent(requestNumber, userCode));
    }

    public void SetEmployeeDetails(
        string? salute,
        string? firstName,
        string? middleName,
        string? lastName,
        string? designation,
        long? userNumber,
        long? pinNumber)
    {
        Salute = salute;
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        Designation = designation;
        UserNumber = userNumber;
        PinNumber = pinNumber;
        ModifiedOn = DateTime.UtcNow;
    }

    public void SetCompensationDetails(CompensationDetails compensation)
    {
        Compensation = compensation;
        ModifiedOn = DateTime.UtcNow;
    }

    public void SetBenefits(BenefitsAvailability benefits)
    {
        Benefits = benefits;
        ModifiedOn = DateTime.UtcNow;
    }

    public void SetAppraisalPeriod(DateTime startDate, DateTime endDate)
    {
        AppraisalStartDate = startDate;
        AppraisalEndDate = endDate;
        ModifiedOn = DateTime.UtcNow;
    }

    public void SubmitByAppraisee()
    {
        Status = AppraisalStatus.SubmittedByAppraisee;
        ModifiedOn = DateTime.UtcNow;
        RaiseDomainEvent(new AppraisalSubmittedByAppraiseeEvent(RequestNumber, UserCode));
    }

    public void AssignToAppraiser()
    {
        Status = AppraisalStatus.PendingWithAppraiser;
        ModifiedOn = DateTime.UtcNow;
        RaiseDomainEvent(new AppraisalAssignedToAppraiserEvent(RequestNumber, UserCode));
    }

    public void SubmitByAppraiser()
    {
        Status = AppraisalStatus.SubmittedByAppraiser;
        ModifiedOn = DateTime.UtcNow;
        RaiseDomainEvent(new AppraisalSubmittedByAppraiserEvent(RequestNumber, UserCode));
    }

    public void Approve()
    {
        Status = AppraisalStatus.CompletedByAppraisee;
        CompletedOn = DateTime.UtcNow;
        ModifiedOn = DateTime.UtcNow;
        RaiseDomainEvent(new AppraisalApprovedEvent(RequestNumber, UserCode));
    }

    public void Cancel(string remarks, long approverIdCancelledBy)
    {
        CancellationRemarks = remarks;
        CancelledByApproverId = approverIdCancelledBy;
        CancelledDate = DateTime.UtcNow;
        ModifiedOn = DateTime.UtcNow;
        RaiseDomainEvent(new AppraisalCancelledEvent(RequestNumber, UserCode, remarks));
    }

    public void SetPayrollStatus(char status)
    {
        PayrollStatus = status;
        ModifiedOn = DateTime.UtcNow;
    }

    public void AddCompetencyAssessment(CompetencyAssessmentEntity assessment)
    {
        _competencyAssessments.Add(assessment);
        ModifiedOn = DateTime.UtcNow;
    }
}
