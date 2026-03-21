using FinanceService.Domain.Common;
using FinanceService.Domain.Events;

namespace FinanceService.Domain.Entities;

public class TravelAccount : BaseEntity
{
    public long TransactionNumber { get; set; }
    public string UnitCode { get; set; } = string.Empty;
    public string? UserCode { get; set; }
    public long? UserNumber { get; set; }
    public string? DebitCreditFlag { get; set; }
    public decimal? TransactionAmount { get; set; }
    public string? AccountCode { get; set; }
    public string? Remarks { get; set; }
    public string? AccountType { get; set; }
    public string? JvPostingStatus { get; set; }

    public static TravelAccount CreatePayment(long transactionNumber, string unitCode,
        decimal amount, long batchNumber)
    {
        var account = new TravelAccount
        {
            TransactionNumber = transactionNumber,
            UnitCode = unitCode,
            DebitCreditFlag = "C",
            TransactionAmount = amount,
            Remarks = $"Batch Payment: {batchNumber}",
            AccountType = "SET"
        };
        account.AddDomainEvent(new PaymentProcessedEvent(transactionNumber, amount));
        return account;
    }
}
