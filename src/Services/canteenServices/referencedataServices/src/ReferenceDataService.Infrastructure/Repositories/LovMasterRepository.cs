using Microsoft.EntityFrameworkCore;
using ReferenceDataService.Domain.Entities;
using ReferenceDataService.Domain.Interfaces;
using ReferenceDataService.Infrastructure.Persistence;

namespace ReferenceDataService.Infrastructure.Repositories;

public class LovMasterRepository : ILovMasterRepository
{
    private readonly ReferenceDataDbContext _context;

    public LovMasterRepository(ReferenceDataDbContext context)
    {
        _context = context;
    }

    public async Task<LovMaster?> GetByIdAsync(string lovId, CancellationToken cancellationToken = default)
    {
        return await _context.LovMasters.FindAsync(new object[] { lovId }, cancellationToken);
    }

    public async Task<IEnumerable<LovMaster>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.LovMasters.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<LovMaster>> GetByTypeAsync(string lovType, CancellationToken cancellationToken = default)
    {
        return await _context.LovMasters
            .Where(x => x.LovType == lovType)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(LovMaster entity, CancellationToken cancellationToken = default)
    {
        await _context.LovMasters.AddAsync(entity, cancellationToken);
    }

    public void Update(LovMaster entity)
    {
        _context.LovMasters.Update(entity);
    }

    public void Delete(LovMaster entity)
    {
        _context.LovMasters.Remove(entity);
    }
}
