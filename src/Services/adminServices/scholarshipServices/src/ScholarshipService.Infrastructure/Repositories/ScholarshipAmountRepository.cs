using Microsoft.EntityFrameworkCore;
using ScholarshipService.Domain.Entities;
using ScholarshipService.Domain.Repositories;
using ScholarshipService.Infrastructure.Data;

namespace ScholarshipService.Infrastructure.Repositories;

public class ScholarshipAmountRepository(ScholarshipDbContext context) : IScholarshipAmountRepository
{
    public async Task<ScholarshipAmount?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        => await context.ScholarshipAmounts.FindAsync([id], cancellationToken);

    public async Task<IEnumerable<ScholarshipAmount>> GetAllAsync(CancellationToken cancellationToken = default)
        => await context.ScholarshipAmounts.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<ScholarshipAmount?> GetEligibleAmountAsync(string gradeCategory, string eligibleExam, int year, CancellationToken cancellationToken = default)
        => await context.ScholarshipAmounts
            .Where(x => x.GradeCategory == gradeCategory
                     && x.EligibleExam == eligibleExam
                     && x.FromYear <= year
                     && (x.CloseYear == null || x.CloseYear >= year))
            .OrderByDescending(x => x.FromYear)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<long> GetNextIdAsync(CancellationToken cancellationToken = default)
    {
        var maxId = await context.ScholarshipAmounts.MaxAsync(x => (long?)x.Id, cancellationToken);
        return (maxId ?? 0L) + 1L;
    }

    public async Task AddAsync(ScholarshipAmount amount, CancellationToken cancellationToken = default)
        => await context.ScholarshipAmounts.AddAsync(amount, cancellationToken);

    public Task UpdateAsync(ScholarshipAmount amount, CancellationToken cancellationToken = default)
    {
        context.ScholarshipAmounts.Update(amount);
        return Task.CompletedTask;
    }
}
