using RecruitmentService.Domain.Entities;

namespace RecruitmentService.Domain.Interfaces;

public interface IProspectRepository
{
    Task<Prospect?> GetByIdAsync(decimal userId, CancellationToken ct = default);
    Task<Prospect?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<IEnumerable<Prospect>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(Prospect prospect, CancellationToken ct = default);
    void Update(Prospect prospect);
    Task<bool> EmailExistsAsync(string email, CancellationToken ct = default);
}
