namespace ReportingService.Application.DTOs;

public class AppraisalDto
{
    public long Id { get; set; }
    public long RequestNumber { get; set; }
    public string? UserName { get; set; }
    public string? UserId { get; set; }
    public string? StatusDescription { get; set; }
    public string? FinancialPeriod { get; set; }
    public string? UnitCode { get; set; }
    public string? GradeCode { get; set; }
    public string? AcademicYear { get; set; }
    public string? DDType { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public ICollection<AppraisalGoalDto> Goals { get; set; } = new List<AppraisalGoalDto>();
    public ICollection<AppraiseePerformanceDto> Performances { get; set; } = new List<AppraiseePerformanceDto>();
}

public class AppraisalGoalDto
{
    public long Id { get; set; }
    public long RequestNumber { get; set; }
    public string? Description { get; set; }
    public decimal? Weightage { get; set; }
    public string? Achievement { get; set; }
    public string? Category { get; set; }
    public string? AppraisalStatus { get; set; }
}

public class AppraiseePerformanceDto
{
    public long Id { get; set; }
    public long RequestNumber { get; set; }
    public string? Description { get; set; }
    public decimal? MeanRating { get; set; }
    public decimal? PerformanceRatingValue { get; set; }
    public string? PerformanceCategory { get; set; }
}

public class DDRatingDto
{
    public long Id { get; set; }
    public string? UserId { get; set; }
    public string? BusinessName { get; set; }
    public string? UnitName { get; set; }
    public decimal? TotalRating { get; set; }
    public decimal? TotalPercentage { get; set; }
    public decimal? Rating1 { get; set; }
    public decimal? Rating2 { get; set; }
    public decimal? Rating3 { get; set; }
    public decimal? Rating4 { get; set; }
    public decimal? Rating5 { get; set; }
}
