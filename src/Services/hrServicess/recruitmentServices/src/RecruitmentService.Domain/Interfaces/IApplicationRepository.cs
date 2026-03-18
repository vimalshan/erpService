using RecruitmentService.Domain.Entities;

namespace RecruitmentService.Domain.Interfaces;

public interface IApplicationRepository
{
    Task<ApplicationHistory?> GetByIdAsync(decimal appId, CancellationToken ct = default);
    Task<IEnumerable<ApplicationHistory>> GetByVacancyIdAsync(decimal vacancyId, CancellationToken ct = default);
    Task<IEnumerable<ApplicationHistory>> GetByProspectAsync(decimal userId, CancellationToken ct = default);
    Task AddAsync(ApplicationHistory application, CancellationToken ct = default);
    void Update(ApplicationHistory application);
}
