namespace BankService.Application.DTOs;

public record ChequeDetailDto
{
    public long? ChequeActranNo { get; init; }
    public long ChequeId { get; init; }
    public string? ChequeBranch { get; init; }
    public decimal? ChequeNo { get; init; }
    public DateTime? ChequeDate { get; init; }
    public long? ChequeBank { get; init; }
    public string? ChequeRemarks { get; init; }
    public decimal? ChequeAmount { get; init; }
    public string ChequeStatus { get; init; } = null!;
    public string? ChequePayee { get; init; }
    public DateTime? ChequeClearedDate { get; init; }
}
