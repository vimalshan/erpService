namespace ErrorLoggingService.Application.DTOs;

public record ErrorLogDto(
    int Id,
    string? ErrorMessage,
    string? StoredProcedureName,
    int? ErrorReference,
    DateTime? ErrorDate
);
