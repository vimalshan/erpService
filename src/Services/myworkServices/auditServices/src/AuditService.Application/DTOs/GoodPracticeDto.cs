namespace AuditService.Application.DTOs;

public record GoodPracticeDto(
    long PracticeId,
    string PracticeTitle,
    string PracticeDescription,
    string PracticeBenefits,
    string PracticeRemarks,
    long PracticeProcess,
    long PracticeEmpSysId,
    long PracticeUnit,
    DateTime PracticeLastModifiedOn,
    double AverageRating,
    int RatingCount,
    string? PracticeAttachment1,
    string? PracticeAttachment2
);

public record CreateGoodPracticeRequest(
    long PracticeId,
    string PracticeTitle,
    string PracticeDescription,
    string PracticeBenefits,
    string PracticeRemarks,
    long PracticeProcess,
    long PracticeEmpSysId,
    long PracticeUnit,
    long CreatedBy
);

public record RateGoodPracticeRequest(
    long RatingId,
    long RatedBy,
    int Rating
);

public record AuthRequest(string Username, string Password);
public record AuthResponse(string Token, DateTime ExpiresAt, string Username, string Role);
