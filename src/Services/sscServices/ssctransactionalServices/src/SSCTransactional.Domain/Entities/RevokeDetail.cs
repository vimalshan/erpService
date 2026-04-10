using SSCTransactional.Domain.Common;
using SSCTransactional.Domain.Events;

namespace SSCTransactional.Domain.Entities;

/// <summary>Maps to DOC_REVOKEDET — Document revocation records</summary>
public class RevokeDetail : Entity<long>
{
    public long DocId { get; private set; }
    public string RevokeRemarks { get; private set; } = default!;
    public string RevokeStatus { get; private set; } = default!;
    public long RevokedBy { get; private set; }
    public DateTime RevokedOn { get; private set; }

    private RevokeDetail() { }

    public static RevokeDetail Create(long id, long docId, string remarks, string status, long revokedBy)
    {
        var revoke = new RevokeDetail
        {
            Id = id,
            DocId = docId,
            RevokeRemarks = remarks,
            RevokeStatus = status,
            RevokedBy = revokedBy,
            RevokedOn = DateTime.UtcNow
        };

        revoke.RaiseDomainEvent(new DocumentRevokedDomainEvent(id, docId));
        return revoke;
    }
}
