using CompensationBenefits.Domain.Common;

namespace CompensationBenefits.Domain.Entities;

/// <summary>Maps to EMP_RETIRALS_EMPSPECIFIC table</summary>
public class EmployeeRetiralEmpSpecific : BaseEntity
{
    public long EmpRetId { get; private set; }
    public long EmpRetEmpSysId { get; private set; }
    public string EmpRetPayType { get; private set; } = default!;
    public long EmpRetEdId { get; private set; }
    public DateTime EmpRetEffDate { get; private set; }
    public DateTime? EmpRetClsDate { get; private set; }
    public long EmpRetPercentage { get; private set; }
    public long EmpRetCreatedBy { get; private set; }
    public DateTime EmpRetCreatedOn { get; private set; }
    public long? EmpRetModifiedBy { get; private set; }
    public DateTime? EmpRetModifiedOn { get; private set; }

    private EmployeeRetiralEmpSpecific() { }
}

/// <summary>Maps to EMP_RETIRALSDET table</summary>
public class EmployeeRetiralDetail : BaseEntity
{
    public long ErDetId { get; private set; }
    public long ErDetEmpSysId { get; private set; }
    public DateTime ErDetPfClsDate { get; private set; }
    public string ErDetRemarks { get; private set; } = default!;
    public long ErDetModifiedBy { get; private set; }
    public DateTime ErDetModifiedOn { get; private set; }

    private EmployeeRetiralDetail() { }
}

/// <summary>Maps to RETRIALS_RANGEMAST table</summary>
public class RetiralRangeMaster : BaseEntity
{
    public long RrMastId { get; private set; }
    public long RrMastUnitId { get; private set; }
    public decimal RrMastFromYear { get; private set; }
    public decimal RrMastToYear { get; private set; }
    public decimal RrMastPercentage { get; private set; }
    public long RrMastModifiedBy { get; private set; }
    public DateTime RrMastModifiedOn { get; private set; }

    private RetiralRangeMaster() { }

    public static RetiralRangeMaster Create(long id, long unitId, decimal fromYear, decimal toYear,
        decimal percentage, long modifiedBy)
    {
        return new RetiralRangeMaster
        {
            RrMastId = id,
            RrMastUnitId = unitId,
            RrMastFromYear = fromYear,
            RrMastToYear = toYear,
            RrMastPercentage = percentage,
            RrMastModifiedBy = modifiedBy,
            RrMastModifiedOn = DateTime.UtcNow
        };
    }
}
