using HealthTransaction.Domain.Common;

namespace HealthTransaction.Domain.Entities;

/// <summary>Maps CHKUP_PFI_HIST — Personal/Family/Immunization history records</summary>
public class PfiHistory : BaseEntity
{
    public decimal HlthNum { get; set; }     // CPH_HLTH_NUM
    public decimal EmpNum { get; set; }      // CPH_EMP_NUM
    public decimal SympId { get; set; }      // CPH_SYMP_ID
    public char? YnFlag { get; set; }        // CPH_YN_FLAG CHAR(1)
    public DateTime? ImmDate { get; set; }   // CPH_IMM_DAT
    public string? TestValue { get; set; }   // CPH_TEST_VAL
}
