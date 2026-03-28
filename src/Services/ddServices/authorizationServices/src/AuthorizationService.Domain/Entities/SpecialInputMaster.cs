namespace AuthorizationService.Domain.Entities;

/// <summary>
/// SpecialInputMaster Entity - Maps to DD_SPECIALINPUT_MASTER table
/// </summary>
public class SpecialInputMaster : BaseEntity
{
    public decimal SpecialInputId { get; set; }
    public decimal YearId { get; set; }
    public string RoleType { get; set; } = string.Empty;
    public decimal EmployeeSysId { get; set; }
    public decimal AppraisalSysId { get; set; }
    public decimal CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }

    public SpecialInputMaster() { }

    public SpecialInputMaster(decimal specialInputId, decimal yearId, string roleType, decimal employeeSysId, decimal appraisalSysId)
    {
        SpecialInputId = specialInputId;
        YearId = yearId;
        RoleType = roleType;
        EmployeeSysId = employeeSysId;
        AppraisalSysId = appraisalSysId;
    }
}
