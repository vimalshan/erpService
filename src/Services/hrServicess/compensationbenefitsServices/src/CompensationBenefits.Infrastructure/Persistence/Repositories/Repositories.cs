using CompensationBenefits.Domain.Entities;
using CompensationBenefits.Domain.Interfaces;
using CompensationBenefits.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CompensationBenefits.Infrastructure.Persistence.Repositories;

public class SalaryRepository(CompensationBenefitsDbContext context)
    : RepositoryBase<SalaryMain>(context), ISalaryRepository
{
    public async Task<SalaryMain?> GetWithDetailsAsync(long salaryId, CancellationToken ct = default)
        => await _context.SalaryMains
            .Include(s => s.Details)
            .FirstOrDefaultAsync(s => s.SalaryId == salaryId, ct);

    public async Task<IEnumerable<SalaryMain>> GetByStructureIdAsync(long structureId, CancellationToken ct = default)
        => await _context.SalaryMains
            .Where(s => s.SalaryStructureId == structureId)
            .ToListAsync(ct);
}

public class SalaryStructureRepository(CompensationBenefitsDbContext context)
    : RepositoryBase<SalaryStructureMain>(context), ISalaryStructureRepository
{
    public async Task<SalaryStructureMain?> GetWithDetailsAsync(long structureId, CancellationToken ct = default)
        => await _context.SalaryStructureMains
            .Include(s => s.Details)
            .FirstOrDefaultAsync(s => s.StructureId == structureId, ct);

    public async Task<IEnumerable<SalaryStructureMain>> GetByUnitIdAsync(long unitId, CancellationToken ct = default)
        => await _context.SalaryStructureMains
            .Where(s => s.StructureUnitId == unitId)
            .ToListAsync(ct);
}

public class MediclaimRepository(CompensationBenefitsDbContext context)
    : RepositoryBase<MediclaimMaster>(context), IMediclaimRepository
{
    public async Task<MediclaimMaster?> GetWithDetailsAsync(long mediclaimId, CancellationToken ct = default)
        => await _context.MediclaimMasters
            .Include(m => m.YearlyPremiums)
            .FirstOrDefaultAsync(m => m.MediclaimId == mediclaimId, ct);
}

public class MobileConnectionRepository(CompensationBenefitsDbContext context)
    : RepositoryBase<MobileConnection>(context), IMobileConnectionRepository
{
    public async Task<IEnumerable<MobileConnection>> GetByEmployeeAsync(long empSysId, CancellationToken ct = default)
        => await _context.MobileConnections
            .Where(c => c.ConnEmpSysId == empSysId)
            .ToListAsync(ct);
}

public class RetiralRangeMasterRepository(CompensationBenefitsDbContext context)
    : RepositoryBase<RetiralRangeMaster>(context), IRetiralRangeMasterRepository
{
    public async Task<IEnumerable<RetiralRangeMaster>> GetByUnitIdAsync(long unitId, CancellationToken ct = default)
        => await _context.RetiralRangeMasters
            .Where(r => r.RrMastUnitId == unitId)
            .ToListAsync(ct);
}
