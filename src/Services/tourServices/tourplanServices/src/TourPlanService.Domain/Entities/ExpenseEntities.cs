using TourPlanService.Domain.Common;

namespace TourPlanService.Domain.Entities;

/// <summary>TRAVEL_DOMDABREAK - Travel Domestic DA Days Breakup</summary>
public sealed class DomesticDaBreak : BaseEntity
{
    private DomesticDaBreak() { }

    public string DomDaId { get; private set; } = default!;
    public string? DomDaTpId { get; private set; }
    public DateTime? DomDaFromDate { get; private set; }
    public DateTime? DomDaToDate { get; private set; }
    public string? DomDaDaDays { get; private set; }
    public DateTime? DomDaDaEffDate { get; private set; }
    public DateTime? DomDaDaClsDate { get; private set; }
    public string? DomDaDaActualDays { get; private set; }
    public string? DomDaDaRate { get; private set; }
    public string? DomDaLeaveDays { get; private set; }
    public string? DomDaFoodExpDays { get; private set; }
    public string? DomDaOwnStayTDays { get; private set; }
    public string? DomDaFinalDays { get; private set; }
    public string? DomDaFinalValue { get; private set; }

    public TourPlan? TourPlan { get; private set; }

    public static DomesticDaBreak Create(string domDaId, string? tpId = null) =>
        new() { DomDaId = domDaId, DomDaTpId = tpId };
}

/// <summary>TRAVEL_EXPENSEINTMAIN - Travel Foreign Expense Main</summary>
public sealed class ForeignExpenseMain : BaseEntity
{
    private readonly List<ForeignExpenseDetail> _details = [];
    private ForeignExpenseMain() { }

    public string TpExpMainId { get; private set; } = default!;
    public string TpExpMainTpId { get; private set; } = default!;
    public string? TpExpMainClaimType { get; private set; }
    public string? TpExpMainLocCur { get; private set; }
    public DateTime TpExpMainSetDate { get; private set; }
    public DateTime TpExpMainAppSetDate { get; private set; }
    public string TpExpMainIntCur1 { get; private set; } = default!;
    public string TpExpMainIntCur2 { get; private set; } = default!;
    public string TpExpMainIntCnv1 { get; private set; } = default!;
    public string TpExpMainIntCnv2 { get; private set; } = default!;
    public string TpExpMainIntVal1 { get; private set; } = default!;
    public string TpExpMainIntVal2 { get; private set; } = default!;
    public string TpExpMainBalAmt { get; private set; } = default!;

    public TourPlan TourPlan { get; private set; } = default!;
    public IReadOnlyCollection<ForeignExpenseDetail> Details => _details.AsReadOnly();

    public static ForeignExpenseMain Create(
        string id, string tpId, DateTime setDate, DateTime appSetDate,
        string intCur1, string intCur2, string intCnv1, string intCnv2,
        string intVal1, string intVal2, string balAmt) =>
        new()
        {
            TpExpMainId = id, TpExpMainTpId = tpId, TpExpMainSetDate = setDate,
            TpExpMainAppSetDate = appSetDate, TpExpMainIntCur1 = intCur1,
            TpExpMainIntCur2 = intCur2, TpExpMainIntCnv1 = intCnv1,
            TpExpMainIntCnv2 = intCnv2, TpExpMainIntVal1 = intVal1,
            TpExpMainIntVal2 = intVal2, TpExpMainBalAmt = balAmt
        };
}

/// <summary>TRAVEL_EXPENSEINTDET - Travel Foreign Expense Detail</summary>
public sealed class ForeignExpenseDetail : BaseEntity
{
    private readonly List<ForeignExpenseBreakup> _breakups = [];
    private ForeignExpenseDetail() { }

    public string TpExpDetId { get; private set; } = default!;
    public string TpExpDetTpId { get; private set; } = default!;
    public string TpExpDetGroupId { get; private set; } = default!;
    public string TpExpDetCurrency { get; private set; } = default!;
    public string TpExpDetValue { get; private set; } = default!;
    public string TpExpDetActValue { get; private set; } = default!;
    public string TpExpDetAppAmt { get; private set; } = default!;
    public string TpExpDetExpFlag { get; private set; } = default!;

    public ForeignExpenseMain ForeignExpenseMain { get; private set; } = default!;
    public IReadOnlyCollection<ForeignExpenseBreakup> Breakups => _breakups.AsReadOnly();

    public static ForeignExpenseDetail Create(
        string id, string tpId, string groupId, string currency,
        string value, string actValue, string appAmt, string expFlag) =>
        new()
        {
            TpExpDetId = id, TpExpDetTpId = tpId, TpExpDetGroupId = groupId,
            TpExpDetCurrency = currency, TpExpDetValue = value,
            TpExpDetActValue = actValue, TpExpDetAppAmt = appAmt, TpExpDetExpFlag = expFlag
        };
}

/// <summary>TRAVEL_EXPENSEINTBRK - Travel Foreign Expense Breakup</summary>
public sealed class ForeignExpenseBreakup : BaseEntity
{
    private ForeignExpenseBreakup() { }

    public string TpExpBrkId { get; private set; } = default!;
    public string TpExpBrkDetId { get; private set; } = default!;
    public string TpExpBrkExpId { get; private set; } = default!;
    public DateTime? TpExpBrkDate { get; private set; }
    public string TpExpBrkRemarks { get; private set; } = default!;
    public string TpExpBrkAmt { get; private set; } = default!;
    public string TpExpBrkActAmt { get; private set; } = default!;
    public string TpExpBrkAppAmt { get; private set; } = default!;
    public string? TpExpBrkPayMode { get; private set; }

    public ForeignExpenseDetail ForeignExpenseDetail { get; private set; } = default!;

    public static ForeignExpenseBreakup Create(
        string brkId, string detId, string expId, string remarks,
        string amt, string actAmt, string appAmt, DateTime? date = null, string? payMode = null) =>
        new()
        {
            TpExpBrkId = brkId, TpExpBrkDetId = detId, TpExpBrkExpId = expId,
            TpExpBrkRemarks = remarks, TpExpBrkAmt = amt, TpExpBrkActAmt = actAmt,
            TpExpBrkAppAmt = appAmt, TpExpBrkDate = date, TpExpBrkPayMode = payMode
        };
}
