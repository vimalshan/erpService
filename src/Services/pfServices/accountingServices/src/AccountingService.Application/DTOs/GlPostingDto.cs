namespace AccountingService.Application.DTOs;

public record GlPostingDto(
    long PostingId,
    string AccountCode,
    DateTime PostingDate,
    decimal DebitAmount,
    decimal CreditAmount,
    long ReferenceId,
    string? PostingRemarks
);

public record TrialBalanceDto(
    string AccountCode,
    string? AccountName,
    decimal TotalDebit,
    decimal TotalCredit,
    decimal Balance
);
