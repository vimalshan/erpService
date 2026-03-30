namespace EmployeeManagement.Application.Promotions.DTOs;

public sealed record PromotionDto(
    long PromotionNo,
    long EmployeeId,
    string Source,
    long OldGradeId,
    long NewGradeId,
    string Status,
    string? Designation,
    string? PromotionType,
    DateTime CreatedOn,
    long CreatedBy
);
