using Microsoft.EntityFrameworkCore;
using TaskServices.Domain.Entities;
using TaskServices.Domain.Repositories;
using TaskServices.Infrastructure.Persistence;

namespace TaskServices.Infrastructure.Repositories;

public class TaskMailRepository : ITaskMailRepository
{
    private readonly TaskDbContext _context;

    public TaskMailRepository(TaskDbContext context)
    {
        _context = context;
    }

    public async Task<TaskMail?> GetByIdAsync(decimal mid, CancellationToken cancellationToken = default)
    {
        return await _context.TaskMails.FirstOrDefaultAsync(t => t.MID == mid, cancellationToken);
    }

    public async Task<IReadOnlyList<TaskMail>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.TaskMails.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<TaskMail>> GetBySystemUserIdAsync(decimal sysId, CancellationToken cancellationToken = default)
    {
        return await _context.TaskMails.Where(t => t.SYSID == sysId).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(TaskMail taskMail, CancellationToken cancellationToken = default)
    {
        await _context.TaskMails.AddAsync(taskMail, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(TaskMail taskMail, CancellationToken cancellationToken = default)
    {
        _context.TaskMails.Update(taskMail);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(decimal mid, CancellationToken cancellationToken = default)
    {
        var entity = await _context.TaskMails.FirstOrDefaultAsync(t => t.MID == mid, cancellationToken);
        if (entity is not null)
        {
            _context.TaskMails.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(decimal mid, CancellationToken cancellationToken = default)
    {
        return await _context.TaskMails.AnyAsync(t => t.MID == mid, cancellationToken);
    }
}
