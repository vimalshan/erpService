namespace DevelopmentService.Application.DTOs;

public record LetPlanDto(
    long ReqNum,
    long? Sno,
    string? UserId,
    long? PinNum,
    string? DevSource,
    string? DevNeed,
    string? DevIndicator,
    long? DevMode,
    string? RecProg,
    string? TrainingProgram,
    long? InternalTraining,
    string? RevDate,
    long? Priority,
    DateTime? EntDate,
    char? AppStatus,
    char? BhrStatus);
