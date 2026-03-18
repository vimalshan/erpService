using System;

namespace AppraisalService.Domain.Entities;

/// <summary>
/// Competency Assessment entity
/// </summary>
public class CompetencyAssessmentEntity : Entity
{
    public long RequestNumber { get; set; }
    public long CompetencyNumber { get; set; }
    public long SerialNumber { get; set; }
    public decimal? AssessmentRating { get; set; }
    public decimal? CompetencyRating { get; set; }
    public string? Remarks { get; set; }
    public string? SelfDevelopment { get; set; }
    public string? JobDevelopment { get; set; }
    public string? TrainingDevelopment { get; set; }
    public string? AppraiserUserCode { get; set; }
    public long? AppraiserUserNumber { get; set; }
    public long? PinNumber { get; set; }
    public string? Role { get; set; }
    public DateTime? CancellationDate { get; set; }
    public string? CancellationRemarks { get; set; }

    // Navigation property
    public long AppraisalMainRequestNumber { get; set; }
    public AppraisalMainEntity? AppraisalMain { get; set; }

    private CompetencyAssessmentEntity() { }

    public CompetencyAssessmentEntity(
        long requestNumber,
        long competencyNumber,
        long serialNumber,
        string appraiserUserCode)
    {
        RequestNumber = requestNumber;
        CompetencyNumber = competencyNumber;
        SerialNumber = serialNumber;
        AppraiserUserCode = appraiserUserCode ?? throw new ArgumentNullException(nameof(appraiserUserCode));
        CreatedOn = DateTime.UtcNow;
        ModifiedOn = DateTime.UtcNow;
    }

    public void SetAssessmentDetails(
        decimal? assessmentRating,
        decimal? competencyRating,
        string? remarks,
        string? selfDevelopment,
        string? jobDevelopment,
        string? trainingDevelopment)
    {
        AssessmentRating = assessmentRating;
        CompetencyRating = competencyRating;
        Remarks = remarks;
        SelfDevelopment = selfDevelopment;
        JobDevelopment = jobDevelopment;
        TrainingDevelopment = trainingDevelopment;
        ModifiedOn = DateTime.UtcNow;
    }

    public void Cancel(string remarks)
    {
        CancellationDate = DateTime.UtcNow;
        CancellationRemarks = remarks;
        ModifiedOn = DateTime.UtcNow;
    }
}
