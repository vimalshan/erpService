using RecruitmentService.Domain.Entities;

namespace RecruitmentService.Domain.Interfaces;

public interface IVacancyRepository
{
    Task<Vacancy?> GetByIdAsync(decimal vacancyId, CancellationToken ct = default);
    Task<IEnumerable<Vacancy>> GetAllOpenAsync(CancellationToken ct = default);
    Task<IEnumerable<Vacancy>> GetByUnitAsync(string unit, CancellationToken ct = default);
    Task AddAsync(Vacancy vacancy, CancellationToken ct = default);
    void Update(Vacancy vacancy);
    Task<bool> ExistsAsync(decimal vacancyId, CancellationToken ct = default);
}
