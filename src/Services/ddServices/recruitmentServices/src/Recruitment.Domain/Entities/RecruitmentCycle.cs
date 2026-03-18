using Recruitment.Domain.Common;

namespace Recruitment.Domain.Entities;

/// <summary>
/// RecruitmentCycle entity representing a recruitment cycle
/// </summary>
public class RecruitmentCycle : Entity
{
    public decimal RecruitmentCycleNo { get; private set; }
    public DateTime EffectiveFromDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public bool IsActive { get; private set; }

    // Required for EF Core
    public RecruitmentCycle() { }

    public RecruitmentCycle(
        decimal recruitmentCycleNo,
        DateTime effectiveFromDate,
        DateTime endDate)
    {
        RecruitmentCycleNo = recruitmentCycleNo;
        EffectiveFromDate = effectiveFromDate;
        EndDate = endDate;
        IsActive = true;
        Id = recruitmentCycleNo;
    }

    public void Deactivate()
    {
        IsActive = false;
        ModifiedDate = DateTime.UtcNow;
    }

    public void ExtendEndDate(DateTime newEndDate)
    {
        if (newEndDate <= EffectiveFromDate)
            throw new ArgumentException("End date must be after effective from date");
        
        EndDate = newEndDate;
        ModifiedDate = DateTime.UtcNow;
    }
}
