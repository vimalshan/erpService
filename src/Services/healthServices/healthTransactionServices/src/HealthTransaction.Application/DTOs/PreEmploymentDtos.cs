namespace HealthTransaction.Application.DTOs;

public record PreEmploymentCheckupDto(
    decimal EmpNum,
    string ComCode,
    decimal HlthNum,
    string? PhysHandicap,
    string? ProposedEmp,
    string? IdentMarks,
    string? FinalRemarks,
    string? FitPh,
    string? FitFinal,
    DateTime? CheckupDate);

public record CreatePreEmploymentCheckupDto(
    decimal EmpNum,
    string ComCode,
    decimal HlthNum,
    string? PhysHandicap,
    string? ProposedEmp,
    string? IdentMarks,
    string? FinalRemarks,
    string? FitPh,
    string? FitFinal,
    DateTime? CheckupDate);
