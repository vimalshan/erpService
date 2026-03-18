using CurrencyManagement.Domain.Entities;

namespace CurrencyManagement.Domain.Interfaces;

/// <summary>
/// Repository contract for Currency aggregate
/// </summary>
public interface ICurrencyRepository
{
    /// <summary>
    /// Gets a currency by its ID
    /// </summary>
    Task<Currency?> GetByIdAsync(long currencyId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all currencies
    /// </summary>
    Task<IList<Currency>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new currency
    /// </summary>
    Task AddAsync(Currency currency, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing currency
    /// </summary>
    Task UpdateAsync(Currency currency, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a currency
    /// </summary>
    Task DeleteAsync(long currencyId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a currency exists
    /// </summary>
    Task<bool> ExistsAsync(long currencyId, CancellationToken cancellationToken = default);
}

/// <summary>
/// Repository contract for ExchangeRate entity
/// </summary>
public interface IExchangeRateRepository
{
    /// <summary>
    /// Gets an exchange rate by ID
    /// </summary>
    Task<ExchangeRate?> GetByIdAsync(long rateId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets exchange rate for a specific currency pair and period
    /// </summary>
    Task<ExchangeRate?> GetRateAsync(long fromCurrencyId, long toCurrencyId, long financialYear, long month, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all exchange rates
    /// </summary>
    Task<IList<ExchangeRate>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets exchange rates for a specific financial year and month
    /// </summary>
    Task<IList<ExchangeRate>> GetByPeriodAsync(long financialYear, long month, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new exchange rate
    /// </summary>
    Task AddAsync(ExchangeRate exchangeRate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing exchange rate
    /// </summary>
    Task UpdateAsync(ExchangeRate exchangeRate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an exchange rate
    /// </summary>
    Task DeleteAsync(long rateId, CancellationToken cancellationToken = default);
}

/// <summary>
/// Repository contract for OrganizationCurrencyMapping entity
/// </summary>
public interface IOrganizationCurrencyRepository
{
    /// <summary>
    /// Gets currencies for an organization
    /// </summary>
    Task<IList<OrganizationCurrencyMapping>> GetByOrganizationAsync(long organizationId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the mapping for a specific organization and currency
    /// </summary>
    Task<OrganizationCurrencyMapping?> GetOrganizationCurrencyAsync(long organizationId, long currencyId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all organization currency mappings
    /// </summary>
    Task<IList<OrganizationCurrencyMapping>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new mapping
    /// </summary>
    Task AddAsync(OrganizationCurrencyMapping mapping, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing mapping
    /// </summary>
    Task UpdateAsync(OrganizationCurrencyMapping mapping, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a mapping
    /// </summary>
    Task DeleteAsync(long organizationId, long currencyId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if an organization has a currency mapped
    /// </summary>
    Task<bool> IsCurrencyMappedAsync(long organizationId, long currencyId, CancellationToken cancellationToken = default);
}
