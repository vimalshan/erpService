using Microsoft.EntityFrameworkCore;
using RecruitmentService.Domain.Entities;
using RecruitmentService.Domain.Interfaces;
using RecruitmentService.Domain.ValueObjects;
using RecruitmentService.Infrastructure.Persistence;

namespace RecruitmentService.Infrastructure.Persistence.Repositories;

public class VacancyRepository : IVacancyRepository
{
    private readonly RecruitmentDbContext _context;

    public VacancyRepository(RecruitmentDbContext context) => _context = context;

    public async Task<Vacancy?> GetByIdAsync(decimal vacancyId, CancellationToken ct = default)
        => await _context.Vacancies.FirstOrDefaultAsync(v => v.VacancyId == vacancyId, ct);

    public async Task<IEnumerable<Vacancy>> GetAllOpenAsync(CancellationToken ct = default)
        => await _context.Vacancies
            .Where(v => v.LiveStatus == VacancyStatus.Open)
            .OrderByDescending(v => v.PostedDate)
            .ToListAsync(ct);

    public async Task<IEnumerable<Vacancy>> GetByUnitAsync(string unit, CancellationToken ct = default)
        => await _context.Vacancies
            .Where(v => v.VacancyUnit == unit)
            .OrderByDescending(v => v.PostedDate)
            .ToListAsync(ct);

    public async Task AddAsync(Vacancy vacancy, CancellationToken ct = default)
        => await _context.Vacancies.AddAsync(vacancy, ct);

    public void Update(Vacancy vacancy)
        => _context.Vacancies.Update(vacancy);

    public async Task<bool> ExistsAsync(decimal vacancyId, CancellationToken ct = default)
        => await _context.Vacancies.AnyAsync(v => v.VacancyId == vacancyId, ct);
}
