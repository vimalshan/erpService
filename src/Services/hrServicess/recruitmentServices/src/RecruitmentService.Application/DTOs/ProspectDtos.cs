namespace RecruitmentService.Application.DTOs;

public record ProspectDto(
    decimal WebUserId,
    string? FirstName,
    string? MiddleName,
    string? LastName,
    string? EmailId,
    string Status,
    DateTime? DateOfBirth,
    DateTime? CreatedOn,
    string? ProspectType,
    IEnumerable<ProspectAddressDto> Addresses,
    IEnumerable<ProspectQualificationDto> Qualifications,
    IEnumerable<ProspectReferenceDto> References,
    IEnumerable<ProspectTrainingDto> Trainings
);

public record ProspectSummaryDto(
    decimal WebUserId,
    string FullName,
    string? EmailId,
    string Status,
    DateTime? DateOfBirth,
    DateTime? CreatedOn
);

public record ProspectAddressDto(
    decimal EmpSysId,
    string AddressFlag,
    string? Address1,
    string? Address2,
    string? Address3,
    string? Address4,
    decimal? City,
    decimal? PinCode,
    string? MobileNo,
    string? LandlineNo
);

public record ProspectQualificationDto(
    decimal EmpSysId,
    decimal QualId,
    decimal QualCode,
    string? QualDescription,
    string? YearFrom,
    string? YearTo,
    decimal? InstitutionCode,
    string? InstitutionDescription,
    string? EducationType,
    decimal? SpecializationCode,
    string? SpecializationDescription,
    string? Percentage,
    decimal? DegreeCode,
    string? DegreeDescription
);

public record ProspectReferenceDto(
    decimal EmpSysId,
    decimal RefId,
    string? Name,
    string? Designation,
    string? Address1,
    string? Address2,
    string? Phone,
    string? Email
);

public record ProspectTrainingDto(
    decimal EmpSysId,
    decimal TrainingId,
    string? Title,
    string? Duration,
    string? Institute,
    string? Location
);

public record RegisterProspectRequest(
    decimal UserId,
    string FirstName,
    string? MiddleName,
    string LastName,
    string EmailId,
    string Password,
    DateTime? DateOfBirth,
    string? ProspectType
);

public record LoginRequest(string Email, string Password);

public record AuthResponse(string Token, decimal UserId, string FullName, string Email);
