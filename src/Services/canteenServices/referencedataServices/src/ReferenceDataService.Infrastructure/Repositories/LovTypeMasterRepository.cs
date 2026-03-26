using Microsoft.EntityFrameworkCore;
using ReferenceDataService.Domain.Entities;
using ReferenceDataService.Domain.Interfaces;
using ReferenceDataService.Infrastructure.Persistence;

namespace ReferenceDataService.Infrastructure.Repositories;

public class LovTypeMasterRepository : ILovTypeMasterRepository
{
    private readonly ReferenceDataDbContext _context;

    public LovTypeMasterRepository(ReferenceDataDbContext context)
    {
        _context = context;
    }

    public async Task<LovTypeMaster?> GetByCodeAsync(string lovTypeCode, CancellationToken cancellationToken = default)
    {
        return await _context.LovTypeMasters.FindAsync(new object[] { lovTypeCode }, cancellationToken);
    }

    public async Task<IEnumerable<LovTypeMaster>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.LovTypeMasters.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(LovTypeMaster entity, CancellationToken cancellationToken = default)
    {
        await _context.LovTypeMasters.AddAsync(entity, cancellationToken);
    }

    public void Update(LovTypeMaster entity)
    {
        _context.LovTypeMasters.Update(entity);
    }

    public void Delete(LovTypeMaster entity)
    {
        _context.LovTypeMasters.Remove(entity);
    }
}
