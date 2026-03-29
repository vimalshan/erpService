using HealthTransaction.Domain.Common;

namespace HealthTransaction.Domain.Entities;

/// <summary>Maps HEALTH_DYN_DET — Dynamic health details (flexible form data)</summary>
public class DynamicHealthDetail : BaseEntity
{
    public decimal HlthNum { get; set; }       // CDD_HLTH_NUM  (composite PK part 1)
    public string ChkupCod { get; set; } = string.Empty; // CDD_CHKUP_COD (composite PK part 2)
    public string ComCode { get; set; } = string.Empty;  // CDD_COM_COD   (composite PK part 3)
    public decimal CtrlSrcId { get; set; }     // CDD_CTRLSRC_ID (composite PK part 4)
    public string? DynVal { get; set; }        // CDD_DYN_VAL
    public decimal EmpNum { get; set; }        // CDD_EMP_NUM
    public DateTime? SysDate { get; set; }     // CDD_SYS_DAT
}
