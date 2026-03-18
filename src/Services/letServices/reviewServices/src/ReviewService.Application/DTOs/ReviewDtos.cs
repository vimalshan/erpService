namespace ReviewService.Application.DTOs;

public record ReviewMainDto(
    long RevSrlNum,
    long? RevFedNum,
    string? Remarks1,
    string? Remarks2,
    string? Remarks3,
    string? Remarks4,
    string? Remarks5,
    string? EntryDate,
    string? Status,
    DateTime? NextDate,
    IEnumerable<ReviewSubDto>? SubRecords = null);

public record ReviewSubDto(
    long? MainSrl,
    long? ReviewNum,
    DateTime? ReviewDate,
    long? ReviewedBy,
    string? Status,
    string? Remarks,
    string? ProgressRemarks);

public record CourseFeedbackDto(
    long CourseId,
    string UserId,
    DateTime ReviewDate,
    string GeneralRemarks,
    long RequestNum,
    DateTime ModifiedDate,
    IEnumerable<CourseFeedSubDto>? SubItems = null);

public record CourseFeedSubDto(
    long RequestNum,
    long RequestSrl,
    long SrlNum,
    long TypeCode,
    long? TypeNum,
    string? TypeDescription);

public record FeedbackSummaryDto(
    long CourseId,
    int TotalFeedbacks,
    decimal AverageRating);

public record ReviewSkillDto(
    long ReqId,
    long SrlNum,
    long SkillCode,
    long LevelNum,
    decimal RatingPercent,
    string Remarks);

public record FeedMastDto(
    long TypeCode,
    string TypeName,
    char NumType,
    string? EvalCode);

public record TrainerFeedDto(
    long GroupCode,
    long FedNum,
    string? QuestionGroup,
    long? WeightNum,
    DateTime? EffectiveDate,
    DateTime? ClosingDate);
