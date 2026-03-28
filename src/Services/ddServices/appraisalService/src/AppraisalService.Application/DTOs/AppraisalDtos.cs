using System;
using System.Collections.Generic;

namespace AppraisalService.Application.DTOs;

/// <summary>
/// DTO for Appraisal Band
/// </summary>
public class AppraisalBandDto
{
    public long BandId { get; set; }
    public string? Description { get; set; }
    public string? Designation { get; set; }
    public string? SignatoryName { get; set; }
    public string? SignatoryDesignation { get; set; }
    public string? Code { get; set; }
    public string? FormFlag { get; set; }
    public long? GradeId { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
}

/// <summary>
/// DTO for Compensation Details
/// </summary>
public class CompensationDto
{
    public decimal? BasicOld { get; set; }
    public decimal? BasicNew { get; set; }
    public decimal? CtcOld { get; set; }
    public decimal? CtcNew { get; set; }
    public decimal? IncrementAmount { get; set; }
    public DateTime? EffectiveFrom { get; set; }
}

/// <summary>
/// DTO for Benefits
/// </summary>
public class BenefitsDto
{
    public bool IsGratuityAvailable { get; set; }
    public bool IsSuperannuationAvailable { get; set; }
    public bool IsPfAvailable { get; set; }
    public decimal? NewFlexipay { get; set; }
}

/// <summary>
/// DTO for Appraisal Main (Summary)
/// </summary>
public class AppraisalMainDto
{
    public long RequestNumber { get; set; }
    public string UserCode { get; set; } = string.Empty;
    public long? UserNumber { get; set; }
    public long? PinNumber { get; set; }
    public DateTime EntryDate { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Designation { get; set; }
    public string? Status { get; set; }
    public DateTime? AppraisalStartDate { get; set; }
    public DateTime? AppraisalEndDate { get; set; }
    public DateTime? CompletedOn { get; set; }
    public string? AppraisalType { get; set; }
    public string? EmployeeType { get; set; }
    public decimal? IncrementAmount { get; set; }
    public long? YearId { get; set; }
    public long? GradeId { get; set; }
    public long? UnitId { get; set; }
}

/// <summary>
/// DTO for detailed Appraisal with all information
/// </summary>
public class AppraisalDetailedDto
{
    public long RequestNumber { get; set; }
    public string UserCode { get; set; } = string.Empty;
    public long? UserNumber { get; set; }
    public long? PinNumber { get; set; }
    public DateTime EntryDate { get; set; }
    public string? Salute { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? Designation { get; set; }
    public string? Status { get; set; }
    public DateTime? AppraisalStartDate { get; set; }
    public DateTime? AppraisalEndDate { get; set; }
    public DateTime? CompletedOn { get; set; }
    public string? AppraisalType { get; set; }
    public string? EmployeeType { get; set; }
    public long? PromotionBand { get; set; }
    public string? FinalVtcRating { get; set; }
    public string? PayrollStatus { get; set; }

    public CompensationDto? Compensation { get; set; }
    public BenefitsDto? Benefits { get; set; }
    public List<CompetencyAssessmentDto> CompetencyAssessments { get; set; } = new();
}

/// <summary>
/// DTO for Competency Assessment
/// </summary>
public class CompetencyAssessmentDto
{
    public long SerialNumber { get; set; }
    public long RequestNumber { get; set; }
    public long CompetencyNumber { get; set; }
    public decimal? AssessmentRating { get; set; }
    public decimal? CompetencyRating { get; set; }
    public string? Remarks { get; set; }
    public string? AppraiserUserCode { get; set; }
    public string? Role { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
}

/// <summary>
/// DTO for Employee Goal
/// </summary>
public class EmployeeGoalDto
{
    public long RequestNumber { get; set; }
    public long SerialNumber { get; set; }
    public long? PinNumber { get; set; }
    public string? UserId { get; set; }
    public string? PersonDesignation { get; set; }
    public decimal? Weightage { get; set; }
    public DateTime? FinancialStartDate { get; set; }
    public DateTime? FinancialEndDate { get; set; }
    public string? Category { get; set; }
    public string? Status { get; set; }
    public string? Achievements { get; set; }
    public string? Difficulties { get; set; }
}

/// <summary>
/// DTO for creating/updating appraisal
/// </summary>
public class CreateOrUpdateAppraisalDto
{
    public string UserCode { get; set; } = string.Empty;
    public long? PinNumber { get; set; }
    public long? UserNumber { get; set; }
    public string? Salute { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? Designation { get; set; }
    public long? GradeId { get; set; }
    public long? UnitId { get; set; }
    public long? YearId { get; set; }
    public string? AppraisalType { get; set; }
    public DateTime? AppraisalStartDate { get; set; }
    public DateTime? AppraisalEndDate { get; set; }
    public string? SignatoryName { get; set; }
    public string? SignatoryDesignation { get; set; }
}

/// <summary>
/// DTO for submitting appraisal
/// </summary>
public class SubmitAppraisalDto
{
    public long RequestNumber { get; set; }
    public string? FinalVtcRating { get; set; }
    public long? PromotionBand { get; set; }
    public long? NewGrade { get; set; }
}
