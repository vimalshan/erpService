using CurrencyManagement.Domain.Common;
using CurrencyManagement.Domain.ValueObjects;

namespace CurrencyManagement.Domain.Entities;

/// <summary>
/// Currency exchange rate entity
/// Maps to DEAL_CURRATES table
/// </summary>
public class ExchangeRate : BaseEntity
{
    public long RateId { get; private set; }
    public long FinancialYear { get; private set; }
    public long Month { get; private set; }
    public long FromCurrencyId { get; private set; }
    public long ToCurrencyId { get; private set; }
    public ExchangeRateValue Rate { get; private set; } = null!;
    public long ModifiedBy { get; private set; }
    public DateTime ModifiedOn { get; private set; }

    #region Constructors
    private ExchangeRate() { }

    public ExchangeRate(long rateId, long financialYear, long month, long fromCurrencyId, long toCurrencyId, ExchangeRateValue rate, long modifiedBy)
    {
        if (financialYear <= 0)
            throw new ArgumentException("Financial year must be positive", nameof(financialYear));

        if (month < 1 || month > 12)
            throw new ArgumentException("Month must be between 1 and 12", nameof(month));

        if (fromCurrencyId <= 0)
            throw new ArgumentException("From currency ID must be positive", nameof(fromCurrencyId));

        if (toCurrencyId <= 0)
            throw new ArgumentException("To currency ID must be positive", nameof(toCurrencyId));

        if (fromCurrencyId == toCurrencyId)
            throw new ArgumentException("From and To currencies cannot be the same", nameof(fromCurrencyId));

        RateId = rateId;
        FinancialYear = financialYear;
        Month = month;
        FromCurrencyId = fromCurrencyId;
        ToCurrencyId = toCurrencyId;
        Rate = rate ?? throw new ArgumentNullException(nameof(rate));
        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Updates the exchange rate
    /// </summary>
    public void UpdateRate(ExchangeRateValue newRate, long modifiedBy)
    {
        Rate = newRate ?? throw new ArgumentNullException(nameof(newRate));
        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;
    }

    /// <summary>
    /// Converts an amount from source to target currency using this rate
    /// </summary>
    public decimal ConvertAmount(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));

        // The rate is stored with 6 decimal places (DECIMAL(19,0) in DB but value is like 1175000 for 1.175)
        return amount * Rate.Value / 1_000_000m;
    }
    #endregion
}
