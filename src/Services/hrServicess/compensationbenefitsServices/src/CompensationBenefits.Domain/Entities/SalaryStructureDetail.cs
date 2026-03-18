using CompensationBenefits.Domain.Common;

namespace CompensationBenefits.Domain.Entities;

/// <summary>Maps to SALSTRUCTURE_DET table</summary>
public class SalaryStructureDetail : BaseEntity
{
    public long StructDetId { get; private set; }
    public long StructDetStructureId { get; private set; }
    public long StructDetEdId { get; private set; }
    public string StructDetAmtType { get; private set; } = default!;
    public long StructDetCalType { get; private set; }
    public string StructDetCategory { get; private set; } = default!;
    public string StructDetFrequency { get; private set; } = default!;
    public decimal StructDetEdAmt { get; private set; }
    public decimal StructDetMinValue { get; private set; }
    public decimal? StructDetMaxValue { get; private set; }
    public long? StructDetGlobalUnitId { get; private set; }
    public string? StructDetSuperChar { get; private set; }
    public string? StructDetSuperDesc { get; private set; }
    public string StructDetModify { get; private set; } = "N";
    public string StructDetFormula { get; private set; } = "N";
    public long StructDetCreatedBy { get; private set; }
    public DateTime StructDetCreatedOn { get; private set; }
    public long StructDetLastModifiedBy { get; private set; }
    public DateTime StructDetLastModifiedOn { get; private set; }
    public string StructureShowMonthly { get; private set; } = "Y";
    public string StructureAnnexOnly { get; private set; } = "N";

    public SalaryStructureMain StructureMain { get; private set; } = default!;

    private SalaryStructureDetail() { }

    public static SalaryStructureDetail Create(long id, long structureId, long edId, string amtType,
        long calType, string category, string frequency, decimal edAmt, decimal minValue, long createdBy)
    {
        return new SalaryStructureDetail
        {
            StructDetId = id,
            StructDetStructureId = structureId,
            StructDetEdId = edId,
            StructDetAmtType = amtType,
            StructDetCalType = calType,
            StructDetCategory = category,
            StructDetFrequency = frequency,
            StructDetEdAmt = edAmt,
            StructDetMinValue = minValue,
            StructDetCreatedBy = createdBy,
            StructDetCreatedOn = DateTime.UtcNow,
            StructDetLastModifiedBy = createdBy,
            StructDetLastModifiedOn = DateTime.UtcNow
        };
    }
}
