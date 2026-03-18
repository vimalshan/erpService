using CurrencyManagement.Domain.Common;

namespace CurrencyManagement.Domain.Events;

/// <summary>
/// Raised when a new currency is created
/// </summary>
public class CurrencyCreatedDomainEvent : DomainEvent
{
    public long CurrencyId { get; }
    public string Name { get; }
    public string Symbol { get; }

    public CurrencyCreatedDomainEvent(long currencyId, string name, string symbol)
    {
        CurrencyId = currencyId;
        Name = name;
        Symbol = symbol;
    }
}

/// <summary>
/// Raised when a currency is updated
/// </summary>
public class CurrencyUpdatedDomainEvent : DomainEvent
{
    public long CurrencyId { get; }
    public string Name { get; }
    public string Symbol { get; }

    public CurrencyUpdatedDomainEvent(long currencyId, string name, string symbol)
    {
        CurrencyId = currencyId;
        Name = name;
        Symbol = symbol;
    }
}

/// <summary>
/// Raised when a currency is deleted
/// </summary>
public class CurrencyDeletedDomainEvent : DomainEvent
{
    public long CurrencyId { get; }

    public CurrencyDeletedDomainEvent(long currencyId)
    {
        CurrencyId = currencyId;
    }
}

/// <summary>
/// Raised when an exchange rate is set
/// </summary>
public class ExchangeRateSetDomainEvent : DomainEvent
{
    public long RateId { get; }
    public long FromCurrencyId { get; }
    public long ToCurrencyId { get; }
    public decimal Rate { get; }

    public ExchangeRateSetDomainEvent(long rateId, long fromCurrencyId, long toCurrencyId, decimal rate)
    {
        RateId = rateId;
        FromCurrencyId = fromCurrencyId;
        ToCurrencyId = toCurrencyId;
        Rate = rate;
    }
}

/// <summary>
/// Raised when an organization is mapped to a currency
/// </summary>
public class OrganizationCurrencyMappedDomainEvent : DomainEvent
{
    public long OrganizationId { get; }
    public long CurrencyId { get; }

    public OrganizationCurrencyMappedDomainEvent(long organizationId, long currencyId)
    {
        OrganizationId = organizationId;
        CurrencyId = currencyId;
    }
}
