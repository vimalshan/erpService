using Microsoft.EntityFrameworkCore;
using TimeSheetService.Domain.Entities;
using TimeSheetService.Domain.Interfaces;
using TimeSheetService.Infrastructure.Persistence;

namespace TimeSheetService.Infrastructure.Repositories;

public class TsProjectRepository : ITsProjectRepository
{
    private readonly TimeSheetDbContext _context;

    public TsProjectRepository(TimeSheetDbContext context) => _context = context;

    public async Task<TsProject?> GetByCodeAsync(string projectCode, CancellationToken cancellationToken = default)
        => await _context.TsProjects.FindAsync([projectCode], cancellationToken);

    public async Task<IReadOnlyList<TsProject>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.TsProjects.OrderBy(p => p.ProjectName).ToListAsync(cancellationToken);

    public async Task<TsProject> AddAsync(TsProject project, CancellationToken cancellationToken = default)
    {
        await _context.TsProjects.AddAsync(project, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return project;
    }
}
