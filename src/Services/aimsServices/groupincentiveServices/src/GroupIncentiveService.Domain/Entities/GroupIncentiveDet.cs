using GroupIncentiveService.Domain.Exceptions;

namespace GroupIncentiveService.Domain.Entities;

public class GroupIncentiveDet : BaseEntity
{
    public long GrpIncDetId { get; private set; }
    public long GrpIncDetMainId { get; private set; }
    public long GrpIncDetEmpSysId { get; private set; }
    public decimal GrpIncDetAllocPercentage { get; private set; }
    public decimal GrpIncDetAllocAmount { get; private set; }
    public decimal? GrpIncDetApprovedAmount { get; private set; }
    public string GrpIncDetAppStatus { get; private set; } = "P";
    public long GrpIncDetLastModifiedBy { get; private set; }
    public DateTime GrpIncDetLastModifiedOn { get; private set; }

    public GroupIncentiveMain? Main { get; private set; }

    private GroupIncentiveDet() { }

    public static GroupIncentiveDet Create(long id, long mainId, long employeeId,
        decimal allocPercentage, decimal allocAmount, long createdBy)
    {
        if (allocPercentage < 0 || allocPercentage > 100)
            throw new DomainException("Allocation percentage must be between 0 and 100.");
        if (allocAmount < 0)
            throw new DomainException("Allocation amount cannot be negative.");

        return new GroupIncentiveDet
        {
            GrpIncDetId = id,
            GrpIncDetMainId = mainId,
            GrpIncDetEmpSysId = employeeId,
            GrpIncDetAllocPercentage = allocPercentage,
            GrpIncDetAllocAmount = allocAmount,
            GrpIncDetAppStatus = "P",
            GrpIncDetLastModifiedBy = createdBy,
            GrpIncDetLastModifiedOn = DateTime.UtcNow
        };
    }

    public void Approve(decimal approvedAmount, long approvedBy)
    {
        if (GrpIncDetAppStatus != "P")
            throw new DomainException("Only pending detail records can be approved.");

        GrpIncDetApprovedAmount = approvedAmount;
        GrpIncDetAppStatus = "Y";
        GrpIncDetLastModifiedBy = approvedBy;
        GrpIncDetLastModifiedOn = DateTime.UtcNow;
    }
}
