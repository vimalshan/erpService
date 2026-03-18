namespace EligibilityService.Domain.Entities;

/// <summary>Maps to CAN_SHIFT_MAPPING table.</summary>
public class ShiftMapping
{
    public long CompanyCode { get; private set; }             // CN_COM_COD
    public string ShiftCode { get; private set; } = default!;  // CN_SFT_COD
    public string BeforeShiftCode { get; private set; } = default!; // CN_SFT_BEF
    public string AfterShiftCode { get; private set; } = default!;  // CN_SFT_AFT

    private ShiftMapping() { }

    public static ShiftMapping Create(long companyCode, string shiftCode, string beforeShiftCode, string afterShiftCode)
    {
        return new ShiftMapping
        {
            CompanyCode = companyCode,
            ShiftCode = shiftCode,
            BeforeShiftCode = beforeShiftCode,
            AfterShiftCode = afterShiftCode
        };
    }
}
