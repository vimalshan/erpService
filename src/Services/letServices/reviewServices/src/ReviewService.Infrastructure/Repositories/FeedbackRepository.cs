using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ReviewService.Domain.Entities;
using ReviewService.Domain.Interfaces;
using ReviewService.Infrastructure.Data;

namespace ReviewService.Infrastructure.Repositories;

public class FeedbackRepository : IFeedbackRepository
{
    private readonly ReviewDbContext _context;
    private readonly string _connectionString;

    public FeedbackRepository(ReviewDbContext context, IConfiguration configuration)
    {
        _context = context;
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    public async Task<CourseFeedMain?> GetByCompositeKeyAsync(
        string userId, long courseId, CancellationToken cancellationToken = default)
        => await _context.CourseFeedMains.FindAsync([userId, courseId], cancellationToken);

    public async Task<IEnumerable<CourseFeedMain>> GetByCourseIdAsync(
        long courseId, CancellationToken cancellationToken = default)
        => await _context.CourseFeedMains
            .AsNoTracking()
            .Where(f => f.FdCrsId == courseId)
            .OrderByDescending(f => f.FdRevDat)
            .ToListAsync(cancellationToken);

    /// <summary>
    /// Uses Dapper to call the usp_Review_GetFeedbackSummary stored procedure.
    /// </summary>
    public async Task<(int TotalFeedbacks, decimal AverageRating)> GetFeedbackSummaryAsync(
        long courseId, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        var result = await connection.QuerySingleOrDefaultAsync<FeedbackSummaryResult>(
            "usp_Review_GetFeedbackSummary",
            new { p_CourseID = courseId },
            commandType: System.Data.CommandType.StoredProcedure);

        return result is null ? (0, 0m) : (result.TotalFeedbacks, result.AverageRating ?? 0m);
    }

    public async Task AddAsync(CourseFeedMain feedback, CancellationToken cancellationToken = default)
        => await _context.CourseFeedMains.AddAsync(feedback, cancellationToken);

    public async Task AddSubAsync(CourseFeedSub feedSub, CancellationToken cancellationToken = default)
        => await _context.CourseFeedSubs.AddAsync(feedSub, cancellationToken);

    public Task UpdateAsync(CourseFeedMain feedback, CancellationToken cancellationToken = default)
    {
        _context.CourseFeedMains.Update(feedback);
        return Task.CompletedTask;
    }

    private sealed class FeedbackSummaryResult
    {
        public int TotalFeedbacks { get; set; }
        public decimal? AverageRating { get; set; }
    }
}
