namespace TransactionService.Application.DTOs;

public class DemandMasterDto
{
    public long Id { get; set; }
    public string DemandType { get; set; } = string.Empty;
    public long DepartmentId { get; set; }
    public string DemandDescription { get; set; } = string.Empty;
    public DateTime RequiredDate { get; set; }
    public string Priority { get; set; } = string.Empty;
    public char DemandStatus { get; set; }
    public long CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string? ApprovalRemarks { get; set; }
    public long? ApprovedBy { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public string? CompletionRemarks { get; set; }
    public long? CompletedBy { get; set; }
    public DateTime? CompletionDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class SaaBudgetDto
{
    public long Id { get; set; }
    public long BusinessId { get; set; }
    public long YearId { get; set; }
    public decimal BudgetAmount { get; set; }
    public long UpdatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class SaaPeriodDto
{
    public long Id { get; set; }
    public long YearId { get; set; }
    public long QuarterNo { get; set; }
    public char Status { get; set; }
    public DateTime PeriodOpenDate { get; set; }
    public DateTime PeriodCloseDate { get; set; }
    public DateTime? CircularGenOn { get; set; }
    public long? CircularGenBy { get; set; }
    public DateTime? ReminderLetOn { get; set; }
    public DateTime FormOpenDate { get; set; }
    public DateTime? AppraiserLastDate { get; set; }
    public DateTime? ReviewerLastDate { get; set; }
    public DateTime? BhrLastDate { get; set; }
    public DateTime? UhrLastDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class SaaLevelDto
{
    public long Id { get; set; }
    public string LevelDesc { get; set; } = string.Empty;
    public string LevelAmount { get; set; } = string.Empty;
    public string LevelReason { get; set; } = string.Empty;
    public decimal LevelMin { get; set; }
    public decimal LevelMax { get; set; }
    public DateTime LevelEffDate { get; set; }
    public DateTime? LevelCloseDate { get; set; }
    public long LevelUpdatedBy { get; set; }
    public DateTime LevelUpdatedOn { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class SaaRecommendDto
{
    public long Id { get; set; }
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
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class SaaSubmitDto
{
    public long Id { get; set; }
    public long PeriodId { get; set; }
    public long BusId { get; set; }
    public char BhrFlag { get; set; }
    public char ChrFlag { get; set; }
    public long BhrUpdBy { get; set; }
    public DateTime BhrUpdOn { get; set; }
    public decimal? BhrAmount { get; set; }
    public long? ChrUpdBy { get; set; }
    public DateTime? ChrUpdOn { get; set; }
    public decimal? ChrAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class SaaMailTriggerDto
{
    public long Id { get; set; }
    public long QuarterId { get; set; }
    public long EmpSysId { get; set; }
    public string MailId { get; set; } = string.Empty;
    public long TriggeredBy { get; set; }
    public DateTime TriggeredOn { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
