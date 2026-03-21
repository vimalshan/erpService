using TourPlanService.Domain.Common;

namespace TourPlanService.Domain.Entities;

/// <summary>TOURPLAN_AGENDA - Travel Agenda</summary>
public sealed class TourAgenda : BaseEntity
{
    private TourAgenda() { }

    public string AgendaId { get; private set; } = default!;
    public string AgendaTpId { get; private set; } = default!;
    public string AgendaCity { get; private set; } = default!;
    public string AgendaMeet { get; private set; } = default!;
    public string AgendaOutcome { get; private set; } = default!;
    public DateTime? AgendaType { get; private set; }

    public TourPlan TourPlan { get; private set; } = default!;

    public static TourAgenda Create(string agendaId, string tpId, string city, string meet, string outcome)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(agendaId);
        return new TourAgenda
        {
            AgendaId = agendaId,
            AgendaTpId = tpId,
            AgendaCity = city,
            AgendaMeet = meet,
            AgendaOutcome = outcome
        };
    }
}

/// <summary>TOURPLAN_COSTCENTRE - Travel Cost Allocation</summary>
public sealed class TourCostCentre : BaseEntity
{
    private TourCostCentre() { }

    public string TpCostId { get; private set; } = default!;
    public string TpCostTpId { get; private set; } = default!;
    public string TpCostBuCode { get; private set; } = default!;
    public string TpCostCcCode { get; private set; } = default!;
    public string TpCostSubAccCode { get; private set; } = default!;
    public string TpCostProductCode { get; private set; } = default!;
    public string TpCostLocSegment { get; private set; } = default!;
    public string TpCostAlllPer { get; private set; } = default!;
    public string? TpCostDefault { get; private set; }

    public TourPlan TourPlan { get; private set; } = default!;

    public static TourCostCentre Create(
        string costId, string tpId, string buCode, string ccCode,
        string subAccCode, string productCode, string locSegment, string alllPer, string? isDefault = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(costId);
        return new TourCostCentre
        {
            TpCostId = costId,
            TpCostTpId = tpId,
            TpCostBuCode = buCode,
            TpCostCcCode = ccCode,
            TpCostSubAccCode = subAccCode,
            TpCostProductCode = productCode,
            TpCostLocSegment = locSegment,
            TpCostAlllPer = alllPer,
            TpCostDefault = isDefault
        };
    }
}

/// <summary>TOURPLAN_DABREAK - DA Breakup</summary>
public sealed class TourDaBreak : BaseEntity
{
    private TourDaBreak() { }

    public string TpDaId { get; private set; } = default!;
    public string TpDaTpId { get; private set; } = default!;
    public string TpDaCountryId { get; private set; } = default!;
    public string TpDaCurrency { get; private set; } = default!;
    public string TpDaDays { get; private set; } = default!;
    public string TpDaRate { get; private set; } = default!;
    public string? TpDaGhDays { get; private set; }
    public string? TpDaGhRate { get; private set; }

    public TourPlan TourPlan { get; private set; } = default!;

    public static TourDaBreak Create(
        string daId, string tpId, string countryId,
        string currency, string days, string rate)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(daId);
        return new TourDaBreak
        {
            TpDaId = daId,
            TpDaTpId = tpId,
            TpDaCountryId = countryId,
            TpDaCurrency = currency,
            TpDaDays = days,
            TpDaRate = rate
        };
    }
}

/// <summary>TOURPLAN_EXPENSE - Travel Approximate Expense</summary>
public sealed class TourExpense : BaseEntity
{
    private TourExpense() { }

    public string TpExpId { get; private set; } = default!;
    public string TpExpTpId { get; private set; } = default!;
    public string TpExpExpId { get; private set; } = default!;
    public string TpExpCur { get; private set; } = default!;
    public string TpExpExpAmt { get; private set; } = default!;
    public string? TpExpRemarks { get; private set; }

    public TourPlan TourPlan { get; private set; } = default!;

    public static TourExpense Create(
        string expId, string tpId, string expenseId, string currency, string amount)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(expId);
        return new TourExpense
        {
            TpExpId = expId,
            TpExpTpId = tpId,
            TpExpExpId = expenseId,
            TpExpCur = currency,
            TpExpExpAmt = amount
        };
    }
}
