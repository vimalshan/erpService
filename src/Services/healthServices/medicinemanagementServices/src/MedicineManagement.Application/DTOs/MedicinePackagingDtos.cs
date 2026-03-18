namespace MedicineManagement.Application.DTOs;

public record MedicinePackagingDto(string PackagingCode, string? PackagingType);
public record CreateMedicinePackagingDto(string PackagingCode, string? PackagingType);
public record UpdateMedicinePackagingDto(string? PackagingType);
