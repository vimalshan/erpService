namespace FinanceService.Application.DTOs;

public class PaymentDto
{
    public long TransactionNumber { get; set; }
    public string UnitCode { get; set; } = string.Empty;
    public string? DebitCreditFlag { get; set; }
    public decimal? TransactionAmount { get; set; }
    public string? Remarks { get; set; }
    public string? AccountType { get; set; }
    public string? JvPostingStatus { get; set; }
}
