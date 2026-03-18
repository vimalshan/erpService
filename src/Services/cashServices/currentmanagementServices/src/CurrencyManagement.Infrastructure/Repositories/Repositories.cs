using CurrencyManagement.Domain.Entities;
using CurrencyManagement.Domain.Interfaces;
using CurrencyManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CurrencyManagement.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Currency aggregate
/// </summary>
public class CurrencyRepository : ICurrencyRepository
{
    private readonly CurrencyDbContext _context;

    public CurrencyRepository(CurrencyDbContext context)
    {
        _context = context;
    }

    public async Task<Currency?> GetByIdAsync(long currencyId, CancellationToken cancellationToken = default)
    {
        return await _context.Currencies.FirstOrDefaultAsync(c => c.CurrencyId == currencyId, cancellationToken);
    }

    public async Task<IList<Currency>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Currencies.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Currency currency, CancellationToken cancellationToken = default)
    {
        await _context.Currencies.AddAsync(currency, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Currency currency, CancellationToken cancellationToken = default)
    {
        _context.Currencies.Update(currency);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(long currencyId, CancellationToken cancellationToken = default)
    {
        var currency = await GetByIdAsync(currencyId, cancellationToken);
        if (currency != null)
        {
            _context.Currencies.Remove(currency);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(long currencyId, CancellationToken cancellationToken = default)
    {
        return await _context.Currencies.AnyAsync(c => c.CurrencyId == currencyId, cancellationToken);
    }
}

/// <summary>
/// Repository implementation for ExchangeRate entity
/// </summary>
public class ExchangeRateRepository : IExchangeRateRepository
{
    private readonly CurrencyDbContext _context;

    public ExchangeRateRepository(CurrencyDbContext context)
    {
        _context = context;
    }

    public async Task<ExchangeRate?> GetByIdAsync(long rateId, CancellationToken cancellationToken = default)
    {
        return await _context.ExchangeRates.FirstOrDefaultAsync(e => e.RateId == rateId, cancellationToken);
    }

    public async Task<ExchangeRate?> GetRateAsync(long fromCurrencyId, long toCurrencyId, long financialYear, long month, CancellationToken cancellationToken = default)
    {
        return await _context.ExchangeRates.FirstOrDefaultAsync(e =>
            e.FromCurrencyId == fromCurrencyId &&
            e.ToCurrencyId == toCurrencyId &&
            e.FinancialYear == financialYear &&
            e.Month == month, cancellationToken);
    }

    public async Task<IList<ExchangeRate>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ExchangeRates.ToListAsync(cancellationToken);
    }

    public async Task<IList<ExchangeRate>> GetByPeriodAsync(long financialYear, long month, CancellationToken cancellationToken = default)
    {
        return await _context.ExchangeRates
            .Where(e => e.FinancialYear == financialYear && e.Month == month)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ExchangeRate exchangeRate, CancellationToken cancellationToken = default)
    {
        await _context.ExchangeRates.AddAsync(exchangeRate, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(ExchangeRate exchangeRate, CancellationToken cancellationToken = default)
    {
        _context.ExchangeRates.Update(exchangeRate);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(long rateId, CancellationToken cancellationToken = default)
    {
        var rate = await GetByIdAsync(rateId, cancellationToken);
        if (rate != null)
        {
            _context.ExchangeRates.Remove(rate);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

/// <summary>
/// Repository implementation for OrganizationCurrencyMapping entity
/// </summary>
public class OrganizationCurrencyRepository : IOrganizationCurrencyRepository
{
    private readonly CurrencyDbContext _context;

    public OrganizationCurrencyRepository(CurrencyDbContext context)
    {
        _context = context;
    }

    public async Task<IList<OrganizationCurrencyMapping>> GetByOrganizationAsync(long organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.OrganizationCurrencyMappings
            .Where(m => m.OrganizationId == organizationId)
            .ToListAsync(cancellationToken);
    }

    public async Task<OrganizationCurrencyMapping?> GetOrganizationCurrencyAsync(long organizationId, long currencyId, CancellationToken cancellationToken = default)
    {
        return await _context.OrganizationCurrencyMappings
            .FirstOrDefaultAsync(m => m.OrganizationId == organizationId && m.CurrencyId == currencyId, cancellationToken);
    }

    public async Task<IList<OrganizationCurrencyMapping>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.OrganizationCurrencyMappings.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(OrganizationCurrencyMapping mapping, CancellationToken cancellationToken = default)
    {
        await _context.OrganizationCurrencyMappings.AddAsync(mapping, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(OrganizationCurrencyMapping mapping, CancellationToken cancellationToken = default)
    {
        _context.OrganizationCurrencyMappings.Update(mapping);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(long organizationId, long currencyId, CancellationToken cancellationToken = default)
    {
        var mapping = await GetOrganizationCurrencyAsync(organizationId, currencyId, cancellationToken);
        if (mapping != null)
        {
            _context.OrganizationCurrencyMappings.Remove(mapping);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> IsCurrencyMappedAsync(long organizationId, long currencyId, CancellationToken cancellationToken = default)
    {
        return await _context.OrganizationCurrencyMappings
            .AnyAsync(m => m.OrganizationId == organizationId && m.CurrencyId == currencyId, cancellationToken);
    }
}
