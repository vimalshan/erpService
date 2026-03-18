using Microsoft.EntityFrameworkCore;
using TrainingDevelopment.Domain.Entities;
using TrainingDevelopment.Domain.Interfaces;

namespace TrainingDevelopment.Infrastructure.Repositories;

public class InstituteMasterRepository : IInstituteMasterRepository
{
    private readonly Data.ApplicationDbContext _context;

    public InstituteMasterRepository(Data.ApplicationDbContext context) => _context = context;

    public async Task<InstituteMaster?> GetByCodeAsync(decimal code, CancellationToken cancellationToken = default)
        => await _context.InstituteMasters.FirstOrDefaultAsync(x => x.InstituteCode == code, cancellationToken);

    public async Task<IEnumerable<InstituteMaster>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.InstituteMasters.ToListAsync(cancellationToken);

    public async Task AddAsync(InstituteMaster entity, CancellationToken cancellationToken = default)
        => await _context.InstituteMasters.AddAsync(entity, cancellationToken);

    public void Update(InstituteMaster entity)
        => _context.InstituteMasters.Update(entity);

    public void Delete(InstituteMaster entity)
        => _context.InstituteMasters.Remove(entity);
}
