using MasterService.Domain.Entities;

namespace MasterService.Domain.Interfaces;

public interface ISkillRepository
{
    Task<Skill?> GetByCodeAsync(long skillCode, CancellationToken ct = default);
    Task<IEnumerable<Skill>> GetAllAsync(char? skillType = null, CancellationToken ct = default);
    Task AddAsync(Skill skill, CancellationToken ct = default);
    Task UpdateAsync(Skill skill, CancellationToken ct = default);
    Task<bool> ExistsAsync(long skillCode, CancellationToken ct = default);
}
