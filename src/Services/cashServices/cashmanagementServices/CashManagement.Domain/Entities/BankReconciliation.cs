using CashManagement.Domain.Common;
using CashManagement.Domain.ValueObjects;

namespace CashManagement.Domain.Entities;

public class BankReconciliation : AggregateRoot
{
    public long BankAccountId { get; private set; }
    public decimal BankStatementBalance { get; private set; }
    public decimal LedgerBalance { get; private set; }
    public decimal? UnclearedCheques { get; private set; }
    public decimal? DifferenceAmount { get; private set; }
    public ReconciliationStatus? Status { get; private set; }
    public DateOnly ReconciliationDate { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }

    private BankReconciliation() { }

    public static BankReconciliation Create(long bankAccountId, decimal statementBalance,
        decimal ledgerBalance, decimal unclearedCheques, DateOnly reconciliationDate, long createdBy)
    {
        var diff = statementBalance - (ledgerBalance - unclearedCheques);
        var recon = new BankReconciliation
        {
            BankAccountId = bankAccountId,
            BankStatementBalance = statementBalance,
            LedgerBalance = ledgerBalance,
            UnclearedCheques = unclearedCheques,
            DifferenceAmount = diff,
            Status = diff == 0 ? ReconciliationStatus.Reconciled : ReconciliationStatus.Difference,
            ReconciliationDate = reconciliationDate,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };
        return recon;
    }
}
