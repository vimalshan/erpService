using GroupIncentiveService.Domain.Events;
using GroupIncentiveService.Domain.Exceptions;

namespace GroupIncentiveService.Domain.Entities;

public class GroupIncentiveMain : BaseEntity
{
    public long GrpIncId { get; private set; }
    public int GrpIncGroupId { get; private set; }
    public int GrpIncIncMonth { get; private set; }
    public int GrpIncIncYear { get; private set; }
    public decimal GrpIncTotalAmount { get; private set; }
    public string GrpIncAppStatus { get; private set; } = "P";
    public decimal? GrpIncApprovedAmount { get; private set; }
    public long? GrpIncApprover { get; private set; }
    public DateTime? GrpIncApprovalDate { get; private set; }
    public DateTime GrpIncEnteredOn { get; private set; }
    public long GrpIncEnteredBy { get; private set; }
    public long GrpIncLastModifiedBy { get; private set; }
    public DateTime GrpIncLastModifiedOn { get; private set; }

    public GroupMaster? Group { get; private set; }

    private readonly List<GroupIncentiveDet> _details = [];
    public IReadOnlyCollection<GroupIncentiveDet> Details => _details.AsReadOnly();

    private readonly List<GroupIncentiveApproval> _approvals = [];
    public IReadOnlyCollection<GroupIncentiveApproval> Approvals => _approvals.AsReadOnly();

    private GroupIncentiveMain() { }

    public static GroupIncentiveMain Create(long id, int groupId, int month, int year,
        decimal totalAmount, long createdBy)
    {
        if (month < 1 || month > 12) throw new DomainException("Invalid month.");
        if (year < 2000 || year > 2100) throw new DomainException("Invalid year.");
        if (totalAmount < 0) throw new DomainException("Total amount cannot be negative.");

        var incentive = new GroupIncentiveMain
        {
            GrpIncId = id,
            GrpIncGroupId = groupId,
            GrpIncIncMonth = month,
            GrpIncIncYear = year,
            GrpIncTotalAmount = totalAmount,
            GrpIncAppStatus = "P",
            GrpIncEnteredOn = DateTime.UtcNow,
            GrpIncEnteredBy = createdBy,
            GrpIncLastModifiedBy = createdBy,
            GrpIncLastModifiedOn = DateTime.UtcNow
        };

        incentive.AddDomainEvent(new GroupIncentiveCreatedEvent(id, groupId, month, year, totalAmount, createdBy));
        return incentive;
    }

    public void Approve(decimal approvedAmount, long approvedBy)
    {
        if (GrpIncAppStatus != "P")
            throw new DomainException("Only pending incentives can be approved.");
        if (approvedAmount < 0)
            throw new DomainException("Approved amount cannot be negative.");

        GrpIncAppStatus = "Y";
        GrpIncApprovedAmount = approvedAmount;
        GrpIncApprover = approvedBy;
        GrpIncApprovalDate = DateTime.UtcNow;
        GrpIncLastModifiedBy = approvedBy;
        GrpIncLastModifiedOn = DateTime.UtcNow;

        AddDomainEvent(new GroupIncentiveApprovedEvent(GrpIncId, GrpIncGroupId, approvedAmount, approvedBy));
    }

    public void Reject(long rejectedBy, string remarks)
    {
        if (GrpIncAppStatus != "P")
            throw new DomainException("Only pending incentives can be rejected.");

        GrpIncAppStatus = "N";
        GrpIncLastModifiedBy = rejectedBy;
        GrpIncLastModifiedOn = DateTime.UtcNow;

        AddDomainEvent(new GroupIncentiveRejectedEvent(GrpIncId, GrpIncGroupId, remarks, rejectedBy));
    }

    public bool IsPending => GrpIncAppStatus == "P";
    public bool IsApproved => GrpIncAppStatus == "Y";
    public bool IsRejected => GrpIncAppStatus == "N";
}
