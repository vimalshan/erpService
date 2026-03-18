using Recruitment.Domain.Common;

namespace Recruitment.Domain.Entities;

/// <summary>
/// AssessmentParameter entity
/// </summary>
public class AssessmentParameter : Entity
{
    public decimal RecruitmentCycleNo { get; private set; }
    public decimal ParameterNo { get; private set; }
    public string ParameterName { get; private set; }

    // Required for EF Core
    public AssessmentParameter() { }

    public AssessmentParameter(
        decimal recruitmentCycleNo,
        decimal parameterNo,
        string parameterName)
    {
        RecruitmentCycleNo = recruitmentCycleNo;
        ParameterNo = parameterNo;
        ParameterName = parameterName;
        Id = parameterNo;
    }
}
