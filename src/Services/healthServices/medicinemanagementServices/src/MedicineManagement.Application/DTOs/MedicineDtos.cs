namespace MedicineManagement.Application.DTOs;

public record MedicineDto(
    string MedicineCode, string MedicineName, string MedicineTypeCode,
    char? Category, decimal? OrderLevelMin, decimal? OrderLevelMax);

public record CreateMedicineDto(
    string MedicineCode, string MedicineName, string MedicineTypeCode,
    char? Category, decimal? OrderLevelMin, decimal? OrderLevelMax);

public record UpdateMedicineDto(
    string MedicineName, string MedicineTypeCode,
    char? Category, decimal? OrderLevelMin, decimal? OrderLevelMax);
