using AccountingService.Domain.Common;

namespace AccountingService.Domain.Entities;

/// <summary>Maps to TRAN_DET table – transaction records.</summary>
public class TransactionDetail : BaseEntity
{
    public string TdTrustCode { get; private set; } = default!;
    public int TransactionId { get; private set; }
    public string TdTransactionCode { get; private set; } = default!;
    public string? TdTransactionType { get; private set; }
    public DateTime TdTransactionDate { get; private set; }
    public decimal TdAmount { get; private set; }
    public string? TdRemarks { get; private set; }
    public int? TdMemberNo { get; private set; }
    public string? TdReferenceType { get; private set; }
    public string? TdContributionReferenceNo { get; private set; }
    public string TdTypeCode { get; private set; } = default!;
    public DateTime TdLastModifiedOn { get; private set; }
    public string TdLastModifiedEmpSysid { get; private set; } = default!;
    public long TdFinyear { get; private set; }
    public string TdJvVoucherType { get; private set; } = default!;
    public string TdJvNo { get; private set; } = default!;
    public long? TdCancelStatus { get; private set; }
    public DateTime? TdCancelDate { get; private set; }
    public string? TdTrnSubType { get; private set; }

    private TransactionDetail() { }

    public static TransactionDetail Create(
        string trustCode, int transactionId, string transactionCode,
        DateTime transactionDate, decimal amount, string typeCode,
        string modifiedBy, long finYear, string jvVoucherType, string jvNo,
        string? transactionType = null, string? remarks = null,
        int? memberNo = null, string? referenceType = null,
        string? contributionRefNo = null, string? trnSubType = null)
    {
        var entity = new TransactionDetail
        {
            TdTrustCode = trustCode,
            TransactionId = transactionId,
            TdTransactionCode = transactionCode,
            TdTransactionType = transactionType,
            TdTransactionDate = transactionDate,
            TdAmount = amount,
            TdRemarks = remarks,
            TdMemberNo = memberNo,
            TdReferenceType = referenceType,
            TdContributionReferenceNo = contributionRefNo,
            TdTypeCode = typeCode,
            TdLastModifiedOn = DateTime.UtcNow,
            TdLastModifiedEmpSysid = modifiedBy,
            TdFinyear = finYear,
            TdJvVoucherType = jvVoucherType,
            TdJvNo = jvNo,
            TdTrnSubType = trnSubType
        };

        entity.AddDomainEvent(new Events.TransactionCreatedEvent(entity));
        return entity;
    }

    public void Cancel(string cancelledBy)
    {
        TdCancelStatus = 1;
        TdCancelDate = DateTime.UtcNow;
        TdLastModifiedOn = DateTime.UtcNow;
        TdLastModifiedEmpSysid = cancelledBy;
    }

    public bool IsCancelled => TdCancelStatus.HasValue;
}
