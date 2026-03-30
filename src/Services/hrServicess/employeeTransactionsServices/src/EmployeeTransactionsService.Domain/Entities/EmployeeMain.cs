using EmployeeTransactionsService.Domain.Common;
using EmployeeTransactionsService.Domain.Events;
using EmployeeTransactionsService.Domain.ValueObjects;

namespace EmployeeTransactionsService.Domain.Entities;

public sealed class EmployeeMain : BaseEntity
{
    private EmployeeMain()
    {
    }

    public decimal EmpSysId { get; private set; }
    public decimal EmpPinNo { get; private set; }
    public DateTime EmpAppDate { get; private set; }
    public string EmpAppUnit { get; private set; } = string.Empty;
    public decimal EmpAppGrade { get; private set; }
    public decimal EmpAppPosition { get; private set; }
    public string EmpAppPositionDesc { get; private set; } = string.Empty;
    public string EmpFrsName { get; private set; } = string.Empty;
    public string? EmpMidName { get; private set; }
    public string? EmpLstName { get; private set; }
    public string EmpGender { get; private set; } = "U";
    public DateTime EmpDobRecord { get; private set; }
    public string EmpOfferStatus { get; private set; } = "I";
    public string? EmpOEmailId { get; private set; }
    public string? EmpPEmailId { get; private set; }
    public string? EmpMobileNo { get; private set; }
    public string EmpLeadRole { get; private set; } = "GEN";
    public DateTime? EmpProbDate { get; private set; }
    public string? EmpProbFlag { get; private set; }
    public DateTime? EmpConfDate { get; private set; }
    public decimal EmpAppUnitId { get; private set; }
    public decimal? EmpCreatedBy { get; private set; }
    public DateTime? EmpCreatedOn { get; private set; }
    public decimal? EmpUpdatedBy { get; private set; }
    public DateTime? EmpUpdatedOn { get; private set; }

    public static EmployeeMain Create(
        decimal employeeId,
        decimal pinNo,
        DateTime appDate,
        string appUnit,
        decimal appGrade,
        decimal appPosition,
        string appPositionDesc,
        EmployeeName name,
        string gender,
        DateTime dob,
        string offerStatus,
        EmailAddress? officialEmail,
        EmailAddress? personalEmail,
        string? mobileNo,
        string leadRole,
        DateTime? probationDueDate,
        decimal appUnitId,
        decimal createdBy)
    {
        var entity = new EmployeeMain
        {
            EmpSysId = employeeId,
            EmpPinNo = pinNo,
            EmpAppDate = appDate,
            EmpAppUnit = appUnit,
            EmpAppGrade = appGrade,
            EmpAppPosition = appPosition,
            EmpAppPositionDesc = appPositionDesc,
            EmpFrsName = name.FirstName,
            EmpMidName = name.MiddleName,
            EmpLstName = name.LastName,
            EmpGender = string.IsNullOrWhiteSpace(gender) ? "U" : gender[..1].ToUpperInvariant(),
            EmpDobRecord = dob,
            EmpOfferStatus = offerStatus,
            EmpOEmailId = officialEmail?.Value,
            EmpPEmailId = personalEmail?.Value,
            EmpMobileNo = mobileNo,
            EmpLeadRole = leadRole,
            EmpProbDate = probationDueDate,
            EmpProbFlag = probationDueDate.HasValue ? "Y" : "N",
            EmpAppUnitId = appUnitId,
            EmpCreatedBy = createdBy,
            EmpCreatedOn = DateTime.UtcNow,
            EmpUpdatedBy = createdBy,
            EmpUpdatedOn = DateTime.UtcNow
        };

        entity.AddDomainEvent(new EmployeeCreatedDomainEvent(entity.EmpSysId, name.FullName));
        return entity;
    }

    public void ApplyProbationReview(string finalStatus, DateTime? confirmationDate, DateTime? nextReviewDate, decimal reviewedBy)
    {
        EmpProbFlag = finalStatus == "B" ? "Y" : "N";
        EmpProbDate = nextReviewDate;
        EmpConfDate = finalStatus == "A" ? (confirmationDate ?? DateTime.UtcNow) : EmpConfDate;
        EmpUpdatedBy = reviewedBy;
        EmpUpdatedOn = DateTime.UtcNow;
    }
}