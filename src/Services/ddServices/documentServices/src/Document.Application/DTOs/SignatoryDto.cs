namespace Document.Application.DTOs;

public record SignatoryDto(
    decimal SignatoryNumber,
    string? Name,
    string? Designation,
    string? LiveFlag,
    decimal? EmployeeSysId,
    string? ImageFileName);

public record CreateSignatoryRequest(
    decimal SignatoryNumber,
    string Name,
    string Designation,
    decimal? EmployeeSysId = null,
    string? ImageFileName = null);

public record UpdateSignatoryRequest(
    string Name,
    string Designation,
    string? ImageFileName = null);
