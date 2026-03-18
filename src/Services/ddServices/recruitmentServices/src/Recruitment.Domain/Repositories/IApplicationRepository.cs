using Recruitment.Domain.Entities;
using Recruitment.Domain.Enums;

namespace Recruitment.Domain.Repositories;

/// <summary>
/// Repository interface for Application aggregate
/// </summary>
public interface IApplicationRepository
{
    Task<Application> GetByIdAsync(decimal applicationNumber);
    Task<IEnumerable<Application>> GetAllAsync();
    Task<IEnumerable<Application>> GetAllByJobIdAsync(decimal jobId);
    Task<IEnumerable<Application>> GetAllByStatusAsync(ApplicationStatus status);
    Task<IEnumerable<Application>> GetAllBySparshIdAsync(string sparshId);
    Task AddAsync(Application application);
    Task UpdateAsync(Application application);
    Task DeleteAsync(decimal applicationNumber);
    Task<bool> ExistsAsync(decimal applicationNumber);
    Task<IEnumerable<Application>> GetByRecruitmentCycleAsync(decimal cycleNo);
}
