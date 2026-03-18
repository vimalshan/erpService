using FaqServices.Domain.Entities;
using FaqServices.Domain.Interfaces;
using FaqServices.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FaqServices.Infrastructure.Repositories;

public class FaqQuestionRepository : IFaqQuestionRepository
{
    private readonly FaqDbContext _context;

    public FaqQuestionRepository(FaqDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<FaqQuestion?> GetByIdAsync(string id, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        return await _context.FaqQuestions
            .Include(q => q.Grade)
            .FirstOrDefaultAsync(q => q.PK == id && !q.IsDeleted, ct);
    }

    public async Task<FaqQuestion?> GetByIdWithAnswersAsync(string id, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        return await _context.FaqQuestions
            .Include(q => q.Answers)
            .Include(q => q.Grade)
            .FirstOrDefaultAsync(q => q.PK == id && !q.IsDeleted, ct);
    }

    public async Task<IEnumerable<FaqQuestion>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.FaqQuestions
            .Include(q => q.Grade)
            .Include(q => q.Answers)
            .Where(q => !q.IsDeleted)
            .OrderBy(q => q.SortOrder)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<FaqQuestion>> GetByGradeIdAsync(string gradeId, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(gradeId);
        return await _context.FaqQuestions
            .Include(q => q.Answers)
            .Where(q => q.GradeId == gradeId && !q.IsDeleted)
            .OrderBy(q => q.SortOrder)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<FaqQuestion>> GetActiveByGradeIdAsync(string gradeId, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(gradeId);
        return await _context.FaqQuestions
            .Include(q => q.Answers.Where(a => a.IsActive && !a.IsDeleted))
            .Where(q => q.GradeId == gradeId && q.IsActive && !q.IsDeleted)
            .OrderBy(q => q.SortOrder)
            .ToListAsync(ct);
    }

    public async Task AddAsync(FaqQuestion question, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(question);
        await _context.FaqQuestions.AddAsync(question, ct);
    }

    public void Update(FaqQuestion question)
    {
        ArgumentNullException.ThrowIfNull(question);
        _context.FaqQuestions.Update(question);
    }

    public void Remove(FaqQuestion question)
    {
        ArgumentNullException.ThrowIfNull(question);
        _context.FaqQuestions.Remove(question);
    }

    public async Task<bool> ExistsAsync(string id, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        return await _context.FaqQuestions.AnyAsync(q => q.PK == id && !q.IsDeleted, ct);
    }
}
