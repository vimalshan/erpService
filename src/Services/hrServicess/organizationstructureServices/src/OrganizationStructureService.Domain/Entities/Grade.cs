using OrganizationStructureService.Domain.Common;
using OrganizationStructureService.Domain.Events;
using OrganizationStructureService.Domain.ValueObjects;

namespace OrganizationStructureService.Domain.Entities;

public class Grade : AggregateRoot
{
    public decimal GradeId { get; private set; }
    public string? GradeCode { get; private set; }
    public string? GradeName { get; private set; }
    public string? GradeDesignation { get; private set; }
    public string? GradeCategoryCode { get; private set; }
    public LiveFlag? LiveFlag { get; private set; }
    public string? ManagementCategoryCode { get; private set; }
    public decimal? Priority { get; private set; }
    public string? SubCategory { get; private set; }
    public string? DefaultRating { get; private set; }
    public decimal? PromotionScore { get; private set; }
    public decimal? LevelCount { get; private set; }
    public decimal? CadreId { get; private set; }

    private Grade() { }

    public static Grade Create(
        decimal gradeId,
        string gradeName,
        string? gradeCode,
        string? gradeDesignation,
        string? categoryCode,
        string? managementCategoryCode,
        decimal? priority)
    {
        var grade = new Grade
        {
            GradeId = gradeId,
            GradeName = gradeName,
            GradeCode = gradeCode,
            GradeDesignation = gradeDesignation,
            GradeCategoryCode = categoryCode,
            ManagementCategoryCode = managementCategoryCode,
            Priority = priority,
            LiveFlag = ValueObjects.LiveFlag.Active
        };
        grade.RaiseDomainEvent(new GradeCreatedEvent(gradeId, gradeName));
        grade.IncrementVersion();
        return grade;
    }

    public void Update(string gradeName, string? gradeDesignation, decimal? priority)
    {
        GradeName = gradeName;
        GradeDesignation = gradeDesignation;
        Priority = priority;
        IncrementVersion();
    }
}
