using CompensationBenefits.Domain.Common;

namespace CompensationBenefits.Domain.Entities;

/// <summary>Maps to SALARY_DET table</summary>
public class SalaryDetail : BaseEntity
{
    public long SalDetId { get; private set; }
    public long SalDetSalaryId { get; private set; }
    public decimal SalDetSrl { get; private set; }
    public string? SalDetAnnGroup { get; private set; }
    public long SalDetEdId { get; private set; }
    public string SalDetCategory { get; private set; } = default!; // E=Earnings, R=Retirals, O=Other
    public string SalDetEdName { get; private set; } = default!;
    public decimal SalDetEdAmt { get; private set; }
    public string SalDetFrequency { get; private set; } = default!;
    public string? SalDetSuperChar { get; private set; }
    public string? SalDetSuperDesc { get; private set; }
    public string? SalDetYearType { get; private set; }
    public long? SalDetGlobalUnitId { get; private set; }
    public string SalDetFormula { get; private set; } = "N";
    public string SalDetShowMonthly { get; private set; } = "Y";
    public string SalDetAnnexOnly { get; private set; } = "N";

    // Navigation
    public SalaryMain SalaryMain { get; private set; } = default!;

    private SalaryDetail() { }

    public static SalaryDetail Create(long id, long salaryId, long edId, string category,
        string edName, decimal edAmt, string frequency, decimal srl)
    {
        return new SalaryDetail
        {
            SalDetId = id,
            SalDetSalaryId = salaryId,
            SalDetEdId = edId,
            SalDetCategory = category,
            SalDetEdName = edName,
            SalDetEdAmt = edAmt,
            SalDetFrequency = frequency,
            SalDetSrl = srl
        };
    }
}
