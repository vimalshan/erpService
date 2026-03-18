using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AppraisalService.Domain.Repositories;

using Entities;

/// <summary>
/// Repository interface for AppraisalMain aggregate root
/// </summary>
public interface IAppraisalRepository
{
    Task<AppraisalMainEntity?> GetByRequestNumberAsync(long requestNumber, CancellationToken cancellationToken = default);
    Task<AppraisalMainEntity?> GetByUserCodeAsync(string userCode, CancellationToken cancellationToken = default);
    Task<IEnumerable<AppraisalMainEntity>> GetByYearAsync(long yearId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AppraisalMainEntity>> GetByStatusAsync(string statusCode, CancellationToken cancellationToken = default);
    Task AddAsync(AppraisalMainEntity appraisal, CancellationToken cancellationToken = default);
    Task UpdateAsync(AppraisalMainEntity appraisal, CancellationToken cancellationToken = default);
    Task DeleteAsync(long requestNumber, CancellationToken cancellationToken = default);
    Task<int> GetCountAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Repository interface for AppraisalBand
/// </summary>
public interface IAppraisalBandRepository
{
    Task<AppraisalBandEntity?> GetByIdAsync(long bandId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AppraisalBandEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<AppraisalBandEntity>> GetByGradeAsync(long gradeId, CancellationToken cancellationToken = default);
    Task AddAsync(AppraisalBandEntity band, CancellationToken cancellationToken = default);
    Task UpdateAsync(AppraisalBandEntity band, CancellationToken cancellationToken = default);
    Task DeleteAsync(long bandId, CancellationToken cancellationToken = default);
}

/// <summary>
/// Repository interface for CompetencyAssessment
/// </summary>
public interface ICompetencyAssessmentRepository
{
    Task<CompetencyAssessmentEntity?> GetByIdAsync(long serialNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<CompetencyAssessmentEntity>> GetByRequestAsync(long requestNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<CompetencyAssessmentEntity>> GetByAppraiserAsync(string appraiserCode, CancellationToken cancellationToken = default);
    Task AddAsync(CompetencyAssessmentEntity assessment, CancellationToken cancellationToken = default);
    Task UpdateAsync(CompetencyAssessmentEntity assessment, CancellationToken cancellationToken = default);
    Task DeleteAsync(long serialNumber, CancellationToken cancellationToken = default);
}

/// <summary>
/// Repository interface for EmployeeGoal
/// </summary>
public interface IEmployeeGoalRepository
{
    Task<EmployeeGoalEntity?> GetByIdAsync(long requestNumber, long serialNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<EmployeeGoalEntity>> GetByRequestAsync(long requestNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<EmployeeGoalEntity>> GetByUserAsync(string userId, CancellationToken cancellationToken = default);
    Task AddAsync(EmployeeGoalEntity goal, CancellationToken cancellationToken = default);
    Task UpdateAsync(EmployeeGoalEntity goal, CancellationToken cancellationToken = default);
    Task DeleteAsync(long requestNumber, long serialNumber, CancellationToken cancellationToken = default);
}

/// <summary>
/// Unit of Work pattern for repository coordination
/// </summary>
public interface IUnitOfWork : IAsyncDisposable
{
    IAppraisalRepository Appraisals { get; }
    IAppraisalBandRepository AppraisalBands { get; }
    ICompetencyAssessmentRepository CompetencyAssessments { get; }
    IEmployeeGoalRepository EmployeeGoals { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
