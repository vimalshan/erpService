namespace MeetingModule.Application.DTOs;

public record MeetingScheduleDto(
    long MeetingId,
    long MeetTypeId,
    string? MeetTypeName,
    string MeetingTitle,
    DateTime MeetingDate,
    string? MeetingLocation,
    int? MeetingDuration,
    long OrganizerId,
    string MeetingStatus,
    string? Notes,
    long CreatedBy,
    DateTime CreatedOn,
    List<PollDetailDto>? Polls);

public record CreateMeetingScheduleDto(
    long MeetTypeId,
    string MeetingTitle,
    DateTime MeetingDate,
    string? MeetingLocation,
    int? MeetingDuration,
    long OrganizerId,
    string? Notes);

public record UpdateMeetingScheduleDto(
    string MeetingTitle,
    DateTime MeetingDate,
    string? MeetingLocation,
    int? MeetingDuration,
    string? Notes);
