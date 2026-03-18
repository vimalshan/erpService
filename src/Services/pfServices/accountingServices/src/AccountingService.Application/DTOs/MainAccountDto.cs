namespace AccountingService.Application.DTOs;

public record MainAccountDto(
    string MainAccountCode,
    string? MainAccountName,
    string? MainAccountShrtName,
    string? MainClosureFlag
);
