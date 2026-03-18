using CompensationBenefits.Domain.Common;

namespace CompensationBenefits.Domain.Entities;

/// <summary>Maps to BASIC_SLABINC table</summary>
public class BasicSlabIncrement : BaseEntity
{
    public long SlabIncId { get; private set; }
    public long SlabGradeId { get; private set; }
    public long SlabUnitId { get; private set; }
    public DateTime SlabIncStrtDate { get; private set; }
    public DateTime? SlabIncClsDate { get; private set; }
    public long SlabIncModifiedBy { get; private set; }
    public DateTime SlabIncModifiedOn { get; private set; }

    private BasicSlabIncrement() { }
}

/// <summary>Maps to COMP_PARAMS table</summary>
public class CompensationParameter : BaseEntity
{
    public long CpId { get; private set; }
    public string CpCountryCode { get; private set; } = default!;
    public string CpEdGroup { get; private set; } = default!;
    public string CpType { get; private set; } = default!; // EMP/COM
    public long CpEdId { get; private set; }
    public long CpModifiedBy { get; private set; }
    public DateTime CpModifiedOn { get; private set; }

    private CompensationParameter() { }
}

/// <summary>Maps to DILIGENCE_RATEMAST table</summary>
public class DiligenceRateMaster : BaseEntity
{
    public long DiligenceId { get; private set; }
    public long DiligencePayUnitId { get; private set; }
    public string DiligenceGradeCategory { get; private set; } = default!;
    public long DiligenceEdId { get; private set; }
    public int DiligenceYearId { get; private set; }
    public decimal DiligenceAmount { get; private set; }
    public DateTime DiligenceEffDate { get; private set; }
    public DateTime? DiligenceClsDate { get; private set; }
    public long DiligenceLastModifiedBy { get; private set; }
    public DateTime DiligenceLastModifiedOn { get; private set; }
    public long? DiligenceBenLogId { get; private set; }

    private DiligenceRateMaster() { }
}

/// <summary>Maps to PMS_CASHPAY table</summary>
public class PmsCashPay : BaseEntity
{
    public long CashPayId { get; private set; }
    public long CashPayUnitId { get; private set; }
    public string CashPayGradeCat { get; private set; } = default!;
    public string CashPayPayType { get; private set; } = default!; // I=Immediate, P=PartPayment
    public DateTime CashPayEffDate { get; private set; }
    public DateTime? CashPayClsDate { get; private set; }
    public long CashPayModifiedBy { get; private set; }
    public DateTime CashPayModifiedOn { get; private set; }

    public ICollection<PmsCashPayDetail> Details { get; private set; } = [];

    private PmsCashPay() { }
}

/// <summary>Maps to PMS_CASHPAYDET table</summary>
public class PmsCashPayDetail : BaseEntity
{
    public long CashPayDetId { get; private set; }
    public long CashPayId { get; private set; }
    public decimal CashPayPer { get; private set; }
    public string CashPayPayDate { get; private set; } = default!;

    public PmsCashPay CashPay { get; private set; } = default!;
    private PmsCashPayDetail() { }
}

/// <summary>Maps to EMPLOYEE_CTCREMARKS table</summary>
public class EmployeeCtcRemarks : BaseEntity
{
    public long CtcRemEmpSysId { get; private set; }
    public long CtcRemId { get; private set; }
    public string? CtcRemLine1 { get; private set; }
    public string? CtcRemLine2 { get; private set; }
    public string? CtcRemLine3 { get; private set; }
    public long? CtcRemUpdatedBy { get; private set; }
    public DateTime? CtcRemUpdatedOn { get; private set; }

    private EmployeeCtcRemarks() { }
}

/// <summary>Maps to TEVCTC table</summary>
public class TevCtc : BaseEntity
{
    public long CtcEmpSysId { get; private set; }
    public long CtcId { get; private set; }
    public DateTime CtcEffDat { get; private set; }
    public DateTime? CtcClsDat { get; private set; }
    public long CtcEdId { get; private set; }
    public string CtcEdFreq { get; private set; } = default!;
    public decimal CtcEdAmtPa { get; private set; }
    public long CtcTranNo { get; private set; }
    public string CtcSource { get; private set; } = default!;
    public long CtcStructureId { get; private set; }
    public long CtcUpdatedBy { get; private set; }
    public DateTime CtcUpdatedOn { get; private set; }
    public string CtcFormula { get; private set; } = "N";
    public decimal? CtcLogNo { get; private set; }

    private TevCtc() { }
}
