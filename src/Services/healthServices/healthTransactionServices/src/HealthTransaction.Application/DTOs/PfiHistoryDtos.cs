namespace HealthTransaction.Application.DTOs;

public record PfiHistoryDto(
    decimal HlthNum,
    decimal EmpNum,
    decimal SympId,
    string? YnFlag,
    DateTime? ImmDate,
    string? TestValue);

public record CreatePfiHistoryDto(
    decimal HlthNum,
    decimal EmpNum,
    decimal SympId,
    string? YnFlag,
    DateTime? ImmDate,
    string? TestValue);
