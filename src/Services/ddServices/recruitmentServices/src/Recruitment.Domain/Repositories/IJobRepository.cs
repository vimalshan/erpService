using Recruitment.Domain.Entities;

namespace Recruitment.Domain.Repositories;

/// <summary>
/// Repository interface for Job aggregate
/// </summary>
public interface IJobRepository
{
    Task<Job> GetByIdAsync(decimal jobId);
    Task<IEnumerable<Job>> GetAllAsync();
    Task<IEnumerable<Job>> GetAllByRecruitmentCycleAsync(decimal cycleNo);
    Task<IEnumerable<Job>> GetActiveJobsAsync();
    Task AddAsync(Job job);
    Task UpdateAsync(Job job);
    Task DeleteAsync(decimal jobId);
}
