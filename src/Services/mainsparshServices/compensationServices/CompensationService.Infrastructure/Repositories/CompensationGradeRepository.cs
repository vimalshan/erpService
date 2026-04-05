using Microsoft.EntityFrameworkCore;
using CompensationService.Domain.Entities;
using CompensationService.Domain.Repositories;
using CompensationService.Infrastructure.Persistence;

namespace CompensationService.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for CompensationGrade
/// </summary>
public class CompensationGradeRepository : ICompensationGradeRepository
{
    private readonly CompensationDbContext _dbContext;

    public CompensationGradeRepository(CompensationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CompensationGrade?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.CompensationGrades
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<CompensationGrade?> GetByCodeAsync(string gradeCode, CancellationToken cancellationToken = default)
    {
        return await _dbContext.CompensationGrades
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.GradeCode.Value == gradeCode, cancellationToken);
    }

    public async Task<IEnumerable<CompensationGrade>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.CompensationGrades
            .AsNoTracking()
            .OrderBy(x => x.GradeLevel)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CompensationGrade>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.CompensationGrades
            .AsNoTracking()
            .Where(x => x.Status.Value == 'A')
            .OrderBy(x => x.GradeLevel)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(CompensationGrade grade, CancellationToken cancellationToken = default)
    {
        await _dbContext.CompensationGrades.AddAsync(grade, cancellationToken);
    }

    public async Task UpdateAsync(CompensationGrade grade, CancellationToken cancellationToken = default)
    {
        _dbContext.CompensationGrades.Update(grade);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var grade = await _dbContext.CompensationGrades.FindAsync(new object[] { id }, cancellationToken: cancellationToken);
        if (grade != null)
        {
            _dbContext.CompensationGrades.Remove(grade);
        }
    }
}
