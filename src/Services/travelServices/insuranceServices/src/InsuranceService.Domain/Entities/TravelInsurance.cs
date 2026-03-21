using InsuranceService.Domain.Common;
using InsuranceService.Domain.Events;
using InsuranceService.Domain.ValueObjects;

namespace InsuranceService.Domain.Entities;

public class TravelInsurance : AggregateRoot
{
    public CompanyCode CompanyCode { get; private set; } = null!;
    public long PlanNumber { get; private set; }
    public InsuranceType InsuranceType { get; private set; } = null!;
    public string? PassportNumber { get; private set; }
    public DateTime? PassportIssueDate { get; private set; }
    public string? VisaIssuePlace { get; private set; }
    public DateTime? VisaIssueDate { get; private set; }
    public string? NomineeName1 { get; private set; }
    public string? NomineeName2 { get; private set; }
    public InsuranceStatus Status { get; private set; } = null!;
    public string? CertificateNumber { get; private set; }
    public DateTime? UpdateDate { get; private set; }
    public string? UpdatedByUserId { get; private set; }
    public long? UpdatedByUserNumber { get; private set; }
    public string? Remarks { get; private set; }
    public string? FlexField1 { get; private set; }
    public decimal? FlexField2 { get; private set; }
    public decimal? FlexField3 { get; private set; }
    public DateTime? FlexField4 { get; private set; }

    private TravelInsurance() { } // EF constructor

    public static TravelInsurance Register(
        string companyCode,
        long planNumber,
        string insuranceType,
        string? passportNumber,
        string? visaPlace,
        string? nominee1,
        string? nominee2,
        string? remarks)
    {
        var insurance = new TravelInsurance
        {
            CompanyCode = new CompanyCode(companyCode),
            PlanNumber = planNumber,
            InsuranceType = new InsuranceType(insuranceType),
            PassportNumber = passportNumber,
            VisaIssuePlace = visaPlace,
            NomineeName1 = nominee1,
            NomineeName2 = nominee2,
            Status = InsuranceStatus.Active,
            UpdateDate = DateTime.UtcNow,
            Remarks = remarks
        };

        insurance.AddDomainEvent(new InsuranceRegisteredEvent(
            companyCode, planNumber, insuranceType));

        return insurance;
    }

    public void UpdateStatus(string status, string? certificateNumber, long? updatedBy)
    {
        var oldStatus = Status.Value;
        Status = new InsuranceStatus(status);
        CertificateNumber = certificateNumber ?? CertificateNumber;
        UpdatedByUserNumber = updatedBy;
        UpdateDate = DateTime.UtcNow;

        AddDomainEvent(new InsuranceStatusChangedEvent(
            CompanyCode.Value, PlanNumber, oldStatus, status));
    }

    public void UpdatePassportDetails(string? passportNumber, DateTime? issueDate)
    {
        PassportNumber = passportNumber;
        PassportIssueDate = issueDate;
        UpdateDate = DateTime.UtcNow;
    }

    public void UpdateVisaDetails(string? visaPlace, DateTime? visaDate)
    {
        VisaIssuePlace = visaPlace;
        VisaIssueDate = visaDate;
        UpdateDate = DateTime.UtcNow;
    }
}
