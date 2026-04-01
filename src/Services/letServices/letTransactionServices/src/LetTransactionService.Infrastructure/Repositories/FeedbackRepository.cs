using LetTransactionService.Domain.Entities;
using LetTransactionService.Domain.Interfaces;
using LetTransactionService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LetTransactionService.Infrastructure.Repositories;

public class FeedbackRepository(LetTransactionDbContext context) : IFeedbackRepository
{
    public async Task<CourseFeedbackMain?> GetByIdAsync(long feedbackNumber, CancellationToken ct = default)
        => await context.CourseFeedbackMain
            .Include(f => f.FeedbackDetails)
            .FirstOrDefaultAsync(f => f.FeedbackNumber == feedbackNumber, ct);

    public async Task<IEnumerable<CourseFeedbackMain>> GetByNominationAsync(long nominationNumber, CancellationToken ct = default)
        => await context.CourseFeedbackMain
            .Include(f => f.FeedbackDetails)
            .Where(f => f.NominationNumber == nominationNumber)
            .OrderByDescending(f => f.FeedbackDate)
            .ToListAsync(ct);

    public async Task<IEnumerable<CourseFeedbackMain>> GetAllAsync(int page, int pageSize, CancellationToken ct = default)
        => await context.CourseFeedbackMain
            .Include(f => f.FeedbackDetails)
            .OrderByDescending(f => f.FeedbackDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task AddAsync(CourseFeedbackMain feedback, CancellationToken ct = default)
    {
        await context.CourseFeedbackMain.AddAsync(feedback, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(CourseFeedbackMain feedback, CancellationToken ct = default)
    {
        context.CourseFeedbackMain.Update(feedback);
        await context.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsAsync(long feedbackNumber, CancellationToken ct = default)
        => await context.CourseFeedbackMain.AnyAsync(f => f.FeedbackNumber == feedbackNumber, ct);
}
