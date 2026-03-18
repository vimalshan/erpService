using InvestmentService.Domain.Common;

namespace InvestmentService.Domain.Entities;

public class BankDetail : BaseEntity
{
    public decimal TransactionId { get; set; }
    public string EntryType { get; set; } = null!;
    public string TransactionType { get; set; } = null!;
    public long InvNo { get; set; }
    public decimal TransactionAmount { get; set; }
    public long BankId { get; set; }
    public long DematId { get; set; }
    public DateTime TransactionDate { get; set; }
    public string Remarks { get; set; } = null!;

    // Navigation
    public Investment Investment { get; set; } = null!;
}
