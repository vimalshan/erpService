namespace ComplaintService.Application.DTOs;

public record ComplaintHistoryDto(
    decimal HistoryNum,
    decimal ActionNum,
    decimal SerialNum,
    string? From,
    string? To,
    DateTime ActionDate,
    char ActionType,
    string? Remarks
);
