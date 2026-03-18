namespace MedicineManagement.Application.DTOs;

public record MedicineIssueDto(
    string? CompanyCode, string? TransactionNumber, string? TransactionDate,
    long? IssuedQuantity, string? VisitNumber, string? MedicineCode);

public record CreateMedicineIssueDto(
    string CompanyCode, string TransactionNumber, string MedicineCode,
    long IssuedQuantity, string VisitNumber);
