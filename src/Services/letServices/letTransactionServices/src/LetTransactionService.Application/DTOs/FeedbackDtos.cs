namespace LetTransactionService.Application.DTOs;

public record FeedbackMainDto(
    long FeedbackNumber,
    long NominationNumber,
    string StatusCode,
    DateTime? FeedbackDate,
    DateTime? ModifiedDate,
    long? OverallRating,
    string? Remarks1,
    string? Remarks2,
    string? Remarks3,
    decimal? FeedbackReviewSerial,
    string? CancelRemark,
    long? RequestNumber,
    long? TotalManHours,
    IEnumerable<FeedbackSubDto>? FeedbackDetails);

public record FeedbackSubDto(
    long FeedbackNumber,
    long FeedbackType,
    long Rating,
    string? Remarks);

public record FeedbackSummaryDto(
    long FeedbackNumber,
    long NominationNumber,
    string StatusCode,
    DateTime? FeedbackDate,
    long? OverallRating,
    int DetailCount);
