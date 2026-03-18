namespace MedicineManagement.Application.DTOs;

public record MedicineTypeDto(string TypeCode, string? TypeName);
public record CreateMedicineTypeDto(string TypeCode, string? TypeName);
public record UpdateMedicineTypeDto(string? TypeName);
