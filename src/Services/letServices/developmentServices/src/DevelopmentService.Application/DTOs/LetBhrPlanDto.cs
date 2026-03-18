namespace DevelopmentService.Application.DTOs;

public record LetBhrPlanDto(
    long ReqNum,
    long? Sno,
    string? UserId,
    string? TrainingProgram,
    decimal? TrainingCode,
    decimal? Priority,
    long? PiNum,
    string? FinalAccept,
    char? BhrAccept);
