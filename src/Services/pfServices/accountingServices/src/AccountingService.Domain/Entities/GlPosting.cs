using AccountingService.Domain.Common;

namespace AccountingService.Domain.Entities;

/// <summary>Maps to GL_POSTING table – general ledger postings.</summary>
public class GlPosting : BaseEntity
{
    public long PostingId { get; private set; }
    public string AccountCode { get; private set; } = default!;
    public DateTime PostingDate { get; private set; }
    public decimal DebitAmount { get; private set; }
    public decimal CreditAmount { get; private set; }
    public long ReferenceId { get; private set; }
    public string? PostingRemarks { get; private set; }

    // Navigation
    public MainAccount? Account { get; private set; }

    private GlPosting() { }

    public static GlPosting Create(
        string accountCode, DateTime postingDate,
        decimal debitAmount, decimal creditAmount,
        long referenceId, string? remarks = null)
    {
        if (debitAmount <= 0 && creditAmount <= 0)
            throw new ArgumentException("Either debit or credit amount must be greater than zero.");

        var entity = new GlPosting
        {
            AccountCode = accountCode,
            PostingDate = postingDate,
            DebitAmount = debitAmount,
            CreditAmount = creditAmount,
            ReferenceId = referenceId,
            PostingRemarks = remarks
        };

        entity.AddDomainEvent(new Events.GlPostedEvent(entity));
        return entity;
    }
}
