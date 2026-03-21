using InsuranceService.Domain.Entities;
using InsuranceService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InsuranceService.Infrastructure.Persistence;

public class TravelInsuranceRepository : ITravelInsuranceRepository
{
    private readonly InsuranceDbContext _context;

    public TravelInsuranceRepository(InsuranceDbContext context)
    {
        _context = context;
    }

    public async Task<TravelInsurance?> GetByKeyAsync(string companyCode, long planNumber, CancellationToken cancellationToken = default)
    {
        return await _context.TravelInsurances
            .FirstOrDefaultAsync(x => x.CompanyCode == new Domain.ValueObjects.CompanyCode(companyCode)
                                   && x.PlanNumber == planNumber, cancellationToken);
    }

    public async Task<IReadOnlyList<TravelInsurance>> GetAllAsync(string? companyCode = null, CancellationToken cancellationToken = default)
    {
        var query = _context.TravelInsurances.AsQueryable();

        if (!string.IsNullOrEmpty(companyCode))
            query = query.Where(x => x.CompanyCode == new Domain.ValueObjects.CompanyCode(companyCode));

        return await query.OrderByDescending(x => x.UpdateDate).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(TravelInsurance insurance, CancellationToken cancellationToken = default)
    {
        await _context.TravelInsurances.AddAsync(insurance, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(TravelInsurance insurance, CancellationToken cancellationToken = default)
    {
        _context.TravelInsurances.Update(insurance);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(string companyCode, long planNumber, CancellationToken cancellationToken = default)
    {
        var entity = await GetByKeyAsync(companyCode, planNumber, cancellationToken);
        if (entity is not null)
        {
            _context.TravelInsurances.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
