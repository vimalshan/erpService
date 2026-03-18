using System;

namespace AppraisalService.Domain.Entities;

/// <summary>
/// Appraisal Details entity representing detailed compensation and promotion information
/// </summary>
public class AppraisalDetailsEntity : Entity
{
    public long RequestNumber { get; set; }
    public string? Designation { get; set; }
    public string? EmployeeType { get; set; }
    public decimal? IncrementAmount { get; set; }
    public decimal? BulletinPercentage { get; set; }
    public long? PromotionLevel { get; set; }
    public long? NewGrade { get; set; }
    public long? PromotionBand { get; set; }
    public long? EmployeeGradeId { get; set; }
    public long? EmployeeLevelId { get; set; }
    public long? EmployeeUnitId { get; set; }
    public long? YearId { get; set; }
    public long? IncrementTemplateId { get; set; }
    public long? RateTemplateId { get; set; }
    public string? LetterFile { get; set; }
    public decimal? ExperienceMonths { get; set; }

    // Navigation property
    public long AppraisalMainRequestNumber { get; set; }

    private AppraisalDetailsEntity() { }

    public AppraisalDetailsEntity(long requestNumber, string? designation, string? employeeType)
    {
        RequestNumber = requestNumber;
        Designation = designation;
        EmployeeType = employeeType;
        CreatedOn = DateTime.UtcNow;
        ModifiedOn = DateTime.UtcNow;
    }
}
