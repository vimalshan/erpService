using EmployeeTransactionsService.Domain.Entities;

namespace EmployeeTransactionsService.Application.DTOs;

public sealed record LoginResultDto(string AccessToken, DateTime ExpiresAtUtc);

public sealed record EmployeeTransactionDto(
    decimal EmployeeId,
    string FullName,
    decimal PinNo,
    string AppUnit,
    decimal AppGrade,
    decimal AppPosition,
    string PositionDescription,
    string Gender,
    DateTime Dob,
    string OfferStatus,
    string? OfficialEmail,
    string? PersonalEmail,
    string? MobileNo,
    string LeadRole,
    DateTime? ProbationDueDate,
    string? ProbationFlag,
    DateTime? ConfirmationDate,
    decimal? CurrentGradeId,
    string? CurrentGradeLiveFlag,
    string? ProbationFinalStatus);

public sealed record EmployeeGradeChangeDto(
    decimal GradeChangeId,
    decimal EmployeeId,
    decimal OldGradeId,
    decimal NewGradeId,
    DateTime EffectiveDate,
    string Status,
    DateTime CreatedOnUtc);

public sealed record ProbationReviewDto(
    decimal ProbationId,
    decimal EmployeeId,
    DateTime? DueDate,
    string? FinalStatus,
    DateTime? ReviewDate,
    DateTime? NextReviewDate,
    DateTime? ConfirmationDate);

public sealed record AlertGroupRecipientDto(
    decimal MappingId,
    decimal GroupId,
    decimal EmployeeId,
    string? EmailId,
    decimal OrgId,
    decimal UnitId,
    decimal? CalendarId,
    DateTime EffectiveDate,
    DateTime? CloseDate);

public sealed record AlertGroupDto(
    decimal AlertGroupId,
    string Name,
    string Type,
    decimal CreatedBy,
    DateTime CreatedOnUtc,
    IReadOnlyList<AlertGroupRecipientDto> Recipients);

public sealed record StationeryImageDto(Guid ImageId, string ItemReference, string BlobName, string ContentType, DateTime UploadedOnUtc);

public sealed record TransactionTimelineItemDto(string TransactionType, decimal ReferenceId, DateTime ActivityOnUtc, string Description);

public static class TransactionMappings
{
    public static EmployeeTransactionDto ToDto(this EmployeeMain employee, EmployeeGrade? currentGrade, EmployeeProbation? probation) =>
        new(
            employee.EmpSysId,
            string.Join(" ", new[] { employee.EmpFrsName, employee.EmpMidName, employee.EmpLstName }.Where(static x => !string.IsNullOrWhiteSpace(x))),
            employee.EmpPinNo,
            employee.EmpAppUnit,
            employee.EmpAppGrade,
            employee.EmpAppPosition,
            employee.EmpAppPositionDesc,
            employee.EmpGender,
            employee.EmpDobRecord,
            employee.EmpOfferStatus,
            employee.EmpOEmailId,
            employee.EmpPEmailId,
            employee.EmpMobileNo,
            employee.EmpLeadRole,
            employee.EmpProbDate,
            employee.EmpProbFlag,
            employee.EmpConfDate,
            currentGrade?.GradeId,
            currentGrade?.GradeLivFlag,
            probation?.ProbFinStatus);

    public static EmployeeGradeChangeDto ToDto(this EmployeeGradeChange change) =>
        new(change.EmpGradeChangeId, change.EmpEmpSysId, change.EmpOldGrade, change.EmpNewGrade, change.EmpEffDate, change.EmpStatus, change.EmpCreatedOn);

    public static ProbationReviewDto ToDto(this EmployeeProbation probation) =>
        new(probation.ProbId, probation.ProbEmpSysId, probation.ProbDueDate, probation.ProbFinStatus, probation.ProbReviewDate, probation.ProbNxtReviewDate, probation.ProbConfDate);

    public static AlertGroupDto ToDto(this AlertGroup group) =>
        new(
            group.AlgrpId,
            group.AlgrpName,
            group.AlgrpType,
            group.AlgrpCreatedBy,
            group.AlgrpCreatedOn,
            group.Members.Select(member => new AlertGroupRecipientDto(
                member.AlmapId,
                member.AlmapGrpid,
                member.AlmapEmpSysId,
                member.AlmapEmailId,
                member.AlmapOrgId,
                member.AlmapUnitId,
                member.AlmapCalendarId,
                member.AlmapEffDate,
                member.AlmapClsDate)).ToList());

    public static StationeryImageDto ToDto(this StationeryItemImage image) =>
        new(image.ImageId, image.ItemReference, image.BlobName, image.ContentType, image.UploadedOnUtc);
}