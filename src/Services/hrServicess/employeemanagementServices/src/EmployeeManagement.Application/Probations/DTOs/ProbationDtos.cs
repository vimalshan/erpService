namespace EmployeeManagement.Application.Probations.DTOs;

public sealed record ProbationDto(
    long ProbationId,
    long EmployeeId,
    long GradeId,
    DateTime DueDate,
    char ProbationStatus,
    bool IsExtended,
    string? Rating,
    DateTime CreatedOn,
    long CreatedBy
);
