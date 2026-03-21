using TravelService.Domain.Common;

namespace TravelService.Domain.Entities.TourPlan;

public class TourPlanSelfExpense : Entity<string>
{
    public string TourPlanId { get; private set; } = string.Empty;
    public string ExpenseCategory { get; private set; } = string.Empty;
    public string TravelMode { get; private set; } = string.Empty;
    public DateTime FromDate { get; private set; }
    public string FromCityId { get; private set; } = string.Empty;
    public string FromCityName { get; private set; } = string.Empty;
    public DateTime ToDate { get; private set; }
    public string ToCityId { get; private set; } = string.Empty;
    public string ToCityName { get; private set; } = string.Empty;
    public decimal NumberOfDays { get; private set; }
    public decimal EntitlementValue { get; private set; }
    public decimal ExpenseValue { get; private set; }
    public decimal ServiceTaxValue { get; private set; }
    public decimal AdditionalCharges { get; private set; }
    public string TravelClass { get; private set; } = string.Empty;
    public string Remarks { get; private set; } = string.Empty;
    public decimal ApprovedAmount { get; private set; }
    public string? FinanceRemarks { get; private set; }
    public string ExpenseId { get; private set; } = string.Empty;

    protected TourPlanSelfExpense() { }

    public static TourPlanSelfExpense Create(
        string id, string tourPlanId, string expenseCategory, string travelMode,
        DateTime fromDate, string fromCityId, string fromCityName,
        DateTime toDate, string toCityId, string toCityName,
        decimal numberOfDays, decimal entitlementValue, decimal expenseValue,
        decimal serviceTaxValue, decimal additionalCharges, string travelClass,
        string remarks, decimal approvedAmount, string expenseId)
        => new()
        {
            Id = id,
            TourPlanId = tourPlanId,
            ExpenseCategory = expenseCategory,
            TravelMode = travelMode,
            FromDate = fromDate,
            FromCityId = fromCityId,
            FromCityName = fromCityName,
            ToDate = toDate,
            ToCityId = toCityId,
            ToCityName = toCityName,
            NumberOfDays = numberOfDays,
            EntitlementValue = entitlementValue,
            ExpenseValue = expenseValue,
            ServiceTaxValue = serviceTaxValue,
            AdditionalCharges = additionalCharges,
            TravelClass = travelClass,
            Remarks = remarks,
            ApprovedAmount = approvedAmount,
            ExpenseId = expenseId
        };
}
