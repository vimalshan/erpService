namespace ComplaintService.Application.DTOs;

public record ComplaintTicketDto(
    decimal TicketNum,
    decimal GroupId,
    decimal Type,
    decimal Location,
    decimal Department,
    decimal Process,
    string? Subject,
    string? Description,
    string? IsNCR,
    string TargetDate,
    DateTime? ClosureDate,
    bool IsClosed
);

public record CreateComplaintRequest(
    decimal GroupId,
    decimal Type,
    decimal Location,
    decimal Department,
    decimal Process,
    string? Subject,
    string? Description,
    bool IsNCR,
    int TargetResolutionHours = 48
);

public record UpdateActionRequest(
    decimal ActionNum,
    char ActionLevel,
    string Solution
);

public record CloseComplaintRequest(
    decimal TicketNum,
    string? FinalRemarks
);

public record ReopenComplaintRequest(
    decimal TicketNum,
    string Remarks
);
