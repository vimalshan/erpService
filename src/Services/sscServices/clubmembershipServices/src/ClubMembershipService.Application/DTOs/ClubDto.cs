namespace ClubMembershipService.Application.DTOs;

public record ClubDto(
    long ClubId,
    string ClubName,
    string Status,
    long CreatedBy,
    DateTime CreatedOn,
    long? ModifiedBy,
    DateTime? ModifiedOn
);
