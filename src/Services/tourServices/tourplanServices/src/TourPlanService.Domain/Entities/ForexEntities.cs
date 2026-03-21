using TourPlanService.Domain.Common;
using TourPlanService.Domain.Events;

namespace TourPlanService.Domain.Entities;

/// <summary>TOURPLAN_SLFEXP - Self Expense Ticket Details</summary>
public sealed class SelfExpense : BaseEntity
{
    private SelfExpense() { }

    public string ExpTktId { get; private set; } = default!;
    public string ExpTpId { get; private set; } = default!;
    public string ExpExpCat { get; private set; } = default!;
    public string ExpTravelMode { get; private set; } = default!;
    public DateTime ExpFromDate { get; private set; }
    public string ExpFromCity { get; private set; } = default!;
    public string ExpFromCityName { get; private set; } = default!;
    public DateTime ExpToDate { get; private set; }
    public string ExpToCity { get; private set; } = default!;
    public string ExpToCityName { get; private set; } = default!;
    public string ExpNoOfDays { get; private set; } = default!;
    public string ExpEntitleValue { get; private set; } = default!;
    public string ExpValue { get; private set; } = default!;
    public string ExpSerTaxVal { get; private set; } = default!;
    public string ExpAdlValue { get; private set; } = default!;
    public string ExpTravelClass { get; private set; } = default!;
    public string ExpRemarks { get; private set; } = default!;
    public string ExpApprovedAmt { get; private set; } = default!;
    public string? ExpFinRemarks { get; private set; }
    public string ExpExpId { get; private set; } = default!;

    public TourPlan TourPlan { get; private set; } = default!;

    public static SelfExpense Create(
        string tktId, string tpId, string expCat, string travelMode,
        DateTime fromDate, string fromCity, string fromCityName,
        DateTime toDate, string toCity, string toCityName,
        string noOfDays, string entitleValue, string value,
        string serTaxVal, string adlValue, string travelClass,
        string remarks, string approvedAmt, string expId) =>
        new()
        {
            ExpTktId = tktId, ExpTpId = tpId, ExpExpCat = expCat,
            ExpTravelMode = travelMode, ExpFromDate = fromDate, ExpFromCity = fromCity,
            ExpFromCityName = fromCityName, ExpToDate = toDate, ExpToCity = toCity,
            ExpToCityName = toCityName, ExpNoOfDays = noOfDays, ExpEntitleValue = entitleValue,
            ExpValue = value, ExpSerTaxVal = serTaxVal, ExpAdlValue = adlValue,
            ExpTravelClass = travelClass, ExpRemarks = remarks, ExpApprovedAmt = approvedAmt,
            ExpExpId = expId
        };
}

/// <summary>TOURPLAN_FOREXMAIN - Travel Forex Requisition Main</summary>
public sealed class ForexRequisition : BaseEntity
{
    private readonly List<ForexDetail> _details = [];
    private readonly List<ForexChequeDetail> _chequeDetails = [];
    private ForexRequisition() { }

    public string ForReqId { get; private set; } = default!;
    public string ForReqTpId { get; private set; } = default!;
    public string ForReqPassNo { get; private set; } = default!;
    public string ForReqPassName { get; private set; } = default!;
    public string ForReqPassLocation { get; private set; } = default!;
    public DateTime ForReqPassExpDate { get; private set; }
    public string? ForReqDestination { get; private set; }
    public string? ForReqStatus { get; private set; }
    public string? ForReqDate { get; private set; }
    public string ForReqLastModifiedBy { get; private set; } = default!;
    public DateTime ForReqLastModifiedOn { get; private set; }
    public DateTime? ForReqReceivedOn { get; private set; }
    public string? ForReqRefNo { get; private set; }
    public string ForReqTax1 { get; private set; } = default!;
    public string ForReqTax2 { get; private set; } = default!;
    public string ForReqTax3 { get; private set; } = default!;
    public string ForReqTax4 { get; private set; } = default!;
    public string ForReqTax5 { get; private set; } = default!;
    public string? ForReqVendorId { get; private set; }
    public string? ForReqCurrency { get; private set; }
    public string? ForReqTotValue { get; private set; }
    public string? ForReqRecBy { get; private set; }
    public string ForReqType { get; private set; } = default!;
    public string ForReqAdlRemarks { get; private set; } = default!;
    public string ForReqAdvRefNo { get; private set; } = default!;
    public string? ForReqNetPay { get; private set; }
    public string? ForReqCurDenoAdj { get; private set; }
    public DateTime? ForReqEncashCertDate { get; private set; }
    public string? ForReqBasAmt { get; private set; }
    public string? ForReqCgstAmt { get; private set; }
    public string? ForReqSgstAmt { get; private set; }
    public string? ForReqIgstAmt { get; private set; }
    public string? ForReqCgstCharges { get; private set; }
    public string? ForReqSgstCharges { get; private set; }
    public string? ForReqIgstCharges { get; private set; }

    public TourPlan TourPlan { get; private set; } = default!;
    public IReadOnlyCollection<ForexDetail> Details => _details.AsReadOnly();
    public IReadOnlyCollection<ForexChequeDetail> ChequeDetails => _chequeDetails.AsReadOnly();

    public static ForexRequisition Create(
        string forReqId, string tpId, string passNo, string passName,
        string passLocation, DateTime passExpDate, string type,
        string adlRemarks, string advRefNo, string modifiedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(forReqId);

        var forex = new ForexRequisition
        {
            ForReqId = forReqId,
            ForReqTpId = tpId,
            ForReqPassNo = passNo,
            ForReqPassName = passName,
            ForReqPassLocation = passLocation,
            ForReqPassExpDate = passExpDate,
            ForReqType = type,
            ForReqAdlRemarks = adlRemarks,
            ForReqAdvRefNo = advRefNo,
            ForReqTax1 = "0",
            ForReqTax2 = "0",
            ForReqTax3 = "0",
            ForReqTax4 = "0",
            ForReqTax5 = "0",
            ForReqLastModifiedBy = modifiedBy,
            ForReqLastModifiedOn = DateTime.UtcNow
        };

        forex.RaiseDomainEvent(new ForexRequisitionCreatedEvent(
            Guid.NewGuid(), forReqId, tpId, modifiedBy, DateTime.UtcNow));

        return forex;
    }

    public void AddDetail(ForexDetail detail) => _details.Add(detail);
    public void AddChequeDetail(ForexChequeDetail cheque) => _chequeDetails.Add(cheque);
}

/// <summary>TOURPLAN_FOREXDET - Travel Forex Currency Detail</summary>
public sealed class ForexDetail : BaseEntity
{
    private ForexDetail() { }

    public string ForexId { get; private set; } = default!;
    public string ForexReqId { get; private set; } = default!;
    public string ForexSrcValue { get; private set; } = default!;
    public string ForexCurrency { get; private set; } = default!;
    public string ForexValue { get; private set; } = default!;
    public string? ForexExgRate { get; private set; }
    public string? ForexExgValue { get; private set; }
    public string? ForexPayMode { get; private set; }
    public string? ForexReqCurVal { get; private set; }
    public string? ForexReqCurRecd { get; private set; }

    public ForexRequisition ForexRequisition { get; private set; } = default!;

    public static ForexDetail Create(
        string forexId, string reqId, string srcValue,
        string currency, string value) =>
        new()
        {
            ForexId = forexId, ForexReqId = reqId,
            ForexSrcValue = srcValue, ForexCurrency = currency, ForexValue = value
        };
}

/// <summary>TOURPLAN_FOREXCHQDET - Forex Cheque Details</summary>
public sealed class ForexChequeDetail : BaseEntity
{
    private ForexChequeDetail() { }

    public string ForexChqDetId { get; private set; } = default!;
    public string ForexReqId { get; private set; } = default!;
    public string ForexChqNo { get; private set; } = default!;
    public DateTime? ForexChqDate { get; private set; }
    public string ForexBankName { get; private set; } = default!;

    public ForexRequisition ForexRequisition { get; private set; } = default!;

    public static ForexChequeDetail Create(
        string chqDetId, string reqId, string chqNo, string bankName, DateTime? chqDate = null) =>
        new()
        {
            ForexChqDetId = chqDetId, ForexReqId = reqId,
            ForexChqNo = chqNo, ForexBankName = bankName, ForexChqDate = chqDate
        };
}
