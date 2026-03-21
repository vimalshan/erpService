using ConfigService.Domain.Entities;
using ConfigService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ConfigService.Infrastructure.Persistence.Repositories;

public class VendorRepository(ConfigDbContext context) : EfRepository<Vendor, string>(context), IVendorRepository
{
    public async Task<Vendor?> GetWithDetailsAsync(string id, CancellationToken ct = default) =>
        await DbSet.Include(v => v.TaxRates).Include(v => v.UnitMaps).Include(v => v.Charges)
            .FirstOrDefaultAsync(v => v.Id == id, ct);

    public async Task<IReadOnlyList<Vendor>> GetActiveVendorsAsync(CancellationToken ct = default) =>
        await DbSet.AsNoTracking().Where(v => v.ActiveStatus == "Active").ToListAsync(ct);
}

public class GradeCatExpenseRuleRepository(ConfigDbContext context) : EfRepository<GradeCatExpenseRule, string>(context), IGradeCatExpenseRuleRepository
{
    public async Task<GradeCatExpenseRule?> GetWithBreaksAsync(string id, CancellationToken ct = default) =>
        await DbSet.Include(r => r.Breaks).FirstOrDefaultAsync(r => r.Id == id, ct);

    public async Task<IReadOnlyList<GradeCatExpenseRule>> GetByGradeCategoryAsync(string gradeCategory, CancellationToken ct = default) =>
        await DbSet.AsNoTracking().Where(r => r.GradeCategory == gradeCategory).ToListAsync(ct);
}

public class GradeCatModeMapRepository(ConfigDbContext context) : EfRepository<GradeCatModeMap, string>(context), IGradeCatModeMapRepository { }
public class GradeCatStayRuleRepository(ConfigDbContext context) : EfRepository<GradeCatStayRule, string>(context), IGradeCatStayRuleRepository { }
public class GradeCatExpenseMapRepository(ConfigDbContext context) : EfRepository<GradeCatExpenseMap, string>(context), IGradeCatExpenseMapRepository { }
public class GradeTypeTravelParamRepository(ConfigDbContext context) : EfRepository<GradeTypeTravelParam, string>(context), IGradeTypeTravelParamRepository { }
