using Recruitment.Domain.Common;

namespace Recruitment.Domain.Entities;

/// <summary>
/// Job entity representing a job posting
/// </summary>
public class Job : Entity
{
    public decimal JobId { get; private set; }
    public decimal RecruitmentCycleNo { get; private set; }
    public string JobDescription { get; private set; }
    public string RoleDetails { get; private set; }
    public string CadreCode { get; private set; }
    public DateTime EffectiveDate { get; private set; }
    public string PrincipalAccount { get; private set; }
    public string JobType { get; private set; }
    public string BusinessCode { get; private set; }
    public string UnitCode { get; private set; }
    public bool IsActive { get; private set; }

    // Required for EF Core
    public Job() { }

    public Job(
        decimal jobId,
        decimal recruitmentCycleNo,
        string jobDescription,
        string roleDetails,
        string cadreCode,
        DateTime effectiveDate,
        string principalAccount,
        string jobType,
        string businessCode,
        string unitCode)
    {
        JobId = jobId;
        RecruitmentCycleNo = recruitmentCycleNo;
        JobDescription = jobDescription;
        RoleDetails = roleDetails;
        CadreCode = cadreCode;
        EffectiveDate = effectiveDate;
        PrincipalAccount = principalAccount;
        JobType = jobType;
        BusinessCode = businessCode;
        UnitCode = unitCode;
        IsActive = true;
        Id = jobId;
    }

    public void UpdateJobDetails(string jobDescription, string roleDetails, string jobType)
    {
        JobDescription = jobDescription;
        RoleDetails = roleDetails;
        JobType = jobType;
        ModifiedDate = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        ModifiedDate = DateTime.UtcNow;
    }
}
