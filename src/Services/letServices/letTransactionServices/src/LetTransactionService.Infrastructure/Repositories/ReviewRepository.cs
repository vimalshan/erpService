using LetTransactionService.Domain.Entities;
using LetTransactionService.Domain.Interfaces;
using LetTransactionService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LetTransactionService.Infrastructure.Repositories;

public class ReviewRepository(LetTransactionDbContext context) : IReviewRepository
{
    public async Task<ReviewMain?> GetByIdAsync(long reviewSerialNumber, CancellationToken ct = default)
        => await context.ReviewMain
            .Include(r => r.ReviewDetails)
            .FirstOrDefaultAsync(r => r.ReviewSerialNumber == reviewSerialNumber, ct);

    public async Task<IEnumerable<ReviewMain>> GetByFeedbackAsync(long feedbackNumber, CancellationToken ct = default)
        => await context.ReviewMain
            .Include(r => r.ReviewDetails)
            .Where(r => r.FeedbackNumber == feedbackNumber)
            .OrderByDescending(r => r.NextReviewDate)
            .ToListAsync(ct);

    public async Task<IEnumerable<ReviewMain>> GetAllAsync(int page, int pageSize, CancellationToken ct = default)
        => await context.ReviewMain
            .Include(r => r.ReviewDetails)
            .OrderByDescending(r => r.NextReviewDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task<IEnumerable<ReviewMain>> GetPendingReviewsAsync(CancellationToken ct = default)
        => await context.ReviewMain
            .Include(r => r.ReviewDetails)
            .Where(r => r.Status == 'N')
            .OrderBy(r => r.NextReviewDate)
            .ToListAsync(ct);

    public async Task AddAsync(ReviewMain review, CancellationToken ct = default)
    {
        await context.ReviewMain.AddAsync(review, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(ReviewMain review, CancellationToken ct = default)
    {
        context.ReviewMain.Update(review);
        await context.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsAsync(long reviewSerialNumber, CancellationToken ct = default)
        => await context.ReviewMain.AnyAsync(r => r.ReviewSerialNumber == reviewSerialNumber, ct);
}
