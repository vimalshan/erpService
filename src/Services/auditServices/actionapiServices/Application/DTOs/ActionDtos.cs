namespace ActionService.Application.DTOs;

public record ActionDto(
    int Id,
    string Action,
    DateTime? DueDate,
    bool HighPriority,
    string? Message,
    string? Language,
    string? Service,
    string? Site,
    string? EntityType,
    int? EntityId,
    string? Subject,
    string? SnowLink
);

public record CreateActionDto(
    string Action,
    DateTime? DueDate,
    bool HighPriority,
    string? Message,
    string? Language,
    string? Service,
    string? Site,
    string? EntityType,
    int? EntityId,
    string? Subject,
    string? SnowLink
);

public record UpdateActionDto(
    int Id,
    string Action,
    DateTime? DueDate,
    bool HighPriority,
    string? Message,
    string? Language,
    string? Service,
    string? Site,
    string? EntityType,
    int? EntityId,
    string? Subject,
    string? SnowLink
);

public record ActionListResponse(
    IEnumerable<ActionDto> Items,
    int TotalCount
);
