namespace Recruitment.Application.DTOs;

/// <summary>
/// DTO for Application (job application)
/// </summary>
public class ApplicationDto
{
    public decimal ApplicationNumber { get; set; }
    public decimal JobId { get; set; }
    public string SparshId { get; set; }
    public decimal SparshPin { get; set; }
    public string CurrentJobDescription { get; set; }
    public string Achievements { get; set; }
    public string ReasonForJoining { get; set; }
    public string Strength { get; set; }
    public string Awards { get; set; }
    public decimal? CrtMarks { get; set; }
    public decimal? DomainMarks { get; set; }
    public string CrtDocumentPath { get; set; }
    public string DomainDocumentPath { get; set; }
    public string Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public List<CourseDetailDto> CourseDetails { get; set; } = new();
}

/// <summary>
/// DTO for creating a new Application
/// </summary>
public class CreateApplicationDto
{
    public decimal ApplicationNumber { get; set; }
    public decimal JobId { get; set; }
    public string SparshId { get; set; }
    public decimal SparshPin { get; set; }
}

/// <summary>
/// DTO for updating Application details
/// </summary>
public class UpdateApplicationDto
{
    public decimal ApplicationNumber { get; set; }
    public string CurrentJobDescription { get; set; }
    public string Achievements { get; set; }
    public string ReasonForJoining { get; set; }
    public string Strength { get; set; }
    public string Awards { get; set; }
}

/// <summary>
/// DTO for Course Details
/// </summary>
public class CourseDetailDto
{
    public decimal SerialNo { get; set; }
    public string CourseTitle { get; set; }
    public string Duration { get; set; }
    public string Institute { get; set; }
}

/// <summary>
/// DTO for Application Status History
/// </summary>
public class ApplicationStatusHistoryDto
{
    public decimal ApplicationNumber { get; set; }
    public decimal SerialNo { get; set; }
    public string Status { get; set; }
    public string Remarks { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
}
