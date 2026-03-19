namespace ClubMembershipService.Application.DTOs;

public record MembershipDto(
    long MembershipId,
    long ClubId,
    string? ClubName,
    long MemberId,
    DateOnly JoinDate,
    decimal? MembershipFee,
    string Status,
    long CreatedBy,
    DateTime CreatedOn,
    long? ModifiedBy,
    DateTime? ModifiedOn
);
