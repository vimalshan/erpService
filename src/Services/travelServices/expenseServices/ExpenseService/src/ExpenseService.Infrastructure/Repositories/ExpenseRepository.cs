using ExpenseService.Domain.Entities;
using ExpenseService.Domain.Interfaces;
using ExpenseService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseService.Infrastructure.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly ExpenseDbContext _context;

    public ExpenseRepository(ExpenseDbContext context)
    {
        _context = context;
    }

    public async Task<TravelExpense?> GetByIdAsync(long requestNumber, long serialNumber, CancellationToken ct = default)
    {
        return await _context.TravelExpenses
            .Include(e => e.Allocations)
            .Include(e => e.SubDetails)
            .FirstOrDefaultAsync(e => e.RequestNumber == requestNumber && e.SerialNumber == serialNumber, ct);
    }

    public async Task<IReadOnlyList<TravelExpense>> GetByRequestNumberAsync(long requestNumber, CancellationToken ct = default)
    {
        return await _context.TravelExpenses
            .Include(e => e.Allocations)
            .Include(e => e.SubDetails)
            .Where(e => e.RequestNumber == requestNumber)
            .ToListAsync(ct);
    }

    public async Task<TravelExpense> AddAsync(TravelExpense expense, CancellationToken ct = default)
    {
        _context.TravelExpenses.Add(expense);
        await _context.SaveChangesAsync(ct);
        return expense;
    }

    public async Task UpdateAsync(TravelExpense expense, CancellationToken ct = default)
    {
        _context.TravelExpenses.Update(expense);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(long requestNumber, long serialNumber, CancellationToken ct = default)
    {
        var expense = await GetByIdAsync(requestNumber, serialNumber, ct);
        if (expense != null)
        {
            _context.TravelExpenses.Remove(expense);
            await _context.SaveChangesAsync(ct);
        }
    }
}
