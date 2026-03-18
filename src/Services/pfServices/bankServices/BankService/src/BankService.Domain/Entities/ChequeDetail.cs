using BankService.Domain.Common;

namespace BankService.Domain.Entities;

public class ChequeDetail : AggregateRoot
{
    public long? ChequeActranNo { get; private set; }
    public long ChequeId { get; private set; }
    public string? ChequeBranch { get; private set; }
    public decimal? ChequeNo { get; private set; }
    public DateTime? ChequeDate { get; private set; }
    public long? ChequeBank { get; private set; }
    public string? ChequeRemarks { get; private set; }
    public decimal? ChequeAmount { get; private set; }
    public string ChequeStatus { get; private set; } = "I";
    public string? ChequePayee { get; private set; }
    public DateTime? ChequeClearedDate { get; private set; }

    // Navigation
    public PaymentReconciliation? Reconciliation { get; private set; }

    private ChequeDetail() { }

    public static ChequeDetail Issue(long chequeId, decimal chequeNo, decimal amount,
        DateTime chequeDate, string payee)
    {
        if (amount <= 0)
            throw new InvalidOperationException("Cheque amount must be greater than zero.");

        var cheque = new ChequeDetail
        {
            ChequeId = chequeId,
            ChequeNo = chequeNo,
            ChequeAmount = amount,
            ChequeDate = chequeDate,
            ChequePayee = payee,
            ChequeStatus = "I"
        };

        cheque.AddDomainEvent(new Events.ChequeIssuedEvent(chequeId, amount, payee));
        return cheque;
    }

    public void Clear(DateTime clearedDate)
    {
        ChequeStatus = "C";
        ChequeClearedDate = clearedDate;
        AddDomainEvent(new Events.ChequeClearedEvent(ChequeId, clearedDate));
    }

    public void MarkOutstanding()
    {
        ChequeStatus = "O";
    }

    public void SetBankDetails(long? bankId, string? branch, string? remarks)
    {
        ChequeBank = bankId;
        ChequeBranch = branch;
        ChequeRemarks = remarks;
    }
}
