using FaqServices.Domain.Entities;
using FaqServices.Domain.Interfaces;
using FaqServices.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FaqServices.Infrastructure.Repositories;

public class FaqGradeRepository : IFaqGradeRepository
{
    private readonly FaqDbContext _context;

    public FaqGradeRepository(FaqDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<FaqGrade?> GetByIdAsync(string id, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        return await _context.FaqGrades
            .Include(g => g.Questions)
            .FirstOrDefaultAsync(g => g.PK == id && !g.IsDeleted, ct);
    }

    public async Task<IEnumerable<FaqGrade>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.FaqGrades
            .Where(g => !g.IsDeleted)
            .OrderBy(g => g.SortOrder)
            .ThenBy(g => g.GradeName)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<FaqGrade>> GetActiveAsync(CancellationToken ct = default)
    {
        return await _context.FaqGrades
            .Where(g => g.IsActive && !g.IsDeleted)
            .OrderBy(g => g.SortOrder)
            .ThenBy(g => g.GradeName)
            .ToListAsync(ct);
    }

    public async Task AddAsync(FaqGrade grade, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(grade);
        await _context.FaqGrades.AddAsync(grade, ct);
    }

    public void Update(FaqGrade grade)
    {
        ArgumentNullException.ThrowIfNull(grade);
        _context.FaqGrades.Update(grade);
    }

    public void Remove(FaqGrade grade)
    {
        ArgumentNullException.ThrowIfNull(grade);
        _context.FaqGrades.Remove(grade);
    }

    public async Task<bool> ExistsAsync(string id, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        return await _context.FaqGrades.AnyAsync(g => g.PK == id && !g.IsDeleted, ct);
    }
}
