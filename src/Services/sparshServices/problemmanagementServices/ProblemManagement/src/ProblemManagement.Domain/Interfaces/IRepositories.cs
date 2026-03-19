using ProblemManagement.Domain.Entities;

namespace ProblemManagement.Domain.Interfaces;

public interface IProblemRepository
{
    Task<ProblemMain?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<ProblemMain>> GetByStatusAsync(char status, CancellationToken ct = default);
    Task<IReadOnlyList<ProblemMain>> GetAllAsync(CancellationToken ct = default);
    Task<ProblemMain> AddAsync(ProblemMain problem, CancellationToken ct = default);
    Task UpdateAsync(ProblemMain problem, CancellationToken ct = default);
    Task DeleteAsync(long id, CancellationToken ct = default);
}

public interface IProblemSolutionRepository
{
    Task<ProblemSolution?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<ProblemSolution>> GetByProblemIdAsync(long problemId, CancellationToken ct = default);
    Task<ProblemSolution> AddAsync(ProblemSolution solution, CancellationToken ct = default);
    Task UpdateAsync(ProblemSolution solution, CancellationToken ct = default);
}

public interface IProblemApprovalRepository
{
    Task<ProblemApproval> AddAsync(ProblemApproval approval, CancellationToken ct = default);
    Task<IReadOnlyList<ProblemApproval>> GetByProblemIdAsync(long problemId, CancellationToken ct = default);
}

public interface ISolutionApprovalRepository
{
    Task<SolutionApproval> AddAsync(SolutionApproval approval, CancellationToken ct = default);
    Task<IReadOnlyList<SolutionApproval>> GetBySolutionIdAsync(long solutionId, CancellationToken ct = default);
}

public interface ISolutionCommentRepository
{
    Task<SolutionComment> AddAsync(SolutionComment comment, CancellationToken ct = default);
    Task<IReadOnlyList<SolutionComment>> GetBySolutionIdAsync(long solutionId, CancellationToken ct = default);
}

public interface IProblemFunctionRepository
{
    Task<IReadOnlyList<ProblemFunction>> GetAllAsync(CancellationToken ct = default);
    Task<ProblemFunction?> GetByIdAsync(long id, CancellationToken ct = default);
}

public interface IProblemImpactRepository
{
    Task<IReadOnlyList<ProblemImpact>> GetAllAsync(CancellationToken ct = default);
    Task<ProblemImpact?> GetByIdAsync(long id, CancellationToken ct = default);
}

public interface IProblemAttachmentRepository
{
    Task<ProblemAttachment> AddAsync(ProblemAttachment attachment, CancellationToken ct = default);
    Task<IReadOnlyList<ProblemAttachment>> GetByProblemIdAsync(long problemId, CancellationToken ct = default);
    Task DeleteAsync(long id, CancellationToken ct = default);
}
