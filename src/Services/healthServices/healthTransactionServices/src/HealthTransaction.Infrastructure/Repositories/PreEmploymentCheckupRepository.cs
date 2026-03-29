using HealthTransaction.Domain.Entities;
using HealthTransaction.Domain.Interfaces;
using HealthTransaction.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HealthTransaction.Infrastructure.Repositories;

public class PreEmploymentCheckupRepository : IPreEmploymentCheckupRepository
{
    private readonly HealthTransactionDbContext _context;
    public PreEmploymentCheckupRepository(HealthTransactionDbContext context) => _context = context;

    public async Task<PreEmploymentCheckup?> GetByKeyAsync(decimal empNum, string comCode, CancellationToken cancellationToken = default)
        => await _context.PreEmploymentCheckups.FindAsync(new object[] { empNum, comCode }, cancellationToken);

    public async Task<IReadOnlyList<PreEmploymentCheckup>> GetByEmployeeNumAsync(decimal empNum, CancellationToken cancellationToken = default)
        => await _context.PreEmploymentCheckups.Where(x => x.EmpNum == empNum).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<PreEmploymentCheckup>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.PreEmploymentCheckups.ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<PreEmploymentCheckup>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default)
        => await _context.PreEmploymentCheckups
            .Where(x => x.CheckupDate >= from && x.CheckupDate <= to)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(PreEmploymentCheckup entity, CancellationToken cancellationToken = default)
        => await _context.PreEmploymentCheckups.AddAsync(entity, cancellationToken);

    public void Update(PreEmploymentCheckup entity) => _context.PreEmploymentCheckups.Update(entity);
    public void Remove(PreEmploymentCheckup entity) => _context.PreEmploymentCheckups.Remove(entity);
}
