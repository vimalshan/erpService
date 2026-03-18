namespace FillingOperationService.Application.DTOs;

public record PlanDeviationDto(
    int ReasonId,
    DateTime PlanDate,
    int FillingLineId,
    int ProductId,
    string? Reason
);
