using TaskTransactional.Domain.Common;
using TaskTransactional.Domain.Events;

namespace TaskTransactional.Domain.Entities;

public class ComplaintMain : AggregateRoot
{
    public string CmUnitCode { get; private set; } = null!;
    public string CmGroupId { get; private set; } = null!;
    public string CmGroupName { get; private set; } = null!;
    public string? CmGroupDesc { get; private set; }
    public decimal CmGroupSrc { get; private set; }
    public string? CmBehalfFlg { get; private set; }
    public decimal? CmBehalfPin { get; private set; }
    public decimal? CmRegPin { get; private set; }
    public string? CmShift { get; private set; }
    public string? CmMail { get; private set; }
    public string? CmSubmit { get; private set; }
    public DateTime? CmRegDate { get; private set; }
    public string? CmUpdatedBy { get; private set; }
    public DateTime? CmUpdatedOn { get; private set; }

    // Note: CD_GROUPID is decimal while CM_GROUPID is varchar - no EF FK relationship

    private ComplaintMain() { }

    public static ComplaintMain Create(
        string unitCode, string groupId, string groupName, decimal groupSrc,
        string? groupDesc = null, string? behalfFlg = null, decimal? behalfPin = null,
        decimal? regPin = null, string? shift = null, string? mail = null)
    {
        var entity = new ComplaintMain
        {
            CmUnitCode = unitCode,
            CmGroupId = groupId,
            CmGroupName = groupName,
            CmGroupSrc = groupSrc,
            CmGroupDesc = groupDesc,
            CmBehalfFlg = behalfFlg,
            CmBehalfPin = behalfPin,
            CmRegPin = regPin,
            CmShift = shift,
            CmMail = mail,
            CmSubmit = "Y",
            CmRegDate = DateTime.UtcNow
        };

        entity.AddDomainEvent(new ComplaintCreatedEvent(groupId, groupName, unitCode));
        return entity;
    }

    public void Update(string groupName, string? groupDesc, string? mail, string updatedBy)
    {
        CmGroupName = groupName;
        CmGroupDesc = groupDesc;
        CmMail = mail;
        CmUpdatedBy = updatedBy;
        CmUpdatedOn = DateTime.UtcNow;
        AddDomainEvent(new ComplaintUpdatedEvent(CmGroupId, groupName));
    }
}
