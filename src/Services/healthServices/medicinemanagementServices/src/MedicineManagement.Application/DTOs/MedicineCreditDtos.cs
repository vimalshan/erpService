namespace MedicineManagement.Application.DTOs;

public record MedicineCreditDto(
    string CompanyCode, long TransactionCode, string MedicineCode,
    char RecordType, long Quantity, DateTime TransactionDate,
    string? LotNumber, char? CancelFlag);

public record CreateMedicineCreditDto(
    string CompanyCode, long TransactionCode, string MedicineCode,
    char RecordType, long Quantity, DateTime TransactionDate, string? LotNumber);

public record StockSummaryDto(string MedicineCode, string? MedicineName, long CurrentStock, decimal? MinLevel, decimal? MaxLevel);
