using SSCTransactional.Domain.Common;
using SSCTransactional.Domain.Events;

namespace SSCTransactional.Domain.Aggregates;

/// <summary>
/// Correspondence aggregate root — maps to DOC_CORRESPOND.
/// Manages hold/release correspondence on documents.
/// </summary>
public class CorrespondenceAggregate : AggregateRoot<long>
{
    public long DocId { get; private set; }
    public long AllocationId { get; private set; }
    public long HoldCategory { get; private set; }
    public long HoldType { get; private set; }
    public DateTime HoldDate { get; private set; }
    public string HoldRemarks { get; private set; } = default!;
    public long HoldBy { get; private set; }
    public string HoldStatus { get; private set; } = "H";      // H/R
    public DateTime? ReleaseDate { get; private set; }
    public string? ReleaseRemarks { get; private set; }
    public long? ReleasedBy { get; private set; }
    public decimal? HoldNature { get; private set; }

    private readonly List<CorrespondenceAttachment> _attachments = new();
    public IReadOnlyCollection<CorrespondenceAttachment> Attachments => _attachments.AsReadOnly();

    private CorrespondenceAggregate() { }

    public static CorrespondenceAggregate Create(
        long id, long docId, long allocationId, long holdCategory, long holdType,
        string holdRemarks, long holdBy, decimal? holdNature = null)
    {
        var corr = new CorrespondenceAggregate
        {
            Id = id,
            DocId = docId,
            AllocationId = allocationId,
            HoldCategory = holdCategory,
            HoldType = holdType,
            HoldDate = DateTime.UtcNow,
            HoldRemarks = holdRemarks,
            HoldBy = holdBy,
            HoldStatus = "H",
            HoldNature = holdNature
        };

        corr.RaiseDomainEvent(new CorrespondenceCreatedDomainEvent(id, docId, holdCategory.ToString()));
        return corr;
    }

    public void Release(long releasedBy, string releaseRemarks)
    {
        if (HoldStatus == "R")
            throw new Exceptions.TransactionDomainException($"Correspondence {Id} is already released.");

        HoldStatus = "R";
        ReleaseDate = DateTime.UtcNow;
        ReleaseRemarks = releaseRemarks;
        ReleasedBy = releasedBy;

        RaiseDomainEvent(new CorrespondenceReleasedDomainEvent(Id, DocId));
    }

    public void AddAttachment(CorrespondenceAttachment attachment)
    {
        _attachments.Add(attachment);
    }
}
