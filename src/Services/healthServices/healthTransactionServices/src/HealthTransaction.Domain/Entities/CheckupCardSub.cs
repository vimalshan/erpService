using HealthTransaction.Domain.Common;

namespace HealthTransaction.Domain.Entities;

/// <summary>Maps HLTH_CHKCARD_SUB — Health checkup card sub-records</summary>
public class CheckupCardSub : BaseEntity
{
    public decimal HlthNum { get; set; }     // HCS_HLTH_NUM (composite PK part 1)
    public decimal SympId { get; set; }      // HCS_SYMP_ID  (composite PK part 2)
    public char? FlagYn { get; set; }        // HCS_FLAG_YN CHAR(1)
    public string? SympVal { get; set; }     // HCS_SYMP_VAL
    public decimal EmpNum { get; set; }      // HCS_EMP_NUM

    public CheckupCard? CheckupCard { get; set; }
}
