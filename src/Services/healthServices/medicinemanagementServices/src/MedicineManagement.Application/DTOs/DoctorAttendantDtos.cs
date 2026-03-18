namespace MedicineManagement.Application.DTOs;

public record DoctorAttendantDto(string? Code, char? Flag, string? Name, long? SystemId);
public record CreateDoctorAttendantDto(string? Code, char? Flag, string? Name);
