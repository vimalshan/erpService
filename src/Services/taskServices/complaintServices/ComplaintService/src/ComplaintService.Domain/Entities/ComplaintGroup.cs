using ComplaintService.Domain.Common;

namespace ComplaintService.Domain.Entities;

/// <summary>Maps to COMPL_MAIN — complaint group registration.</summary>
public class ComplaintGroup : BaseEntity
{
    public string UnitCode { get; private set; } = default!;   // CM_UNIT_CODE
    public string GroupId { get; private set; } = default!;    // CM_GROUPID (PK)
    public string GroupName { get; private set; } = default!;  // CM_GROUP_NAME
    public string? GroupDesc { get; private set; }             // CM_GROUP_DESC
    public decimal GroupSrc { get; private set; }              // CM_GROUP_SRC
    public char? BehalfFlag { get; private set; }              // CM_BEHALF_FLG
    public decimal? BehalfPin { get; private set; }            // CM_BEHALF_PIN
    public decimal? RegPin { get; private set; }               // CM_REG_PIN
    public string? Shift { get; private set; }                 // CM_SHIFT
    public string? Mail { get; private set; }                  // CM_MAIL
    public string? Submit { get; private set; }                // CM_SUBMIT
    public DateTime? RegDate { get; private set; }             // CM_REG_DATE
    public string? UpdatedBy { get; private set; }             // CM_UPDATEDBY
    public DateTime? UpdatedOn { get; private set; }           // CM_UPDATEDON

    // Navigation
    public ICollection<ComplaintTicket> Tickets { get; private set; } = [];

    protected ComplaintGroup() { }

    public static ComplaintGroup Create(
        string unitCode, string groupId, string groupName,
        decimal groupSrc, decimal regPin, string? shift = null, string? mail = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(unitCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(groupId);
        ArgumentException.ThrowIfNullOrWhiteSpace(groupName);

        return new ComplaintGroup
        {
            UnitCode = unitCode.Trim(),
            GroupId = groupId.Trim(),
            GroupName = groupName.Trim(),
            GroupSrc = groupSrc,
            RegPin = regPin,
            Shift = shift,
            Mail = mail,
            RegDate = DateTime.UtcNow
        };
    }

    public void Update(string groupName, string? groupDesc, string? mail)
    {
        GroupName = groupName;
        GroupDesc = groupDesc;
        Mail = mail;
        UpdatedOn = DateTime.UtcNow;
    }
}
