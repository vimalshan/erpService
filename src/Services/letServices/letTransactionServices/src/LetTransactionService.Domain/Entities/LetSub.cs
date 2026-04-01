namespace LetTransactionService.Domain.Entities;

/// <summary>Maps to LET_SUB table — LET request sub-entries with development details.</summary>
public class LetSub
{
    public long RequestNumber { get; private set; }
    public int SerialNumber { get; private set; }
    public DateTime? ModifiedDate { get; private set; }
    public string? ModifiedUser { get; private set; }
    public char? PreferredModeDev { get; private set; }
    public string? ActionTaken { get; private set; }
    public int? CourseId { get; private set; }
    public string? TrainingProgramBhr { get; private set; }
    public string? ImpactBenefitProcess { get; private set; }
    public string? MeasureCompetency { get; private set; }
    public string? MidYearReviewerName { get; private set; }
    public string? MidYearReviewerDate { get; private set; }
    public string? MidYearReviewerRemark { get; private set; }
    public string? AnnualReviewerName { get; private set; }
    public string? AnnualReviewerDate { get; private set; }
    public string? AnnualReviewerRemark { get; private set; }
    public int? CompetencyToDevelop { get; private set; }
    public string? DomainKnowledgeDev { get; private set; }
    public string? DomainKnowledgeDevDetail { get; private set; }
    public string? ProcessDev { get; private set; }
    public string? ProcessDevDetail { get; private set; }
    public char? LetSubCode { get; private set; }
    public string? ReviewType { get; private set; }

    // Navigation
    public LetMain LetMain { get; private set; } = null!;

    private LetSub() { }

    internal static LetSub Create(
        long requestNumber,
        int serialNumber,
        char? preferredModeDev,
        string? actionTaken,
        int? courseId,
        string? trainingProgramBhr,
        string? impactBenefitProcess,
        string? measureCompetency,
        int? competencyToDevelop,
        string? domainKnowledgeDev,
        string? domainKnowledgeDevDetail,
        string? processDev,
        string? processDevDetail,
        char? letSubCode,
        string? reviewType)
    {
        return new LetSub
        {
            RequestNumber = requestNumber,
            SerialNumber = serialNumber,
            ModifiedDate = DateTime.UtcNow,
            PreferredModeDev = preferredModeDev,
            ActionTaken = actionTaken,
            CourseId = courseId,
            TrainingProgramBhr = trainingProgramBhr,
            ImpactBenefitProcess = impactBenefitProcess,
            MeasureCompetency = measureCompetency,
            CompetencyToDevelop = competencyToDevelop,
            DomainKnowledgeDev = domainKnowledgeDev,
            DomainKnowledgeDevDetail = domainKnowledgeDevDetail,
            ProcessDev = processDev,
            ProcessDevDetail = processDevDetail,
            LetSubCode = letSubCode,
            ReviewType = reviewType
        };
    }

    internal void UpdateReviews(
        string? midYearReviewerName,
        string? midYearReviewerDate,
        string? midYearReviewerRemark,
        string? annualReviewerName,
        string? annualReviewerDate,
        string? annualReviewerRemark)
    {
        MidYearReviewerName = midYearReviewerName;
        MidYearReviewerDate = midYearReviewerDate;
        MidYearReviewerRemark = midYearReviewerRemark;
        AnnualReviewerName = annualReviewerName;
        AnnualReviewerDate = annualReviewerDate;
        AnnualReviewerRemark = annualReviewerRemark;
        ModifiedDate = DateTime.UtcNow;
    }
}
