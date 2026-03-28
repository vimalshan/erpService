namespace AuthorizationService.Domain.Entities;

/// <summary>
/// SpecialInput Entity - Maps to DD_SPECIALINPUTS table
/// </summary>
public class SpecialInput : BaseEntity
{
    public decimal SpecialInputId { get; set; }
    public decimal YearId { get; set; }
    public string RoleType { get; set; } = string.Empty;
    public decimal EmployeeSysId { get; set; }
    public decimal AppraisalSysId { get; set; }
    public string Inputs { get; set; } = string.Empty;
    public char Status { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? SubmittedOn { get; set; }

    public SpecialInput() { }

    public SpecialInput(decimal specialInputId, decimal yearId, string roleType, decimal employeeSysId, decimal appraisalSysId)
    {
        SpecialInputId = specialInputId;
        YearId = yearId;
        RoleType = roleType;
        EmployeeSysId = employeeSysId;
        AppraisalSysId = appraisalSysId;
    }

    public bool IsSubmitted => SubmittedOn.HasValue;
}
