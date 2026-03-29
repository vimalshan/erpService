using HealthTransaction.Domain.Common;

namespace HealthTransaction.Domain.Entities;

/// <summary>Maps CHKUP_PRE_MAIN — Pre-employment health checkup records</summary>
public class PreEmploymentCheckup : BaseEntity
{
    public decimal EmpNum { get; set; }          // CPM_EMP_NUM
    public string ComCode { get; set; } = string.Empty;  // CPM_COM_COD VARCHAR(3)
    public decimal HlthNum { get; set; }         // CPM_HLTH_NUM
    public string? PhysHandicap { get; set; }    // CPM_PHYS_HAND
    public string? ProposedEmp { get; set; }     // CPM_PROP_EMP
    public string? IdentMarks { get; set; }      // CPM_IDENT_MARKS
    public string? FinalRemarks { get; set; }    // CPM_FINAL_RMKS
    public char? FitPh { get; set; }             // CPM_FIT_PH CHAR(3) — first char
    public string? FitFinal { get; set; }        // CPM_FIT_FINAL
    public DateTime? CheckupDate { get; set; }   // CPM_CHK_DAT
}
