using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MobileExpenseManagement.Application.Common.Interfaces;
using MobileExpenseManagement.Infrastructure.Repositories;

namespace MobileExpenseManagement.Infrastructure.Data;

/// <summary>
/// Unit of Work implementation
/// </summary>
public partial class UnitOfWork : IUnitOfWork
{
    private readonly ExpenseDbContext _context;
    private readonly ILogger<UnitOfWork> _logger;
    private IExpenseRepository? _expenseRepository;

    public UnitOfWork(ExpenseDbContext context, ILogger<UnitOfWork> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IExpenseRepository Expenses =>
        _expenseRepository ??= new ExpenseRepository(_context, LoggerFactory.CreateLogger<ExpenseRepository>());

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"Changes saved: {result} records affected");
            return result;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database update error");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving changes");
            throw;
        }
    }

    public async Task<bool> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Database.BeginTransactionAsync(cancellationToken);
            _logger.LogInformation("Transaction started");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error beginning transaction");
            return false;
        }
    }

    public async Task<bool> CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Database.CommitTransactionAsync(cancellationToken);
            _logger.LogInformation("Transaction committed");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error committing transaction");
            return false;
        }
    }

    public async Task<bool> RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Database.RollbackTransactionAsync(cancellationToken);
            _logger.LogInformation("Transaction rolled back");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rolling back transaction");
            return false;
        }
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}

// Extension to UnitOfWork for logger factory
public partial class UnitOfWork
{
    private static ILoggerFactory? _loggerFactory;

    internal static void SetLoggerFactory(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    private static ILoggerFactory LoggerFactory => _loggerFactory ?? new Microsoft.Extensions.Logging.LoggerFactory();
}
