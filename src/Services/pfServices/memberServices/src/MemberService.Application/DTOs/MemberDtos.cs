namespace MemberService.Application.DTOs;

public record MemberDto(
    long MemberNo,
    string TrustCode,
    string MemberName,
    string? FatherName,
    DateTime DateOfJoining,
    DateTime? DateOfBirth,
    string EmployeeType,
    string UnitCode,
    long EmployeeNo,
    long EmployeeSysId,
    string Status,
    DateTime EnrollmentDate,
    DateTime? ClosureDate,
    string? LeaveReason
);

public record MemberSummaryDto(
    long MemberNo,
    string MemberName,
    string TrustCode,
    string Status,
    DateTime DateOfJoining,
    string UnitCode,
    int NomineeCount
);

public record NomineeDto(
    int SerialNo,
    string FundType,
    string NomineeName,
    string RelationshipCode,
    long Percentage,
    DateTime DateOfBirth,
    bool IsMinor,
    string Status
);

public record ContactDto(
    long ContactId,
    string ContactType,
    string AddressLine1,
    string? AddressLine2,
    string? AddressLine3,
    string City,
    string State,
    string PinCode,
    string Country,
    string? PhoneNo,
    string? Email,
    DateTime EffectiveDate
);

public record MemberProfileDto(
    MemberDto Member,
    IReadOnlyList<NomineeDto> Nominees,
    IReadOnlyList<ContactDto> Contacts
);
