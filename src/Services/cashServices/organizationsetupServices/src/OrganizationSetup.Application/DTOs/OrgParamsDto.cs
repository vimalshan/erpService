namespace OrganizationSetup.Application.DTOs;

public class OrgParamsDto
{
    public long OrgParamId { get; set; }
    public string OrgParamType { get; set; } = string.Empty;
    public long OrgParamValue { get; set; }
    public long OrgId { get; set; }
    public decimal OrgModifiedBy { get; set; }
    public DateTime OrgModifiedOn { get; set; }
}
