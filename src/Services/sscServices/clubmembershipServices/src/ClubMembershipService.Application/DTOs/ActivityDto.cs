namespace ClubMembershipService.Application.DTOs;

public record ActivityDto(
    long ActivityId,
    long ClubId,
    string? ClubName,
    string ActivityName,
    DateOnly ActivityDate,
    decimal? ActivityBudget,
    long OrganizerId,
    string Status,
    long CreatedBy,
    DateTime CreatedOn,
    long? ModifiedBy,
    DateTime? ModifiedOn
);
