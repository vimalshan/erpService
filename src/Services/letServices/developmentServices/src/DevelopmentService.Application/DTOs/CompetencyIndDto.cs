namespace DevelopmentService.Application.DTOs;

public record CompetencyIndDto(
    decimal? SrlNo,
    string? Band,
    long? CompNum,
    char? IndFlag,
    string? IndDefn);
