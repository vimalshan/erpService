namespace BankService.Application.DTOs;

public record PaymentReconciliationDto
{
    public long ReconId { get; init; }
    public long ChequeId { get; init; }
    public string ReconReference { get; init; } = null!;
    public decimal ReconAmount { get; init; }
    public DateTime ReconDate { get; init; }
    public string ReconStatus { get; init; } = null!;
}
