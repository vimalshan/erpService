namespace LetTransactionService.Application.DTOs;

public record LetMainDto(
    long RequestNumber,
    int FinancialYearSerialNo,
    string EmployeeUserId,
    string? SupervisorUserId,
    DateTime? RequestDate,
    IEnumerable<LetSubDto>? SubEntries);

public record LetSubDto(
    long RequestNumber,
    int SerialNumber,
    DateTime? ModifiedDate,
    string? ModifiedUser,
    string PreferredModeDev,
    string? ActionTaken,
    int? CourseId,
    string? TrainingProgramBhr,
    string? ImpactBenefitProcess,
    string? MeasureCompetency,
    string? MidYearReviewerName,
    string? MidYearReviewerDate,
    string? MidYearReviewerRemark,
    string? AnnualReviewerName,
    string? AnnualReviewerDate,
    string? AnnualReviewerRemark,
    int? CompetencyToDevelop,
    string? DomainKnowledgeDev,
    string? DomainKnowledgeDevDetail,
    string? ProcessDev,
    string? ProcessDevDetail,
    string LetSubCode,
    string? ReviewType);

public record LetSummaryDto(
    long RequestNumber,
    string EmployeeUserId,
    string? SupervisorUserId,
    DateTime? RequestDate,
    int SubEntryCount);
