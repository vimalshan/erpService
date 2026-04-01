namespace LetTransactionService.Application.DTOs;

public record ReviewMainDto(
    long ReviewSerialNumber,
    long FeedbackNumber,
    string? ImplementationGoal,
    string? KeyLearning,
    string? KeyStepsImplementation,
    string? KeyOutputsExpected,
    string? MeasurementProcess,
    string? HelpRequiredFromHr,
    string? EntryDate,
    string Status,
    DateTime? NextReviewDate,
    IEnumerable<ReviewSubDto>? ReviewDetails);

public record ReviewSubDto(
    long ReviewMainSerial,
    long ReviewNumber,
    string NextRequired,
    DateTime? ReviewDate,
    long ReviewBy,
    string? Remarks,
    string? ReviewStatus,
    string? ProgressRemarks);

public record ReviewSummaryDto(
    long ReviewSerialNumber,
    long FeedbackNumber,
    string Status,
    DateTime? NextReviewDate,
    int DetailCount);

public record PendingReviewDto(
    long ReviewSerialNumber,
    long FeedbackNumber,
    string? ImplementationGoal,
    DateTime? NextReviewDate);
