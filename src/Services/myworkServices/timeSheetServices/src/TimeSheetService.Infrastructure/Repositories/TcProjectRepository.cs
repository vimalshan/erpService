using Microsoft.EntityFrameworkCore;
using TimeSheetService.Domain.Entities;
using TimeSheetService.Domain.Interfaces;
using TimeSheetService.Infrastructure.Persistence;

namespace TimeSheetService.Infrastructure.Repositories;

public class TcProjectRepository : ITcProjectRepository
{
    private readonly TimeSheetDbContext _context;

    public TcProjectRepository(TimeSheetDbContext context) => _context = context;

    public async Task<TcProject?> GetByIdAsync(long projectId, CancellationToken cancellationToken = default)
        => await _context.TcProjects.FindAsync([projectId], cancellationToken);

    public async Task<IReadOnlyList<TcProject>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.TcProjects.OrderBy(p => p.ProjectName).ToListAsync(cancellationToken);

    public async Task<TcProject> AddAsync(TcProject project, CancellationToken cancellationToken = default)
    {
        await _context.TcProjects.AddAsync(project, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return project;
    }

    public async Task UpdateAsync(TcProject project, CancellationToken cancellationToken = default)
    {
        _context.TcProjects.Update(project);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
