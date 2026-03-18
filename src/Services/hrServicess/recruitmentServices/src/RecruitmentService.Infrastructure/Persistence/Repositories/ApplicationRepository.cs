using Microsoft.EntityFrameworkCore;
using RecruitmentService.Domain.Entities;
using RecruitmentService.Domain.Interfaces;
using RecruitmentService.Infrastructure.Persistence;

namespace RecruitmentService.Infrastructure.Persistence.Repositories;

public class ApplicationRepository : IApplicationRepository
{
    private readonly RecruitmentDbContext _context;

    public ApplicationRepository(RecruitmentDbContext context) => _context = context;

    public async Task<ApplicationHistory?> GetByIdAsync(decimal appId, CancellationToken ct = default)
        => await _context.ApplicationHistories
            .Include(a => a.Qualifications)
            .Include(a => a.Trainings)
            .FirstOrDefaultAsync(a => a.AppId == appId, ct);

    public async Task<IEnumerable<ApplicationHistory>> GetByVacancyIdAsync(decimal vacancyId, CancellationToken ct = default)
        => await _context.ApplicationHistories
            .Include(a => a.Qualifications)
            .Include(a => a.Trainings)
            .Where(a => a.AppVacancyId == vacancyId)
            .OrderByDescending(a => a.UpdatedOn)
            .ToListAsync(ct);

    public async Task<IEnumerable<ApplicationHistory>> GetByProspectAsync(decimal userId, CancellationToken ct = default)
        => await _context.ApplicationHistories
            .Include(a => a.Qualifications)
            .Include(a => a.Trainings)
            .Where(a => a.UpdatedBy == userId)
            .OrderByDescending(a => a.UpdatedOn)
            .ToListAsync(ct);

    public async Task AddAsync(ApplicationHistory application, CancellationToken ct = default)
        => await _context.ApplicationHistories.AddAsync(application, ct);

    public void Update(ApplicationHistory application)
        => _context.ApplicationHistories.Update(application);
}
