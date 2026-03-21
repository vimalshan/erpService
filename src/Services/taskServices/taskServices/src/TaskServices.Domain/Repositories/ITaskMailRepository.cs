using TaskServices.Domain.Entities;

namespace TaskServices.Domain.Repositories;

public interface ITaskMailRepository
{
    Task<TaskMail?> GetByIdAsync(decimal mid, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TaskMail>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TaskMail>> GetBySystemUserIdAsync(decimal sysId, CancellationToken cancellationToken = default);
    Task AddAsync(TaskMail taskMail, CancellationToken cancellationToken = default);
    Task UpdateAsync(TaskMail taskMail, CancellationToken cancellationToken = default);
    Task DeleteAsync(decimal mid, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(decimal mid, CancellationToken cancellationToken = default);
}
