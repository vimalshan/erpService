using ExpenseService.Domain.Entities;
using ExpenseService.Domain.Interfaces;
using ExpenseService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseService.Infrastructure.Repositories;

public class CurrencyRepository : ICurrencyRepository
{
    private readonly ExpenseDbContext _context;

    public CurrencyRepository(ExpenseDbContext context)
    {
        _context = context;
    }

    public async Task<TravelCurrency?> GetByIdAsync(long requestNumber, int serialNumber, CancellationToken ct = default)
    {
        return await _context.TravelCurrencies
            .FirstOrDefaultAsync(c => c.RequestNumber == requestNumber && c.SerialNumber == serialNumber, ct);
    }

    public async Task<IReadOnlyList<TravelCurrency>> GetByRequestNumberAsync(long requestNumber, CancellationToken ct = default)
    {
        return await _context.TravelCurrencies
            .Where(c => c.RequestNumber == requestNumber)
            .ToListAsync(ct);
    }

    public async Task<TravelCurrency> AddAsync(TravelCurrency currency, CancellationToken ct = default)
    {
        _context.TravelCurrencies.Add(currency);
        await _context.SaveChangesAsync(ct);
        return currency;
    }

    public async Task UpdateAsync(TravelCurrency currency, CancellationToken ct = default)
    {
        _context.TravelCurrencies.Update(currency);
        await _context.SaveChangesAsync(ct);
    }
}
