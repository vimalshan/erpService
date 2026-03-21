using ComplaintService.Application.Interfaces;
using ComplaintService.Domain.Entities;
using ComplaintService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ComplaintService.Infrastructure.Repositories;

public class ComplaintGroupRepository(ComplaintDbContext dbContext) : IComplaintGroupRepository
{
    public async Task<ComplaintGroup?> GetByIdAsync(string groupId, CancellationToken ct = default) =>
        await dbContext.ComplaintGroups.FirstOrDefaultAsync(g => g.GroupId == groupId, ct);

    public async Task<IEnumerable<ComplaintGroup>> GetAllAsync(CancellationToken ct = default) =>
        await dbContext.ComplaintGroups.OrderBy(g => g.GroupName).ToListAsync(ct);

    public async Task AddAsync(ComplaintGroup group, CancellationToken ct = default) =>
        await dbContext.ComplaintGroups.AddAsync(group, ct);

    public Task UpdateAsync(ComplaintGroup group, CancellationToken ct = default)
    {
        dbContext.ComplaintGroups.Update(group);
        return Task.CompletedTask;
    }
}
