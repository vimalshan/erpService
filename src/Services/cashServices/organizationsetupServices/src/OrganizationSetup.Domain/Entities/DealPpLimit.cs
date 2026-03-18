using OrganizationSetup.Domain.Common;
using OrganizationSetup.Domain.Events;
using OrganizationSetup.Domain.ValueObjects;

namespace OrganizationSetup.Domain.Entities;

/// <summary>Maps to DEAL_PPLIMIT table - Provisional Prepayment Limit Management.</summary>
public class DealPpLimit : BaseEntity, IAggregateRoot
{
    public long PpLimitId { get; private set; }
    public long PpOrgId { get; private set; }
    public TransactionType PpTranType { get; private set; } = default!;
    public long PpBasCurr { get; private set; }
    public decimal? PpLimitAmt { get; private set; }
    public int PpFinYear { get; private set; }
    public decimal? PpLimitAct { get; private set; }
    public string? PpCertificateUpload { get; private set; }
    public decimal? PpModifiedBy { get; private set; }
    public DateTime? PpModifiedOn { get; private set; }

    private DealPpLimit() { }

    public static DealPpLimit Create(
        long limitId, long orgId, string tranType, long baseCurr,
        decimal? limitAmt, int finYear, decimal? limitAct, decimal? modifiedBy)
    {
        var limit = new DealPpLimit
        {
            PpLimitId = limitId,
            PpOrgId = orgId,
            PpTranType = TransactionType.Create(tranType),
            PpBasCurr = baseCurr,
            PpLimitAmt = limitAmt,
            PpFinYear = finYear,
            PpLimitAct = limitAct,
            PpModifiedBy = modifiedBy,
            PpModifiedOn = DateTime.UtcNow
        };
        limit.AddDomainEvent(new PpLimitSetEvent(limitId, orgId, tranType, limitAmt, finYear));
        return limit;
    }

    public void UpdateCertificate(string blobUrl, decimal modifiedBy)
    {
        PpCertificateUpload = blobUrl;
        PpModifiedBy = modifiedBy;
        PpModifiedOn = DateTime.UtcNow;
    }

    public void UpdateActual(decimal actualAmount, decimal modifiedBy)
    {
        PpLimitAct = actualAmount;
        PpModifiedBy = modifiedBy;
        PpModifiedOn = DateTime.UtcNow;
    }
}
