namespace ScholarshipService.Application.DTOs;

public record ScholarshipMainDto(
    int Id,
    int EmployeeSysId,
    int GradeId,
    int DependentId,
    string ChildName,
    string LastSchool,
    decimal LastYearOfSchool,
    string LastExam,
    string CgpaFlag,
    decimal MarksPercentage,
    decimal MarksGpa,
    string MarksFile,
    string CourseName,
    int CourseJoinYear,
    decimal CourseJoinMonth,
    long CourseDuration,
    string? AdmissionReceiptFile,
    string? PaymentMode,
    string? ChildAccountNumber,
    string? ChildBankIfsc,
    string? ChildBankMicr,
    string? EntryStatus,
    string Source,
    decimal DisbursementAmount,
    string DisbursementFrequency,
    string LiveStatus,
    DateTime CreatedOn,
    long CreatedBy,
    DateTime? UpdatedOn,
    long? UpdatedBy,
    string IsOffline,
    int? OfflineYear,
    IEnumerable<ScholarshipDetailDto> Details
);

public record ScholarshipDetailDto(
    long Id,
    int MainId,
    int Year,
    string MarksFile,
    string MarksStatus,
    string PayStatus,
    DateTime CreatedOn,
    long CreatedBy,
    DateTime? ApprovedOn,
    long? ApprovedBy,
    DateTime? PayDate,
    long? PayAmount
);

public record ScholarshipAmountDto(
    long Id,
    long OrgId,
    string GradeCategory,
    string EligibleExam,
    string ApplicableAllGrade,
    decimal GradeId,
    decimal FromYear,
    decimal? CloseYear,
    long EligibleAmount,
    int EligibleYear,
    int CutoffMarks
);
