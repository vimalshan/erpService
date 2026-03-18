namespace EmployeeManagement.Application.Promotions.DTOs;

public sealed record PromotionDto(
    long PromotionNo,
    long EmployeeId,
    string Source,
    long OldGradeId,
    long NewGradeId,
    char Status,
    string? Designation,
    char? PromotionType,
    DateTime CreatedOn,
    long CreatedBy
);
