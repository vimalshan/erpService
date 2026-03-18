namespace Recruitment.API.GraphQL.Types;

public class JobType
{
    public decimal JobId { get; set; }
    public string JobDescription { get; set; }
    public string RoleDetails { get; set; }
    public string CadreCode { get; set; }
    public DateTime EffectiveDate { get; set; }
    public string PrincipalAccount { get; set; }
    public string Type { get; set; }
    public string BusinessCode { get; set; }
    public string UnitCode { get; set; }
    public bool IsActive { get; set; }
}

public class ApplicationType
{
    public decimal ApplicationNumber { get; set; }
    public decimal JobId { get; set; }
    public string SparshId { get; set; }
    public string Status { get; set; }
    public string Achievements { get; set; }
    public string ReasonForJoining { get; set; }
    public string Strength { get; set; }
    public decimal? CrtMarks { get; set; }
    public decimal? DomainMarks { get; set; }
    public DateTime CreatedDate { get; set; }
    public List<CourseDetailType> CourseDetails { get; set; } = new();
}

public class CourseDetailType
{
    public string CourseTitle { get; set; }
    public string Duration { get; set; }
    public string Institute { get; set; }
}
