namespace MeetingModule.Application.DTOs;

public record PollDetailDto(
    long PollId,
    long MeetingId,
    string PollQuestion,
    string? PollType,
    string PollStatus,
    long CreatedBy,
    DateTime? CreatedOn);

public record CreatePollDetailDto(
    long MeetingId,
    string PollQuestion,
    string? PollType);

public record UpdatePollDetailDto(
    string PollQuestion,
    string? PollType);
