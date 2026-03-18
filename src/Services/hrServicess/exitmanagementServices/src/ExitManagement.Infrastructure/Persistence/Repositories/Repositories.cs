using ExitManagement.Domain.Entities;
using ExitManagement.Domain.Interfaces;
using ExitManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExitManagement.Infrastructure.Persistence.Repositories;

public class EmployeeExitRepository : IEmployeeExitRepository
{
    private readonly ApplicationDbContext _context;

    public EmployeeExitRepository(ApplicationDbContext context) => _context = context;

    public async Task<EmployeeExit?> GetByIdAsync(decimal exitNo, CancellationToken ct = default)
        => await _context.EmployeeExits.FirstOrDefaultAsync(e => e.ExitNo == exitNo, ct);

    public async Task<IEnumerable<EmployeeExit>> GetAllAsync(CancellationToken ct = default)
        => await _context.EmployeeExits.ToListAsync(ct);

    public async Task<IEnumerable<EmployeeExit>> GetByEmployeeAsync(decimal employeeSysId, CancellationToken ct = default)
        => await _context.EmployeeExits.Where(e => e.EmployeeSysId == employeeSysId).ToListAsync(ct);

    public async Task AddAsync(EmployeeExit entity, CancellationToken ct = default)
    {
        await _context.EmployeeExits.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(EmployeeExit entity, CancellationToken ct = default)
    {
        _context.EmployeeExits.Update(entity);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(decimal exitNo, CancellationToken ct = default)
    {
        var entity = await GetByIdAsync(exitNo, ct);
        if (entity is not null)
        {
            _context.EmployeeExits.Remove(entity);
            await _context.SaveChangesAsync(ct);
        }
    }
}

public class ExitInterviewFeedbackRepository : IExitInterviewFeedbackRepository
{
    private readonly ApplicationDbContext _context;

    public ExitInterviewFeedbackRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<ExitInterviewFeedback>> GetByExitNoAsync(decimal exitNo, CancellationToken ct = default)
        => await _context.ExitInterviewFeedbacks.Where(f => f.ExitNo == exitNo).ToListAsync(ct);

    public async Task AddAsync(ExitInterviewFeedback entity, CancellationToken ct = default)
    {
        await _context.ExitInterviewFeedbacks.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(ExitInterviewFeedback entity, CancellationToken ct = default)
    {
        _context.ExitInterviewFeedbacks.Update(entity);
        await _context.SaveChangesAsync(ct);
    }
}

public class ExitQuestionRepository : IExitQuestionRepository
{
    private readonly ApplicationDbContext _context;

    public ExitQuestionRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<ExitQuestion>> GetAllAsync(CancellationToken ct = default)
        => await _context.ExitQuestions.ToListAsync(ct);

    public async Task<ExitQuestion?> GetByIdAsync(string questionId, CancellationToken ct = default)
        => await _context.ExitQuestions.FirstOrDefaultAsync(q => q.QuestionId == questionId, ct);

    public async Task AddAsync(ExitQuestion entity, CancellationToken ct = default)
    {
        await _context.ExitQuestions.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(ExitQuestion entity, CancellationToken ct = default)
    {
        _context.ExitQuestions.Update(entity);
        await _context.SaveChangesAsync(ct);
    }
}

public class ExitInterviewQuestionRepository : IExitInterviewQuestionRepository
{
    private readonly ApplicationDbContext _context;

    public ExitInterviewQuestionRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<ExitInterviewQuestion>> GetAllAsync(CancellationToken ct = default)
        => await _context.ExitInterviewQuestions.ToListAsync(ct);

    public async Task<ExitInterviewQuestion?> GetByIdAsync(string questionId, CancellationToken ct = default)
        => await _context.ExitInterviewQuestions.FirstOrDefaultAsync(q => q.QuestionId == questionId, ct);
}

public class ExitResponsibilityMapRepository : IExitResponsibilityMapRepository
{
    private readonly ApplicationDbContext _context;

    public ExitResponsibilityMapRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<ExitResponsibilityMap>> GetByEmployeeAsync(decimal employeeSysId, CancellationToken ct = default)
        => await _context.ExitResponsibilityMaps.Where(r => r.EmployeeSysId == employeeSysId).ToListAsync(ct);

    public async Task AddAsync(ExitResponsibilityMap entity, CancellationToken ct = default)
    {
        await _context.ExitResponsibilityMaps.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);
    }
}
