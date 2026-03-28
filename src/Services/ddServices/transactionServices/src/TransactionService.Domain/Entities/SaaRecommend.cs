namespace TransactionService.Domain.Entities;

public class SaaRecommend : BaseEntity
{
    public long YearId { get; set; }
    public long PeriodId { get; set; }
    public long EmpSysId { get; set; }
    public long LevelId { get; set; }
    public decimal CtcAmount { get; set; }
    public decimal MaximumCap { get; set; }
    public decimal EligibilityAmount { get; set; }
    public decimal? RecommendAmount { get; set; }
    public string InitiativeTaken { get; set; } = string.Empty;
    public string Results { get; set; } = string.Empty;
    public string? AddRemarks { get; set; }
    public long Status { get; set; }
    public long? RejectionBy { get; set; }
    public DateTime? RejectionOn { get; set; }
    public string RecommendBy { get; set; } = string.Empty;
    public long? RecommendSubmitBy { get; set; }
    public DateTime? RecommendSubmitOn { get; set; }
    public long? ReviewerSubmitBy { get; set; }
    public DateTime? ReviewerSubmitOn { get; set; }
    public long? BhrSubmitBy { get; set; }
    public DateTime? BhrSubmitOn { get; set; }
    public long? ChrSubmitBy { get; set; }
    public DateTime? ChrSubmitOn { get; set; }
    public string? RejectionRemarks { get; set; }
    public long? FinalLevel { get; set; }
    public decimal? FinalAmount { get; set; }
    public string? InitiativeLetter { get; set; }
    public string? ResultsLetter { get; set; }
    public long? UhrSubmitBy { get; set; }
    public DateTime? UhrSubmitOn { get; set; }
    public long? RecommendSignId { get; set; }
    public long? RecommendSignId2 { get; set; }

    public SaaPeriod? Period { get; set; }
    public SaaLevel? Level { get; set; }

    public SaaRecommend() { }

    public SaaRecommend(long yearId, long periodId, long empSysId, long levelId, decimal ctcAmount,
        decimal maximumCap, decimal eligibilityAmount, string initiativeTaken, string results, string recommendBy)
    {
        YearId = yearId;
        PeriodId = periodId;
        EmpSysId = empSysId;
        LevelId = levelId;
        CtcAmount = ctcAmount;
        MaximumCap = maximumCap;
        EligibilityAmount = eligibilityAmount;
        InitiativeTaken = initiativeTaken;
        Results = results;
        RecommendBy = recommendBy;
        Status = 0;
    }

    public bool IsRejected => RejectionBy.HasValue;
    public bool IsFullyApproved => ChrSubmitBy.HasValue;
}
