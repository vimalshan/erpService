using HealthTransaction.Domain.Entities;
using HealthTransaction.Domain.Interfaces;
using HealthTransaction.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HealthTransaction.Infrastructure.Repositories;

public class DynamicHealthDetailRepository : IDynamicHealthDetailRepository
{
    private readonly HealthTransactionDbContext _context;
    public DynamicHealthDetailRepository(HealthTransactionDbContext context) => _context = context;

    public async Task<IReadOnlyList<DynamicHealthDetail>> GetByHlthNumAsync(decimal hlthNum, CancellationToken cancellationToken = default)
        => await _context.DynamicHealthDetails.Where(d => d.HlthNum == hlthNum).ToListAsync(cancellationToken);

    public async Task<DynamicHealthDetail?> GetByKeyAsync(decimal hlthNum, string chkupCod, string comCode, decimal ctrlSrcId, CancellationToken cancellationToken = default)
        => await _context.DynamicHealthDetails.FindAsync(new object[] { hlthNum, chkupCod, comCode, ctrlSrcId }, cancellationToken);

    public async Task<IReadOnlyList<DynamicHealthDetail>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.DynamicHealthDetails.ToListAsync(cancellationToken);

    public async Task AddAsync(DynamicHealthDetail entity, CancellationToken cancellationToken = default)
        => await _context.DynamicHealthDetails.AddAsync(entity, cancellationToken);

    public async Task AddRangeAsync(IEnumerable<DynamicHealthDetail> entities, CancellationToken cancellationToken = default)
        => await _context.DynamicHealthDetails.AddRangeAsync(entities, cancellationToken);

    public void Update(DynamicHealthDetail entity) => _context.DynamicHealthDetails.Update(entity);
    public void Remove(DynamicHealthDetail entity) => _context.DynamicHealthDetails.Remove(entity);
}
