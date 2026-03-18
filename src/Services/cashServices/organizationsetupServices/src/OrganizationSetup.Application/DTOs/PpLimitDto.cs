namespace OrganizationSetup.Application.DTOs;

public class PpLimitDto
{
    public long PpLimitId { get; set; }
    public long PpOrgId { get; set; }
    public string PpTranType { get; set; } = string.Empty;
    public long PpBasCurr { get; set; }
    public decimal? PpLimitAmt { get; set; }
    public int PpFinYear { get; set; }
    public decimal? PpLimitAct { get; set; }
    public string? PpCertificateUpload { get; set; }
    public decimal? PpModifiedBy { get; set; }
    public DateTime? PpModifiedOn { get; set; }
}
