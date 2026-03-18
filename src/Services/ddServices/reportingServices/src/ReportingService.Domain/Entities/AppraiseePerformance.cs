namespace ReportingService.Domain.Entities;

/// <summary>
/// Appraisee Self Performance Entity
/// </summary>
public class AppraiseePerformance : BaseEntity
{
    public long RequestNumber { get; set; }
    public long PerformanceSerialNumber { get; set; }
    public string? Description { get; set; }
    public string? PerformanceRating { get; set; }
    public string? UnitFrom { get; set; }
    public string? UnitTo { get; set; }
    public string? UnitActual { get; set; }
    public string? PerformanceRemarks { get; set; }
    public decimal? AssessmentWeightage { get; set; }
    public DateTime? CandidateDate { get; set; }
    public string? AppraIserId { get; set; }
    public long? AppraIsalNumber { get; set; }
    public string? CandidateRemark { get; set; }
    public string? UnitMeasure { get; set; }
    public string? PerformanceRating1 { get; set; }
    public string? PerformanceRemark1 { get; set; }
    public string? PerformanceCategory { get; set; }
    public decimal? PerformanceRatingValue { get; set; }
    public decimal? PerfRating { get; set; }
    public decimal? MeanRating { get; set; }
    public decimal? AppPerfRating { get; set; }
    public decimal? AppMeanRating { get; set; }
    public string? MeanRemarks { get; set; }
    public string? AppMeanRemarks { get; set; }

    public AppraiseePerformance() { }

    public AppraiseePerformance(long requestNumber, long performanceSerialNumber)
    {
        RequestNumber = requestNumber;
        PerformanceSerialNumber = performanceSerialNumber;
    }
}
