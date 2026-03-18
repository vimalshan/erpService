using ExitManagement.Domain.Entities;

namespace ExitManagement.Domain.Interfaces;

public interface IEmployeeExitRepository
{
    Task<EmployeeExit?> GetByIdAsync(decimal exitNo, CancellationToken ct = default);
    Task<IEnumerable<EmployeeExit>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<EmployeeExit>> GetByEmployeeAsync(decimal employeeSysId, CancellationToken ct = default);
    Task AddAsync(EmployeeExit entity, CancellationToken ct = default);
    Task UpdateAsync(EmployeeExit entity, CancellationToken ct = default);
    Task DeleteAsync(decimal exitNo, CancellationToken ct = default);
}

public interface IExitInterviewFeedbackRepository
{
    Task<IEnumerable<ExitInterviewFeedback>> GetByExitNoAsync(decimal exitNo, CancellationToken ct = default);
    Task AddAsync(ExitInterviewFeedback entity, CancellationToken ct = default);
    Task UpdateAsync(ExitInterviewFeedback entity, CancellationToken ct = default);
}

public interface IExitQuestionRepository
{
    Task<IEnumerable<ExitQuestion>> GetAllAsync(CancellationToken ct = default);
    Task<ExitQuestion?> GetByIdAsync(string questionId, CancellationToken ct = default);
    Task AddAsync(ExitQuestion entity, CancellationToken ct = default);
    Task UpdateAsync(ExitQuestion entity, CancellationToken ct = default);
}

public interface IExitInterviewQuestionRepository
{
    Task<IEnumerable<ExitInterviewQuestion>> GetAllAsync(CancellationToken ct = default);
    Task<ExitInterviewQuestion?> GetByIdAsync(string questionId, CancellationToken ct = default);
}

public interface IExitResponsibilityMapRepository
{
    Task<IEnumerable<ExitResponsibilityMap>> GetByEmployeeAsync(decimal employeeSysId, CancellationToken ct = default);
    Task AddAsync(ExitResponsibilityMap entity, CancellationToken ct = default);
}
