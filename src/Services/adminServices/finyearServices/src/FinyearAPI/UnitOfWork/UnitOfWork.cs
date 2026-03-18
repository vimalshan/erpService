using Microsoft.EntityFrameworkCore.Storage;
using FinyearAPI.Data;
using FinyearAPI.Models;
using FinyearAPI.Repositories.Implementation;
using FinyearAPI.Repositories.Interfaces;
using FinyearAPI.Repositories.Dapper;

namespace FinyearAPI.UnitOfWork
{
    /// <summary>
    /// Unit of Work Implementation
    /// Manages repositories and transactions
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AdminDbContext _context;
        private readonly IFinancialYearDapperRepository _financialYearDapperRepository;
        private IDbContextTransaction? _transaction;
        private IFinancialYearRepository? _financialYearRepository;

        public UnitOfWork(
            AdminDbContext context,
            IFinancialYearDapperRepository financialYearDapperRepository)
        {
            _context = context;
            _financialYearDapperRepository = financialYearDapperRepository;
        }

        public IFinancialYearRepository FinancialYearRepository
        {
            get
            {
                _financialYearRepository ??= new FinancialYearRepository(_context);
                return _financialYearRepository;
            }
        }

        public IFinancialYearDapperRepository FinancialYearDapperRepository => _financialYearDapperRepository;

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
            return _transaction != null;
        }

        public async Task<bool> CommitAsync()
        {
            try
            {
                await SaveChangesAsync();
                await _transaction?.CommitAsync()!;
                return true;
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                }
                _transaction = null;
            }
        }

        public async Task<bool> RollbackAsync()
        {
            try
            {
                if (_transaction != null)
                {
                    await _transaction.RollbackAsync();
                }
                return true;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                }
                _transaction = null;
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
            }
            await _context.DisposeAsync();
        }
    }
}
