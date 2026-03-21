using ArchiveService.Domain.Common;

namespace ArchiveService.Domain.Entities;

public class ArchivedServiceOrderDetail : BaseEntity
{
    public long Id { get; private set; }
    public string? SernoDell { get; private set; }
    public string? PartNo { get; private set; }
    public string? Quantity { get; private set; }
    public string? UniqueId { get; private set; }
    public string? PartStatus { get; private set; }

    private ArchivedServiceOrderDetail() { }

    public static ArchivedServiceOrderDetail Create(
        string? sernoDell, string? partNo, string? quantity,
        string? uniqueId, string? partStatus, string? enteredBy)
    {
        return new ArchivedServiceOrderDetail
        {
            SernoDell = sernoDell,
            PartNo = partNo,
            Quantity = quantity,
            UniqueId = uniqueId,
            PartStatus = partStatus,
            EnteredOn = DateTime.UtcNow,
            EnteredBy = enteredBy
        };
    }

    public void UpdatePartStatus(string? partStatus, string? changedBy)
    {
        PartStatus = partStatus;
        ChangedOn = DateTime.UtcNow;
        ChangedBy = changedBy;
    }
}
