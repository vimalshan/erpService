namespace FeedbackService.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using Domain.Aggregates;
using Persistence;
using Application.Commands.Handlers;

/// <summary>
/// Repository for Feedback aggregate
/// </summary>
public class FeedbackRepository : IFeedbackRepository
{
    private readonly FeedbackDbContext _context;

    /// <summary>
    /// Initializes a new instance of the FeedbackRepository class
    /// </summary>
    public FeedbackRepository(FeedbackDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets the unit of work
    /// </summary>
    public IUnitOfWork UnitOfWork => _context;

    /// <summary>
    /// Adds a feedback to the repository
    /// </summary>
    public async Task AddAsync(Feedback feedback, CancellationToken cancellationToken = default)
    {
        await _context.Feedbacks.AddAsync(feedback, cancellationToken);
    }

    /// <summary>
    /// Gets a feedback by ID
    /// </summary>
    public async Task<Feedback?> GetByIdAsync(decimal id, CancellationToken cancellationToken = default)
    {
        return await _context.Feedbacks
            .Include(f => f.Items)
            .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
    }

    /// <summary>
    /// Gets all feedback
    /// </summary>
    public async Task<List<Feedback>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Feedbacks
            .Include(f => f.Items)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Gets feedback by request number
    /// </summary>
    public async Task<List<Feedback>> GetByRequestNoAsync(decimal requestNo, CancellationToken cancellationToken = default)
    {
        return await _context.Feedbacks
            .Where(f => f.RequestNo == requestNo)
            .Include(f => f.Items)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Updates a feedback
    /// </summary>
    public async Task UpdateAsync(Feedback feedback, CancellationToken cancellationToken = default)
    {
        _context.Feedbacks.Update(feedback);
        await Task.CompletedTask;
    }

    /// <summary>
    /// Deletes a feedback
    /// </summary>
    public async Task DeleteAsync(decimal id, CancellationToken cancellationToken = default)
    {
        var feedback = await GetByIdAsync(id, cancellationToken);
        if (feedback != null)
        {
            _context.Feedbacks.Remove(feedback);
        }
    }
}
