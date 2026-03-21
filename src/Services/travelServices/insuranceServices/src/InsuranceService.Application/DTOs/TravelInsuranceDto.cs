namespace InsuranceService.Application.DTOs;

public class TravelInsuranceDto
{
    public string CompanyCode { get; set; } = string.Empty;
    public long PlanNumber { get; set; }
    public string InsuranceType { get; set; } = string.Empty;
    public string? PassportNumber { get; set; }
    public DateTime? PassportIssueDate { get; set; }
    public string? VisaIssuePlace { get; set; }
    public DateTime? VisaIssueDate { get; set; }
    public string? NomineeName1 { get; set; }
    public string? NomineeName2 { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? CertificateNumber { get; set; }
    public DateTime? UpdateDate { get; set; }
    public string? Remarks { get; set; }
}
