using System;

namespace AppraisalService.Domain.Entities;

/// <summary>
/// Appraisal Band aggregate root
/// </summary>
public class AppraisalBandEntity : AggregateRoot
{
    public string? Description { get; set; }
    public string? Designation { get; set; }
    public string? SignatoryName { get; set; }
    public string? SignatoryDesignation { get; set; }
    public string? Code { get; set; }
    public char? FormFlag { get; set; }
    public long? GradeId { get; set; }

    private AppraisalBandEntity() { }

    public AppraisalBandEntity(
        long id,
        string? description,
        string? designation,
        string? signatoryName,
        string? signatoryDesignation,
        string? code,
        char? formFlag,
        long? gradeId) : base(id)
    {
        Description = description;
        Designation = designation;
        SignatoryName = signatoryName;
        SignatoryDesignation = signatoryDesignation;
        Code = code;
        FormFlag = formFlag;
        GradeId = gradeId;
    }

    public void Update(
        string? description,
        string? designation,
        string? signatoryName,
        string? signatoryDesignation,
        string? code,
        char? formFlag,
        long? gradeId)
    {
        Description = description;
        Designation = designation;
        SignatoryName = signatoryName;
        SignatoryDesignation = signatoryDesignation;
        Code = code;
        FormFlag = formFlag;
        GradeId = gradeId;
        ModifiedOn = DateTime.UtcNow;
    }
}
