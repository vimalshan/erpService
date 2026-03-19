using Microsoft.EntityFrameworkCore;
using ProblemManagement.Domain.Entities;
using ProblemManagement.Domain.Interfaces;
using ProblemManagement.Infrastructure.Data;

namespace ProblemManagement.Infrastructure.Repositories;

public class ProblemRepository(ProblemManagementDbContext context) : IProblemRepository
{
    public async Task<ProblemMain?> GetByIdAsync(long id, CancellationToken ct = default) =>
        await context.Problems
            .Include(p => p.Solutions).ThenInclude(s => s.SolutionComments)
            .Include(p => p.Solutions).ThenInclude(s => s.SolutionApprovals)
            .Include(p => p.Attachments)
            .Include(p => p.Approvals)
            .Include(p => p.Audiences)
            .FirstOrDefaultAsync(p => p.PrId == id, ct);

    public async Task<IReadOnlyList<ProblemMain>> GetByStatusAsync(char status, CancellationToken ct = default) =>
        await context.Problems
            .Where(p => p.PrStatus == status)
            .OrderByDescending(p => p.PrEnteredOn)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<ProblemMain>> GetAllAsync(CancellationToken ct = default) =>
        await context.Problems
            .OrderByDescending(p => p.PrEnteredOn)
            .ToListAsync(ct);

    public async Task<ProblemMain> AddAsync(ProblemMain problem, CancellationToken ct = default)
    {
        await context.Problems.AddAsync(problem, ct);
        return problem;
    }

    public Task UpdateAsync(ProblemMain problem, CancellationToken ct = default)
    {
        context.Problems.Update(problem);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long id, CancellationToken ct = default)
    {
        var problem = await context.Problems.FindAsync([id], ct);
        if (problem is not null)
            context.Problems.Remove(problem);
    }
}

public class ProblemSolutionRepository(ProblemManagementDbContext context) : IProblemSolutionRepository
{
    public async Task<ProblemSolution?> GetByIdAsync(long id, CancellationToken ct = default) =>
        await context.ProblemSolutions
            .Include(s => s.SolutionComments)
            .Include(s => s.SolutionApprovals)
            .FirstOrDefaultAsync(s => s.SolId == id, ct);

    public async Task<IReadOnlyList<ProblemSolution>> GetByProblemIdAsync(long problemId, CancellationToken ct = default) =>
        await context.ProblemSolutions
            .Where(s => s.SolPrId == problemId)
            .Include(s => s.SolutionComments)
            .OrderByDescending(s => s.SolEnteredOn)
            .ToListAsync(ct);

    public async Task<ProblemSolution> AddAsync(ProblemSolution solution, CancellationToken ct = default)
    {
        await context.ProblemSolutions.AddAsync(solution, ct);
        return solution;
    }

    public Task UpdateAsync(ProblemSolution solution, CancellationToken ct = default)
    {
        context.ProblemSolutions.Update(solution);
        return Task.CompletedTask;
    }
}

public class ProblemApprovalRepository(ProblemManagementDbContext context) : IProblemApprovalRepository
{
    public async Task<ProblemApproval> AddAsync(ProblemApproval approval, CancellationToken ct = default)
    {
        await context.ProblemApprovals.AddAsync(approval, ct);
        return approval;
    }

    public async Task<IReadOnlyList<ProblemApproval>> GetByProblemIdAsync(long problemId, CancellationToken ct = default) =>
        await context.ProblemApprovals.Where(a => a.PrAppPrId == problemId).ToListAsync(ct);
}

public class SolutionApprovalRepository(ProblemManagementDbContext context) : ISolutionApprovalRepository
{
    public async Task<SolutionApproval> AddAsync(SolutionApproval approval, CancellationToken ct = default)
    {
        await context.SolutionApprovals.AddAsync(approval, ct);
        return approval;
    }

    public async Task<IReadOnlyList<SolutionApproval>> GetBySolutionIdAsync(long solutionId, CancellationToken ct = default) =>
        await context.SolutionApprovals.Where(a => a.SolAppSolId == solutionId).ToListAsync(ct);
}

public class SolutionCommentRepository(ProblemManagementDbContext context) : ISolutionCommentRepository
{
    public async Task<SolutionComment> AddAsync(SolutionComment comment, CancellationToken ct = default)
    {
        await context.SolutionComments.AddAsync(comment, ct);
        return comment;
    }

    public async Task<IReadOnlyList<SolutionComment>> GetBySolutionIdAsync(long solutionId, CancellationToken ct = default) =>
        await context.SolutionComments.Where(c => c.SolCommentSolId == solutionId).OrderByDescending(c => c.SolCommentOn).ToListAsync(ct);
}

public class ProblemFunctionRepository(ProblemManagementDbContext context) : IProblemFunctionRepository
{
    public async Task<IReadOnlyList<ProblemFunction>> GetAllAsync(CancellationToken ct = default) =>
        await context.ProblemFunctions.ToListAsync(ct);

    public async Task<ProblemFunction?> GetByIdAsync(long id, CancellationToken ct = default) =>
        await context.ProblemFunctions.FindAsync([id], ct);
}

public class ProblemImpactRepository(ProblemManagementDbContext context) : IProblemImpactRepository
{
    public async Task<IReadOnlyList<ProblemImpact>> GetAllAsync(CancellationToken ct = default) =>
        await context.ProblemImpacts.ToListAsync(ct);

    public async Task<ProblemImpact?> GetByIdAsync(long id, CancellationToken ct = default) =>
        await context.ProblemImpacts.FindAsync([id], ct);
}

public class ProblemAttachmentRepository(ProblemManagementDbContext context) : IProblemAttachmentRepository
{
    public async Task<ProblemAttachment> AddAsync(ProblemAttachment attachment, CancellationToken ct = default)
    {
        await context.ProblemAttachments.AddAsync(attachment, ct);
        return attachment;
    }

    public async Task<IReadOnlyList<ProblemAttachment>> GetByProblemIdAsync(long problemId, CancellationToken ct = default) =>
        await context.ProblemAttachments.Where(a => a.PratPrId == problemId).ToListAsync(ct);

    public async Task DeleteAsync(long id, CancellationToken ct = default)
    {
        var attachment = await context.ProblemAttachments.FindAsync([id], ct);
        if (attachment is not null)
            context.ProblemAttachments.Remove(attachment);
    }
}
