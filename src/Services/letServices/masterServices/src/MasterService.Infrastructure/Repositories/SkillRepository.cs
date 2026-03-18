using MasterService.Domain.Entities;
using MasterService.Domain.Interfaces;
using MasterService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MasterService.Infrastructure.Repositories;

public sealed class SkillRepository(ApplicationDbContext context) : ISkillRepository
{
    public async Task<Skill?> GetByCodeAsync(long skillCode, CancellationToken ct = default)
        => await context.Skills.FindAsync([skillCode], ct);

    public async Task<IEnumerable<Skill>> GetAllAsync(char? skillType = null, CancellationToken ct = default)
    {
        var query = context.Skills.AsQueryable();
        if (skillType.HasValue)
            query = query.Where(s => s.SkillType == char.ToUpper(skillType.Value));
        return await query.OrderBy(s => s.SkillName).ToListAsync(ct);
    }

    public async Task AddAsync(Skill skill, CancellationToken ct = default)
    {
        await context.Skills.AddAsync(skill, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Skill skill, CancellationToken ct = default)
    {
        context.Skills.Update(skill);
        await context.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsAsync(long skillCode, CancellationToken ct = default)
        => await context.Skills.AnyAsync(s => s.SkillCode == skillCode, ct);
}
