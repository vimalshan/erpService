namespace EmployeeManagement.Application.Employees.DTOs;

public sealed record EmployeeDto(
    long Id,
    string? EmployeeNo,
    string? BusinessUnit,
    string? Unit,
    long? GradeId,
    string? Designation,
    long? DivisionId,
    long? DepartmentId,
    long? PositionId,
    bool IsActive,
    DateTime CreatedOn,
    long CreatedBy
);

public sealed record EmployeeSummaryDto(
    long Id,
    string? EmployeeNo,
    string? Designation,
    string? Unit,
    bool IsActive
);

public sealed record EmployeeAddressDto(
    long EmployeeId,
    char AddressFlag,
    string? Line1,
    string? Line2,
    string? Line3,
    string? Line4,
    long? CityId,
    string? CityOthers,
    long? PinCode,
    long? StateId
);

public sealed record EmployeeQualificationDto(
    long QualificationId,
    long EmployeeId,
    string? QualDescription,
    string? YearFrom,
    string? YearTo,
    string? InstitutionDesc,
    char? EducationType,
    string? Percentage,
    string? DegreeDesc
);

public sealed record EmployeeCareerDto(
    long CareerId,
    long EmployeeId,
    string? Business,
    string? Unit,
    DateTime? From,
    DateTime? To,
    string? EmployeeNo,
    string? Designation,
    string? Reason
);
