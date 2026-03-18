using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AccidentManagementService.Domain.Entities;
using AccidentManagementService.Domain.Repositories;

namespace AccidentManagementService.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Generic repository base class for all entities
    /// </summary>
    public abstract class GenericRepository<TEntity, TKey> where TEntity : DomainEntity
    {
        protected readonly AccidentManagementDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        protected GenericRepository(AccidentManagementDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual async Task<TEntity?> GetByIdAsync(long id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public virtual async Task<TEntity?> GetByGuidAsync(Guid guid)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Guid == guid && !x.IsDeleted);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.Where(x => !x.IsDeleted).ToListAsync();
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(long id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
                return false;

            entity.Delete();
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> RestoreAsync(long id)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted);
            if (entity == null)
                return false;

            entity.Restore();
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }

    /// <summary>
    /// Repository implementation for AccidentReport
    /// </summary>
    public class AccidentReportRepository : GenericRepository<AccidentReport, long>, IAccidentReportRepository
    {
        public AccidentReportRepository(AccidentManagementDbContext context) : base(context) { }

        public async Task<AccidentReport?> GetByAccidentNumberAsync(long accidentNumber)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.AccidentNumber.Value == accidentNumber && !x.IsDeleted);
        }

        public async Task<IEnumerable<AccidentReport>> GetByCompanyCodeAsync(string companyCode)
        {
            return await _dbSet
                .Where(x => x.CompanyCode == companyCode && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<AccidentReport>> GetByStatusAsync(long statusId)
        {
            return await _dbSet
                .Where(x => x.StatusId == statusId && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<AccidentReport>> GetBySeverityAsync(long severityId)
        {
            return await _dbSet
                .Where(x => x.SeverityId == severityId && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<AccidentReport>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, string? companyCode = null)
        {
            var query = _dbSet.Where(x =>
                x.AccidentCircumstances.AccidentDateTime >= startDate &&
                x.AccidentCircumstances.AccidentDateTime <= endDate &&
                !x.IsDeleted);

            if (!string.IsNullOrEmpty(companyCode))
                query = query.Where(x => x.CompanyCode == companyCode);

            return await query.OrderByDescending(x => x.AccidentCircumstances.AccidentDateTime).ToListAsync();
        }

        public async Task<IEnumerable<AccidentReport>> GetByEmployeeNumberAsync(string employeeNumber)
        {
            return await _dbSet
                .Where(x => x.EmployeeInfo != null && x.EmployeeInfo.EmployeeNumber == employeeNumber && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }
    }

    /// <summary>
    /// Repository implementation for InjuryCategory
    /// </summary>
    public class InjuryCategoryRepository : GenericRepository<InjuryCategory, long>, IInjuryCategoryRepository
    {
        public InjuryCategoryRepository(AccidentManagementDbContext context) : base(context) { }
    }

    /// <summary>
    /// Repository implementation for InjuryNature
    /// </summary>
    public class InjuryNatureRepository : GenericRepository<InjuryNature, long>, IInjuryNatureRepository
    {
        public InjuryNatureRepository(AccidentManagementDbContext context) : base(context) { }
    }

    /// <summary>
    /// Repository implementation for AccidentSeverity
    /// </summary>
    public class AccidentSeverityRepository : GenericRepository<AccidentSeverity, long>, IAccidentSeverityRepository
    {
        public AccidentSeverityRepository(AccidentManagementDbContext context) : base(context) { }

        public async Task<AccidentSeverity?> GetByCodeAsync(string code)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Code == code && !x.IsDeleted);
        }
    }

    /// <summary>
    /// Repository implementation for AccidentStatus
    /// </summary>
    public class AccidentStatusRepository : GenericRepository<AccidentStatus, long>, IAccidentStatusRepository
    {
        public AccidentStatusRepository(AccidentManagementDbContext context) : base(context) { }

        public async Task<AccidentStatus?> GetByCodeAsync(string code)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Code == code && !x.IsDeleted);
        }
    }

    /// <summary>
    /// Repository implementation for Contractor
    /// </summary>
    public class ContractorRepository : GenericRepository<Contractor, long>, IContractorRepository
    {
        public ContractorRepository(AccidentManagementDbContext context) : base(context) { }

        public async Task<Contractor?> GetByContractorIdAsync(long contractorId)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.ContractorId == contractorId && !x.IsDeleted);
        }

        public async Task<IEnumerable<Contractor>> GetActiveAsync()
        {
            return await _dbSet
                .Where(x => x.Status == ContractorStatusEnum.Active && !x.IsDeleted)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
    }

    /// <summary>
    /// Repository implementation for InjuredPerson
    /// </summary>
    public class InjuredPersonRepository : GenericRepository<InjuredPerson, long>, IInjuredPersonRepository
    {
        public InjuredPersonRepository(AccidentManagementDbContext context) : base(context) { }

        public async Task<InjuredPerson?> GetBySerialNumberAsync(long serialNumber)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.SerialNumber == serialNumber && !x.IsDeleted);
        }
    }

    /// <summary>
    /// Unit of Work implementation
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AccidentManagementDbContext _context;
        private IAccidentReportRepository? _accidentReports;
        private IInjuryCategoryRepository? _injuryCategories;
        private IInjuryNatureRepository? _injuryNatures;
        private IAccidentSeverityRepository? _accidentSeverities;
        private IAccidentStatusRepository? _accidentStatuses;
        private IContractorRepository? _contractors;
        private IInjuredPersonRepository? _injuredPersons;

        public UnitOfWork(AccidentManagementDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IAccidentReportRepository AccidentReports =>
            _accidentReports ??= new AccidentReportRepository(_context);

        public IInjuryCategoryRepository InjuryCategories =>
            _injuryCategories ??= new InjuryCategoryRepository(_context);

        public IInjuryNatureRepository InjuryNatures =>
            _injuryNatures ??= new InjuryNatureRepository(_context);

        public IAccidentSeverityRepository AccidentSeverities =>
            _accidentSeverities ??= new AccidentSeverityRepository(_context);

        public IAccidentStatusRepository AccidentStatuses =>
            _accidentStatuses ??= new AccidentStatusRepository(_context);

        public IContractorRepository Contractors =>
            _contractors ??= new ContractorRepository(_context);

        public IInjuredPersonRepository InjuredPersons =>
            _injuredPersons ??= new InjuredPersonRepository(_context);

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();
            }
            catch
            {
                await _context.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task RollbackAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
