using LoanDefinition.Domain.Entities;
using LoanDefinition.Domain.Repositories;
using LoanDefinition.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LoanDefinition.Infrastructure.Repositories;

public class LoanTypeMasterRepository(LoanDefinitionDbContext context) : ILoanTypeMasterRepository
{
    public async Task<LoanTypeMaster?> GetByIdAsync(long id, CancellationToken ct = default)
        => await context.LoanTypeMasters.FindAsync([id], ct);

    public async Task<IReadOnlyList<LoanTypeMaster>> GetAllAsync(CancellationToken ct = default)
        => await context.LoanTypeMasters.AsNoTracking().ToListAsync(ct);

    public async Task<IReadOnlyList<LoanTypeMaster>> GetByCategoryAsync(string category, CancellationToken ct = default)
        => await context.LoanTypeMasters.AsNoTracking().Where(x => x.LoanCategory == category).ToListAsync(ct);

    public async Task<LoanTypeMaster> AddAsync(LoanTypeMaster entity, CancellationToken ct = default)
    {
        await context.LoanTypeMasters.AddAsync(entity, ct);
        return entity;
    }

    public Task UpdateAsync(LoanTypeMaster entity, CancellationToken ct = default)
    {
        context.LoanTypeMasters.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(LoanTypeMaster entity, CancellationToken ct = default)
    {
        context.LoanTypeMasters.Remove(entity);
        return Task.CompletedTask;
    }
}

public class LoanMasterRepository(LoanDefinitionDbContext context) : ILoanMasterRepository
{
    public async Task<LoanMaster?> GetByIdAsync(long id, CancellationToken ct = default)
        => await context.LoanMasters.FindAsync([id], ct);

    public async Task<LoanMaster?> GetWithDetailsAsync(long id, CancellationToken ct = default)
        => await context.LoanMasters
            .Include(x => x.LoanType)
            .Include(x => x.SubClasses)
            .Include(x => x.InterestRates)
            .Include(x => x.LimitRanges)
            .Include(x => x.FestivalMaps).ThenInclude(x => x.Festival)
            .FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<IReadOnlyList<LoanMaster>> GetAllAsync(CancellationToken ct = default)
        => await context.LoanMasters.AsNoTracking().Include(x => x.LoanType).ToListAsync(ct);

    public async Task<IReadOnlyList<LoanMaster>> GetByTypeAsync(long loanTypeId, CancellationToken ct = default)
        => await context.LoanMasters.AsNoTracking().Where(x => x.LoanTypeId == loanTypeId).Include(x => x.LoanType).ToListAsync(ct);

    public async Task<IReadOnlyList<LoanMaster>> GetActiveLoansAsync(CancellationToken ct = default)
        => await context.LoanMasters.AsNoTracking()
            .Where(x => x.ClosureDate == null || x.ClosureDate > DateTime.UtcNow)
            .Include(x => x.LoanType).ToListAsync(ct);

    public async Task<LoanMaster> AddAsync(LoanMaster entity, CancellationToken ct = default)
    {
        await context.LoanMasters.AddAsync(entity, ct);
        return entity;
    }

    public Task UpdateAsync(LoanMaster entity, CancellationToken ct = default)
    {
        context.LoanMasters.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(LoanMaster entity, CancellationToken ct = default)
    {
        context.LoanMasters.Remove(entity);
        return Task.CompletedTask;
    }
}

public class LoanSubClassRepository(LoanDefinitionDbContext context) : ILoanSubClassRepository
{
    public async Task<LoanSubClass?> GetByIdAsync(long id, CancellationToken ct = default)
        => await context.LoanSubClasses.FindAsync([id], ct);

    public async Task<IReadOnlyList<LoanSubClass>> GetAllAsync(CancellationToken ct = default)
        => await context.LoanSubClasses.AsNoTracking().ToListAsync(ct);

    public async Task<IReadOnlyList<LoanSubClass>> GetByLoanIdAsync(long loanId, CancellationToken ct = default)
        => await context.LoanSubClasses.AsNoTracking().Where(x => x.LoanId == loanId).ToListAsync(ct);

    public async Task<LoanSubClass> AddAsync(LoanSubClass entity, CancellationToken ct = default)
    {
        await context.LoanSubClasses.AddAsync(entity, ct);
        return entity;
    }

    public Task UpdateAsync(LoanSubClass entity, CancellationToken ct = default)
    {
        context.LoanSubClasses.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(LoanSubClass entity, CancellationToken ct = default)
    {
        context.LoanSubClasses.Remove(entity);
        return Task.CompletedTask;
    }
}

public class LoanInterestRateRepository(LoanDefinitionDbContext context) : ILoanInterestRateRepository
{
    public async Task<LoanInterestRateMaster?> GetByIdAsync(long id, CancellationToken ct = default)
        => await context.LoanInterestRates.FindAsync([id], ct);

    public async Task<IReadOnlyList<LoanInterestRateMaster>> GetAllAsync(CancellationToken ct = default)
        => await context.LoanInterestRates.AsNoTracking().ToListAsync(ct);

    public async Task<IReadOnlyList<LoanInterestRateMaster>> GetByLoanIdAsync(long loanId, CancellationToken ct = default)
        => await context.LoanInterestRates.AsNoTracking().Where(x => x.LoanId == loanId).ToListAsync(ct);

    public async Task<LoanInterestRateMaster?> GetEffectiveRateAsync(long loanId, DateTime asOfDate, CancellationToken ct = default)
        => await context.LoanInterestRates.AsNoTracking()
            .Where(x => x.LoanId == loanId && x.EffectiveDate <= asOfDate && (x.ClosureDate == null || x.ClosureDate > asOfDate))
            .OrderByDescending(x => x.EffectiveDate)
            .FirstOrDefaultAsync(ct);

    public async Task<LoanInterestRateMaster> AddAsync(LoanInterestRateMaster entity, CancellationToken ct = default)
    {
        await context.LoanInterestRates.AddAsync(entity, ct);
        return entity;
    }

    public Task UpdateAsync(LoanInterestRateMaster entity, CancellationToken ct = default)
    {
        context.LoanInterestRates.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(LoanInterestRateMaster entity, CancellationToken ct = default)
    {
        context.LoanInterestRates.Remove(entity);
        return Task.CompletedTask;
    }
}

public class LoanLimitRangeRepository(LoanDefinitionDbContext context) : ILoanLimitRangeRepository
{
    public async Task<LoanLimitRangeMaster?> GetByIdAsync(long id, CancellationToken ct = default)
        => await context.LoanLimitRanges.FindAsync([id], ct);

    public async Task<IReadOnlyList<LoanLimitRangeMaster>> GetAllAsync(CancellationToken ct = default)
        => await context.LoanLimitRanges.AsNoTracking().ToListAsync(ct);

    public async Task<IReadOnlyList<LoanLimitRangeMaster>> GetByLoanIdAsync(long loanId, CancellationToken ct = default)
        => await context.LoanLimitRanges.AsNoTracking().Where(x => x.LoanId == loanId).ToListAsync(ct);

    public async Task<LoanLimitRangeMaster> AddAsync(LoanLimitRangeMaster entity, CancellationToken ct = default)
    {
        await context.LoanLimitRanges.AddAsync(entity, ct);
        return entity;
    }

    public Task UpdateAsync(LoanLimitRangeMaster entity, CancellationToken ct = default)
    {
        context.LoanLimitRanges.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(LoanLimitRangeMaster entity, CancellationToken ct = default)
    {
        context.LoanLimitRanges.Remove(entity);
        return Task.CompletedTask;
    }
}

public class LoanPerquisiteRepository(LoanDefinitionDbContext context) : ILoanPerquisiteRepository
{
    public async Task<LoanPerquisite?> GetByIdAsync(long id, CancellationToken ct = default)
        => await context.LoanPerquisites.FindAsync([id], ct);

    public async Task<IReadOnlyList<LoanPerquisite>> GetAllAsync(CancellationToken ct = default)
        => await context.LoanPerquisites.AsNoTracking().ToListAsync(ct);

    public async Task<IReadOnlyList<LoanPerquisite>> GetByClassIdAsync(string classId, CancellationToken ct = default)
        => await context.LoanPerquisites.AsNoTracking().Where(x => x.ClassId == classId).ToListAsync(ct);

    public async Task<LoanPerquisite> AddAsync(LoanPerquisite entity, CancellationToken ct = default)
    {
        await context.LoanPerquisites.AddAsync(entity, ct);
        return entity;
    }

    public Task UpdateAsync(LoanPerquisite entity, CancellationToken ct = default)
    {
        context.LoanPerquisites.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(LoanPerquisite entity, CancellationToken ct = default)
    {
        context.LoanPerquisites.Remove(entity);
        return Task.CompletedTask;
    }
}

public class LoanFestivalRepository(LoanDefinitionDbContext context) : ILoanFestivalRepository
{
    public async Task<LoanFestival?> GetByIdAsync(long id, CancellationToken ct = default)
        => await context.LoanFestivals.FindAsync([id], ct);

    public async Task<IReadOnlyList<LoanFestival>> GetAllAsync(CancellationToken ct = default)
        => await context.LoanFestivals.AsNoTracking().ToListAsync(ct);

    public async Task<IReadOnlyList<LoanFestival>> GetActiveFestivalsAsync(DateTime asOfDate, CancellationToken ct = default)
        => await context.LoanFestivals.AsNoTracking()
            .Where(x => x.StartDate <= asOfDate && x.EndDate >= asOfDate).ToListAsync(ct);

    public async Task<LoanFestival> AddAsync(LoanFestival entity, CancellationToken ct = default)
    {
        await context.LoanFestivals.AddAsync(entity, ct);
        return entity;
    }

    public Task UpdateAsync(LoanFestival entity, CancellationToken ct = default)
    {
        context.LoanFestivals.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(LoanFestival entity, CancellationToken ct = default)
    {
        context.LoanFestivals.Remove(entity);
        return Task.CompletedTask;
    }
}

public class LoanFestivalMapRepository(LoanDefinitionDbContext context) : ILoanFestivalMapRepository
{
    public async Task<LoanFestivalMap?> GetByIdAsync(long id, CancellationToken ct = default)
        => await context.LoanFestivalMaps.FindAsync([id], ct);

    public async Task<IReadOnlyList<LoanFestivalMap>> GetAllAsync(CancellationToken ct = default)
        => await context.LoanFestivalMaps.AsNoTracking().ToListAsync(ct);

    public async Task<IReadOnlyList<LoanFestivalMap>> GetByLoanIdAsync(long loanId, CancellationToken ct = default)
        => await context.LoanFestivalMaps.AsNoTracking().Where(x => x.LoanId == loanId).Include(x => x.Festival).ToListAsync(ct);

    public async Task<LoanFestivalMap> AddAsync(LoanFestivalMap entity, CancellationToken ct = default)
    {
        await context.LoanFestivalMaps.AddAsync(entity, ct);
        return entity;
    }

    public Task UpdateAsync(LoanFestivalMap entity, CancellationToken ct = default)
    {
        context.LoanFestivalMaps.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(LoanFestivalMap entity, CancellationToken ct = default)
    {
        context.LoanFestivalMaps.Remove(entity);
        return Task.CompletedTask;
    }
}

public class LoanAccountMasterRepository(LoanDefinitionDbContext context) : ILoanAccountMasterRepository
{
    public async Task<LoanAccountMaster?> GetByIdAsync(long id, CancellationToken ct = default)
        => await context.LoanAccountMasters.FindAsync([id], ct);

    public async Task<IReadOnlyList<LoanAccountMaster>> GetAllAsync(CancellationToken ct = default)
        => await context.LoanAccountMasters.AsNoTracking().ToListAsync(ct);

    public async Task<IReadOnlyList<LoanAccountMaster>> GetByLoanTypeAsync(long loanType, CancellationToken ct = default)
        => await context.LoanAccountMasters.AsNoTracking().Where(x => x.LoanType == loanType).ToListAsync(ct);

    public async Task<LoanAccountMaster> AddAsync(LoanAccountMaster entity, CancellationToken ct = default)
    {
        await context.LoanAccountMasters.AddAsync(entity, ct);
        return entity;
    }

    public Task UpdateAsync(LoanAccountMaster entity, CancellationToken ct = default)
    {
        context.LoanAccountMasters.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(LoanAccountMaster entity, CancellationToken ct = default)
    {
        context.LoanAccountMasters.Remove(entity);
        return Task.CompletedTask;
    }
}
