namespace Recruitment.Application.DTOs;

/// <summary>
/// DTO for Job data transfer
/// </summary>
public class JobDto
{
    public decimal JobId { get; set; }
    public decimal RecruitmentCycleNo { get; set; }
    public string JobDescription { get; set; }
    public string RoleDetails { get; set; }
    public string CadreCode { get; set; }
    public DateTime EffectiveDate { get; set; }
    public string PrincipalAccount { get; set; }
    public string JobType { get; set; }
    public string BusinessCode { get; set; }
    public string UnitCode { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>
/// DTO for creating a new Job
/// </summary>
public class CreateJobDto
{
    public decimal JobId { get; set; }
    public decimal RecruitmentCycleNo { get; set; }
    public string JobDescription { get; set; }
    public string RoleDetails { get; set; }
    public string CadreCode { get; set; }
    public DateTime EffectiveDate { get; set; }
    public string PrincipalAccount { get; set; }
    public string JobType { get; set; }
    public string BusinessCode { get; set; }
    public string UnitCode { get; set; }
}

/// <summary>
/// DTO for updating a Job
/// </summary>
public class UpdateJobDto
{
    public decimal JobId { get; set; }
    public string JobDescription { get; set; }
    public string RoleDetails { get; set; }
    public string JobType { get; set; }
}
