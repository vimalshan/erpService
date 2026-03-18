using Recruitment.Domain.Common;

namespace Recruitment.Domain.Entities;

/// <summary>
/// SteeringCommitteeAssessment entity
/// </summary>
public class SteeringCommitteeAssessment : Entity
{
    public decimal ApplicationNumber { get; private set; }
    public decimal ParameterNo { get; private set; }
    public decimal CommitteeMemberPin { get; private set; }
    public string CommitteeMemberId { get; private set; }
    public string ParameterRemarks { get; private set; }
    public string OtherRemarks { get; private set; }
    public decimal Mark { get; private set; }

    public SteeringCommitteeAssessment(
        decimal applicationNumber,
        decimal parameterNo,
        decimal committeeMemberPin,
        string committeeMemberId,
        decimal mark)
    {
        ApplicationNumber = applicationNumber;
        ParameterNo = parameterNo;
        CommitteeMemberPin = committeeMemberPin;
        CommitteeMemberId = committeeMemberId;
        Mark = mark;
        Id = parameterNo;
    }

    public void UpdateAssessment(decimal mark, string parameterRemarks, string otherRemarks)
    {
        Mark = mark;
        ParameterRemarks = parameterRemarks;
        OtherRemarks = otherRemarks;
        ModifiedDate = DateTime.UtcNow;
    }
}
