using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MobileExpenseManagement.Application.Common.Interfaces;
using MobileExpenseManagement.Domain.Entities;
using MobileExpenseManagement.Infrastructure.Data;

namespace MobileExpenseManagement.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Expense entity using Entity Framework
/// </summary>
public class ExpenseRepository : IExpenseRepository
{
    private readonly ExpenseDbContext _context;
    private readonly ILogger<ExpenseRepository> _logger;

    public ExpenseRepository(ExpenseDbContext context, ILogger<ExpenseRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task AddAsync(Expense expense, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Expenses.AddAsync(expense, cancellationToken);
            _logger.LogInformation($"Expense added to context: {expense.Id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding expense to repository");
            throw;
        }
    }

    public async Task<Expense?> GetByIdAsync(decimal expenseId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Expenses
                .Include(e => e.Files)
                .FirstOrDefaultAsync(e => e.Id == expenseId && !e.IsDeleted, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting expense by ID: {expenseId}");
            throw;
        }
    }

    public async Task<List<Expense>> GetByTripIdAsync(decimal tripId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Expenses
                .Where(e => e.TripId == tripId && !e.IsDeleted)
                .Include(e => e.Files)
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting expenses by trip ID: {tripId}");
            throw;
        }
    }

    public async Task<(List<Expense>, int)> GetByTripIdPaginatedAsync(decimal tripId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _context.Expenses
                .Where(e => e.TripId == tripId && !e.IsDeleted)
                .Include(e => e.Files)
                .OrderByDescending(e => e.ExpenseDate);

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting paginated expenses by trip ID: {tripId}");
            throw;
        }
    }

    public async Task<List<ExpenseFile>> GetExpenseFilesAsync(decimal expenseId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.ExpenseFiles
                .Where(f => f.ExpenseId == expenseId && !f.IsDeleted)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting expense files: {expenseId}");
            throw;
        }
    }

    public async Task<List<Expense>> SearchByDateRangeAsync(DateTime startDate, DateTime endDate, decimal? tripId, decimal? categoryId, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _context.Expenses
                .Where(e => e.ExpenseDate >= startDate && e.ExpenseDate <= endDate && !e.IsDeleted);

            if (tripId.HasValue && tripId > 0)
                query = query.Where(e => e.TripId == tripId);

            if (categoryId.HasValue && categoryId > 0)
                query = query.Where(e => e.CategoryId == categoryId);

            return await query
                .Include(e => e.Files)
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching expenses by date range");
            throw;
        }
    }

    public async Task UpdateAsync(Expense expense, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Expenses.Update(expense);
            _logger.LogInformation($"Expense updated in context: {expense.Id}");
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating expense in repository");
            throw;
        }
    }

    public async Task DeleteAsync(decimal expenseId, CancellationToken cancellationToken = default)
    {
        try
        {
            var expense = await GetByIdAsync(expenseId, cancellationToken);
            if (expense != null)
            {
                _context.Expenses.Remove(expense);
                _logger.LogInformation($"Expense removed from context: {expenseId}");
            }
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting expense: {expenseId}");
            throw;
        }
    }
}
