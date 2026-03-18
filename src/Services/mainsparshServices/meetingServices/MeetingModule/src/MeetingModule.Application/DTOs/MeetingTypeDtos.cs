namespace MeetingModule.Application.DTOs;

public record MeetingTypeDto(
    long MeetTypeId,
    string MeetTypeCode,
    string MeetTypeName,
    string? MeetTypeDesc,
    string MeetTypeStatus,
    long CreatedBy,
    DateTime CreatedOn);

public record CreateMeetingTypeDto(
    string MeetTypeCode,
    string MeetTypeName,
    string? MeetTypeDesc);

public record UpdateMeetingTypeDto(
    string MeetTypeName,
    string? MeetTypeDesc);
