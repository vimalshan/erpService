using Microsoft.EntityFrameworkCore;
using ReviewService.Domain.Entities;
using ReviewService.Domain.Interfaces;
using ReviewService.Infrastructure.Data;

namespace ReviewService.Infrastructure.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly ReviewDbContext _context;

    public ReviewRepository(ReviewDbContext context) => _context = context;

    public async Task<ReviewMain?> GetByIdAsync(long srlNum, CancellationToken cancellationToken = default)
        => await _context.ReviewMains.FindAsync([srlNum], cancellationToken);

    public async Task<IEnumerable<ReviewMain>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.ReviewMains.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<IEnumerable<ReviewSub>> GetSubsByMainIdAsync(long mainSrl, CancellationToken cancellationToken = default)
        => await _context.ReviewSubs.AsNoTracking()
            .Where(s => s.RevMainSrl == mainSrl)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(ReviewMain review, CancellationToken cancellationToken = default)
        => await _context.ReviewMains.AddAsync(review, cancellationToken);

    public async Task AddSubAsync(ReviewSub reviewSub, CancellationToken cancellationToken = default)
        => await _context.ReviewSubs.AddAsync(reviewSub, cancellationToken);

    public Task UpdateAsync(ReviewMain review, CancellationToken cancellationToken = default)
    {
        _context.ReviewMains.Update(review);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(long srlNum, CancellationToken cancellationToken = default)
        => await _context.ReviewMains.AnyAsync(r => r.RevSrlNum == srlNum, cancellationToken);
}
