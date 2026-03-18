using CurrencyManagement.Domain.Common;
using CurrencyManagement.Domain.Events;
using CurrencyManagement.Domain.ValueObjects;

namespace CurrencyManagement.Domain.Entities;

/// <summary>
/// Currency aggregate root - represents a currency in the system
/// Maps to DEAL_CURRMAST table
/// </summary>
public class Currency : BaseEntity
{
    public long CurrencyId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public CurrencySymbol Symbol { get; private set; } = null!;
    public long ModifiedBy { get; private set; }
    public DateTime ModifiedOn { get; private set; }

    private readonly List<ExchangeRate> _exchangeRates = new();
    /// <summary>
    /// Gets the collection of exchange rates for this currency
    /// </summary>
    public IReadOnlyCollection<ExchangeRate> ExchangeRates => _exchangeRates.AsReadOnly();

    #region Constructors
    private Currency() { }

    public Currency(long currencyId, string name, CurrencySymbol symbol, long modifiedBy)
    {
        CurrencyId = currencyId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;

        RaiseDomainEvent(new CurrencyCreatedDomainEvent(currencyId, name, symbol.Value));
    }
    #endregion

    #region Methods
 /// <summary>
    /// Updates the currency information
    /// </summary>
    public void Update(string name, CurrencySymbol symbol, long modifiedBy)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Currency name cannot be empty", nameof(name));

        bool changed = Name != name || Symbol != symbol;

        Name = name;
        Symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;

        if (changed)
        {
            RaiseDomainEvent(new CurrencyUpdatedDomainEvent(CurrencyId, name, symbol.Value));
        }
    }

    /// <summary>
    /// Adds an exchange rate for this currency
    /// </summary>
    public void AddExchangeRate(ExchangeRate exchangeRate)
    {
        if (exchangeRate == null)
            throw new ArgumentNullException(nameof(exchangeRate));

        _exchangeRates.AddExchangeRateIfNotExists(exchangeRate);
    }

    /// <summary>
    /// Removes an exchange rate
    /// </summary>
    public void RemoveExchangeRate(long rateId)
    {
        var rate = _exchangeRates.FirstOrDefault(r => r.RateId == rateId);
        if (rate != null)
        {
            _exchangeRates.Remove(rate);
        }
    }
    #endregion
}

/// <summary>
/// Static helper extension methods for Currency
/// </summary>
public static class CurrencyExtensions
{
    public static void AddExchangeRateIfNotExists(this List<ExchangeRate> rates, ExchangeRate newRate)
    {
        var existing = rates.FirstOrDefault(r =>
            r.FromCurrencyId == newRate.FromCurrencyId &&
            r.ToCurrencyId == newRate.ToCurrencyId &&
            r.FinancialYear == newRate.FinancialYear &&
            r.Month == newRate.Month);

        if (existing != null)
        {
            rates.Remove(existing);
        }

        rates.Add(newRate);
    }
}
