using Microsoft.EntityFrameworkCore;
using TrainingDevelopment.Domain.Entities;
using TrainingDevelopment.Domain.Interfaces;

namespace TrainingDevelopment.Infrastructure.Repositories;

public class ProgramLovRepository : IProgramLovRepository
{
    private readonly Data.ApplicationDbContext _context;

    public ProgramLovRepository(Data.ApplicationDbContext context) => _context = context;

    public async Task<ProgramLovMaster?> GetByTypeCodeAsync(string typeCode, CancellationToken cancellationToken = default)
        => await _context.ProgramLovMasters.FirstOrDefaultAsync(x => x.TypeCode == typeCode, cancellationToken);

    public async Task<IEnumerable<ProgramLovMaster>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.ProgramLovMasters.ToListAsync(cancellationToken);

    public async Task AddAsync(ProgramLovMaster entity, CancellationToken cancellationToken = default)
        => await _context.ProgramLovMasters.AddAsync(entity, cancellationToken);

    public void Update(ProgramLovMaster entity)
        => _context.ProgramLovMasters.Update(entity);

    public void Delete(ProgramLovMaster entity)
        => _context.ProgramLovMasters.Remove(entity);
}
