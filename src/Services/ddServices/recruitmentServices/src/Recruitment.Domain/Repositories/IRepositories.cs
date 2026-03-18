using Recruitment.Domain.Entities;

namespace Recruitment.Domain.Repositories;

/// <summary>
/// Repository interface for RecruitmentCycle
/// </summary>
public interface IRecruitmentCycleRepository
{
    Task<RecruitmentCycle> GetByIdAsync(decimal cycleNo);
    Task<IEnumerable<RecruitmentCycle>> GetAllAsync();
    Task<IEnumerable<RecruitmentCycle>> GetActiveAsync();
    Task AddAsync(RecruitmentCycle cycle);
    Task UpdateAsync(RecruitmentCycle cycle);
    Task DeleteAsync(decimal cycleNo);
}

/// <summary>
/// Repository interface for CourseDetail
/// </summary>
public interface ICourseDetailRepository
{
    Task<IEnumerable<CourseDetail>> GetByApplicationNumberAsync(decimal applicationNumber);
    Task AddAsync(CourseDetail courseDetail);
    Task AddRangeAsync(IEnumerable<CourseDetail> courseDetails);
    Task DeleteByApplicationNumberAsync(decimal applicationNumber);
}

/// <summary>
/// Repository interface for SteeringCommitteeAssessment
/// </summary>
public interface IAssessmentRepository
{
    Task<SteeringCommitteeAssessment> GetByIdAsync(decimal parameterNo);
    Task<IEnumerable<SteeringCommitteeAssessment>> GetByApplicationNumberAsync(decimal applicationNumber);
    Task AddAsync(SteeringCommitteeAssessment assessment);
    Task UpdateAsync(SteeringCommitteeAssessment assessment);
    Task DeleteAsync(decimal parameterNo);
}

/// <summary>
/// Unit of Work pattern interface
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IJobRepository Jobs { get; }
    IApplicationRepository Applications { get; }
    IRecruitmentCycleRepository RecruitmentCycles { get; }
    ICourseDetailRepository CourseDetails { get; }
    IAssessmentRepository Assessments { get; }
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
