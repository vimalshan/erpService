using AccountingService.Domain.Common;
using AccountingService.Domain.Entities;

namespace AccountingService.Domain.Aggregates;

/// <summary>
/// Accounting Aggregate Root – orchestrates GL postings and account details.
/// </summary>
public class AccountingAggregate : BaseEntity, IAggregateRoot
{
    private readonly List<GlPosting> _postings = [];
    private readonly List<AccountDetail> _details = [];

    public string TrustCode { get; private set; } = default!;
    public long FinancialYear { get; private set; }

    public IReadOnlyCollection<GlPosting> Postings => _postings.AsReadOnly();
    public IReadOnlyCollection<AccountDetail> Details => _details.AsReadOnly();

    private AccountingAggregate() { }

    public static AccountingAggregate Create(string trustCode, long financialYear)
        => new() { TrustCode = trustCode, FinancialYear = financialYear };

    public GlPosting PostGlEntry(
        string accountCode, DateTime postingDate,
        decimal debitAmount, decimal creditAmount,
        long referenceId, string? remarks = null)
    {
        var posting = GlPosting.Create(accountCode, postingDate, debitAmount, creditAmount, referenceId, remarks);
        _postings.Add(posting);
        return posting;
    }
}
