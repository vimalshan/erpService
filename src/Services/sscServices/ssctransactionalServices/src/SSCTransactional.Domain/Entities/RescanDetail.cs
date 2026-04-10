using SSCTransactional.Domain.Common;
using SSCTransactional.Domain.Events;

namespace SSCTransactional.Domain.Entities;

/// <summary>Maps to DOC_RESCANDET — Rescan workflow records</summary>
public class RescanDetail : Entity<long>
{
    public long DocId { get; private set; }
    public long AllocationId { get; private set; }
    public string Status { get; private set; } = "N";           // N=Pending, Y=Completed
    public DateTime RescanDate { get; private set; }
    public string RescanTo { get; private set; } = default!;    // S=SSC, U=User
    public string RescanRemarks { get; private set; } = default!;
    public DateTime? CompletedOn { get; private set; }
    public long? CompletedBy { get; private set; }
    public string? CompletionRemarks { get; private set; }
    public string? FilePath { get; private set; }

    private RescanDetail() { }

    public static RescanDetail Create(long id, long docId, long allocationId, string rescanTo, string remarks)
    {
        var rescan = new RescanDetail
        {
            Id = id,
            DocId = docId,
            AllocationId = allocationId,
            Status = "N",
            RescanDate = DateTime.UtcNow,
            RescanTo = rescanTo,
            RescanRemarks = remarks
        };

        rescan.RaiseDomainEvent(new RescanRequestedDomainEvent(id, docId, allocationId));
        return rescan;
    }

    public void Complete(long completedBy, string completionRemarks, string? filePath = null)
    {
        Status = "Y";
        CompletedOn = DateTime.UtcNow;
        CompletedBy = completedBy;
        CompletionRemarks = completionRemarks;
        FilePath = filePath;
    }
}
