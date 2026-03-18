using FaqServices.Domain.Entities;
using FaqServices.Domain.Interfaces;
using FaqServices.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FaqServices.Infrastructure.Repositories;

public class FaqAnswerRepository : IFaqAnswerRepository
{
    private readonly FaqDbContext _context;

    public FaqAnswerRepository(FaqDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<FaqAnswer?> GetByIdAsync(string id, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        return await _context.FaqAnswers
            .Include(a => a.Question)
            .FirstOrDefaultAsync(a => a.PK == id && !a.IsDeleted, ct);
    }

    public async Task<IEnumerable<FaqAnswer>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.FaqAnswers
            .Include(a => a.Question)
            .Where(a => !a.IsDeleted)
            .OrderBy(a => a.SortOrder)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<FaqAnswer>> GetByQuestionIdAsync(string questionId, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(questionId);
        return await _context.FaqAnswers
            .Where(a => a.QuestionId == questionId && !a.IsDeleted)
            .OrderBy(a => a.SortOrder)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<FaqAnswer>> GetActiveByQuestionIdAsync(string questionId, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(questionId);
        return await _context.FaqAnswers
            .Where(a => a.QuestionId == questionId && a.IsActive && !a.IsDeleted)
            .OrderBy(a => a.SortOrder)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<FaqAnswer>> GetCorrectAnswersForQuestionAsync(string questionId, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(questionId);
        return await _context.FaqAnswers
            .Where(a => a.QuestionId == questionId && a.IsCorrect && a.IsActive && !a.IsDeleted)
            .OrderBy(a => a.SortOrder)
            .ToListAsync(ct);
    }

    public async Task AddAsync(FaqAnswer answer, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(answer);
        await _context.FaqAnswers.AddAsync(answer, ct);
    }

    public void Update(FaqAnswer answer)
    {
        ArgumentNullException.ThrowIfNull(answer);
        _context.FaqAnswers.Update(answer);
    }

    public void Remove(FaqAnswer answer)
    {
        ArgumentNullException.ThrowIfNull(answer);
        _context.FaqAnswers.Remove(answer);
    }

    public async Task<bool> ExistsAsync(string id, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        return await _context.FaqAnswers.AnyAsync(a => a.PK == id && !a.IsDeleted, ct);
    }
}
