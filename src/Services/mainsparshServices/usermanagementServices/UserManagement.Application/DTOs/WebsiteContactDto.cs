namespace UserManagement.Application.DTOs;

public record WebsiteContactDto(
    long ContactId,
    long UserSysId,
    string PrimaryEmail,
    string? SecondaryEmail,
    string? Phone,
    string? Mobile,
    string? Website,
    string? SocialMedia,
    string NewsletterOptIn,
    string ContactStatus,
    long CreatedBy,
    DateTime CreatedOn,
    long? UpdatedBy,
    DateTime? UpdatedOn);

public record CreateWebsiteContactDto(
    long UserSysId,
    string PrimaryEmail,
    long CreatedBy,
    string? SecondaryEmail = null,
    string? Phone = null,
    string? Mobile = null,
    string? Website = null,
    string? SocialMedia = null,
    bool NewsletterOptIn = true);

public record UpdateWebsiteContactDto(
    string? SecondaryEmail,
    string? Phone,
    string? Mobile,
    string? Website,
    string? SocialMedia,
    bool NewsletterOptIn,
    long UpdatedBy);
