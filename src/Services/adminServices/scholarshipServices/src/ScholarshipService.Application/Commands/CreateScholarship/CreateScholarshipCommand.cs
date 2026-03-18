using MediatR;

namespace ScholarshipService.Application.Commands.CreateScholarship;

public record CreateScholarshipCommand(
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
    string Source,
    decimal DisbursementAmount,
    string DisbursementFrequency,
    int CreatedBy,
    string IsOffline = "N",
    int? OfflineYear = null
) : IRequest<int>;
