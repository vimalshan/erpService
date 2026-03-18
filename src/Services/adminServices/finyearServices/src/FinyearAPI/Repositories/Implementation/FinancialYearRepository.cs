using Microsoft.EntityFrameworkCore;
using FinyearAPI.Data;
using FinyearAPI.Domain.Entities;
using FinyearAPI.Repositories.Interfaces;

namespace FinyearAPI.Repositories.Implementation
{
    /// <summary>
    /// Generic Repository implementation with EF Core
    /// </summary>
    public abstract class RepositoryBase<T> : IRepository<T> where T : class
    {
        protected readonly AdminDbContext _context;

        protected RepositoryBase(AdminDbContext context)
        {
            _context = context;
        }

        public virtual async Task<T?> GetByIdAsync(long id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(long id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
                return false;

            _context.Set<T>().Remove(entity);
            return true;
        }

        public virtual async Task<bool> ExistsAsync(long id)
        {
            return await _context.Set<T>().FindAsync(id) != null;
        }
    }

    /// <summary>
    /// FinancialYear Repository - EF Core Implementation
    /// </summary>
    public class FinancialYearRepository : RepositoryBase<FinancialYearMaster>, IFinancialYearRepository
    {
        public FinancialYearRepository(AdminDbContext context)
            : base(context)
        {
        }

        public async Task<FinancialYearMaster?> GetCurrentFinancialYearAsync()
        {
            return await _context.FinancialYearMasters
                .Where(fy => fy.StartDate <= DateTime.Now && fy.CloseDate >= DateTime.Now)
                .FirstOrDefaultAsync();
        }

        public async Task<FinancialYearMaster?> GetByNameAsync(string financialYearName)
        {
            return await _context.FinancialYearMasters
                .Where(fy => fy.FinancialYearName == financialYearName)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<FinancialYearMaster>> GetActiveFinancialYearsAsync()
        {
            return await _context.FinancialYearMasters
                .Where(fy => fy.StartDate <= DateTime.Now && fy.CloseDate >= DateTime.Now)
                .ToListAsync();
        }

        public async Task<IEnumerable<FinancialYearMaster>> GetFinancialYearsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.FinancialYearMasters
                .Where(fy => fy.StartDate >= startDate && fy.CloseDate <= endDate)
                .ToListAsync();
        }
    }
}
