namespace TrainingDevelopment.Application.DTOs;

public record InstituteMasterDto(
    decimal InstituteCode,
    string? InstituteName,
    string? Address1,
    string? Address2,
    string? City,
    string? State,
    string? Pin,
    string? Phone,
    string? Fax,
    string? Email,
    string? Url,
    string? InstituteType,
    string CampusRecruit,
    string? InstituteClass,
    decimal? LastModifiedBy,
    DateTime? LastModifiedOn
);
