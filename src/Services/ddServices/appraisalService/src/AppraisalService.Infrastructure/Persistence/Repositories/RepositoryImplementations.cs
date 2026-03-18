using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AppraisalService.Domain;
using AppraisalService.Domain.Entities;
using AppraisalService.Domain.Repositories;
using AppraisalService.Infrastructure.Persistence.Data;

namespace AppraisalService.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository implementation for AppraisalMain aggregate
/// </summary>
public class AppraisalRepository : IAppraisalRepository
{
    private readonly AppraisalDbContext _context;

    public AppraisalRepository(AppraisalDbContext context)
    {
        _context = context;
    }

    public async Task<AppraisalMainEntity?> GetByRequestNumberAsync(long requestNumber, CancellationToken cancellationToken = default)
    {
        return await _context.AppraisalMains
            .Include(a => a.CompetencyAssessments)
            .FirstOrDefaultAsync(a => a.RequestNumber == requestNumber, cancellationToken);
    }

    public async Task<AppraisalMainEntity?> GetByUserCodeAsync(string userCode, CancellationToken cancellationToken = default)
    {
        return await _context.AppraisalMains
            .Include(a => a.CompetencyAssessments)
            .FirstOrDefaultAsync(a => a.UserCode == userCode, cancellationToken);
    }

    public async Task<IEnumerable<AppraisalMainEntity>> GetByYearAsync(long yearId, CancellationToken cancellationToken = default)
    {
        return await _context.AppraisalMains
            .Where(a => a.YearId == yearId)
            .Include(a => a.CompetencyAssessments)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<AppraisalMainEntity>> GetByStatusAsync(string statusCode, CancellationToken cancellationToken = default)
    {
        return await _context.AppraisalMains
            .Where(a => a.Status.Code == statusCode)
            .Include(a => a.CompetencyAssessments)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(AppraisalMainEntity appraisal, CancellationToken cancellationToken = default)
    {
        await _context.AppraisalMains.AddAsync(appraisal, cancellationToken);
    }

    public async Task UpdateAsync(AppraisalMainEntity appraisal, CancellationToken cancellationToken = default)
    {
        _context.AppraisalMains.Update(appraisal);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(long requestNumber, CancellationToken cancellationToken = default)
    {
        var appraisal = await GetByRequestNumberAsync(requestNumber, cancellationToken);
        if (appraisal != null)
        {
            _context.AppraisalMains.Remove(appraisal);
        }
    }

    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.AppraisalMains.CountAsync(cancellationToken);
    }
}

/// <summary>
/// Repository implementation for AppraisalBand
/// </summary>
public class AppraisalBandRepository : IAppraisalBandRepository
{
    private readonly AppraisalDbContext _context;

    public AppraisalBandRepository(AppraisalDbContext context)
    {
        _context = context;
    }

    public async Task<AppraisalBandEntity?> GetByIdAsync(long bandId, CancellationToken cancellationToken = default)
    {
        return await _context.AppraisalBands.FirstOrDefaultAsync(b => b.Id == bandId, cancellationToken);
    }

    public async Task<IEnumerable<AppraisalBandEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.AppraisalBands.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<AppraisalBandEntity>> GetByGradeAsync(long gradeId, CancellationToken cancellationToken = default)
    {
        return await _context.AppraisalBands
            .Where(b => b.GradeId == gradeId)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(AppraisalBandEntity band, CancellationToken cancellationToken = default)
    {
        await _context.AppraisalBands.AddAsync(band, cancellationToken);
    }

    public async Task UpdateAsync(AppraisalBandEntity band, CancellationToken cancellationToken = default)
    {
        _context.AppraisalBands.Update(band);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(long bandId, CancellationToken cancellationToken = default)
    {
        var band = await GetByIdAsync(bandId, cancellationToken);
        if (band != null)
        {
            _context.AppraisalBands.Remove(band);
        }
    }
}

/// <summary>
/// Repository implementation for CompetencyAssessment
/// </summary>
public class CompetencyAssessmentRepository : ICompetencyAssessmentRepository
{
    private readonly AppraisalDbContext _context;

    public CompetencyAssessmentRepository(AppraisalDbContext context)
    {
        _context = context;
    }

    public async Task<CompetencyAssessmentEntity?> GetByIdAsync(long serialNumber, CancellationToken cancellationToken = default)
    {
        return await _context.CompetencyAssessments
            .FirstOrDefaultAsync(c => c.SerialNumber == serialNumber, cancellationToken);
    }

    public async Task<IEnumerable<CompetencyAssessmentEntity>> GetByRequestAsync(long requestNumber, CancellationToken cancellationToken = default)
    {
        return await _context.CompetencyAssessments
            .Where(c => c.RequestNumber == requestNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CompetencyAssessmentEntity>> GetByAppraiserAsync(string appraiserCode, CancellationToken cancellationToken = default)
    {
        return await _context.CompetencyAssessments
            .Where(c => c.AppraiserUserCode == appraiserCode)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(CompetencyAssessmentEntity assessment, CancellationToken cancellationToken = default)
    {
        await _context.CompetencyAssessments.AddAsync(assessment, cancellationToken);
    }

    public async Task UpdateAsync(CompetencyAssessmentEntity assessment, CancellationToken cancellationToken = default)
    {
        _context.CompetencyAssessments.Update(assessment);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(long serialNumber, CancellationToken cancellationToken = default)
    {
        var assessment = await GetByIdAsync(serialNumber, cancellationToken);
        if (assessment != null)
        {
            _context.CompetencyAssessments.Remove(assessment);
        }
    }
}

/// <summary>
/// Repository implementation for EmployeeGoal
/// </summary>
public class EmployeeGoalRepository : IEmployeeGoalRepository
{
    private readonly AppraisalDbContext _context;

    public EmployeeGoalRepository(AppraisalDbContext context)
    {
        _context = context;
    }

    public async Task<EmployeeGoalEntity?> GetByIdAsync(long requestNumber, long serialNumber, CancellationToken cancellationToken = default)
    {
        return await _context.EmployeeGoals
            .FirstOrDefaultAsync(g => g.RequestNumber == requestNumber && g.SerialNumber == serialNumber, cancellationToken);
    }

    public async Task<IEnumerable<EmployeeGoalEntity>> GetByRequestAsync(long requestNumber, CancellationToken cancellationToken = default)
    {
        return await _context.EmployeeGoals
            .Where(g => g.RequestNumber == requestNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<EmployeeGoalEntity>> GetByUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _context.EmployeeGoals
            .Where(g => g.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(EmployeeGoalEntity goal, CancellationToken cancellationToken = default)
    {
        await _context.EmployeeGoals.AddAsync(goal, cancellationToken);
    }

    public async Task UpdateAsync(EmployeeGoalEntity goal, CancellationToken cancellationToken = default)
    {
        _context.EmployeeGoals.Update(goal);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(long requestNumber, long serialNumber, CancellationToken cancellationToken = default)
    {
        var goal = await GetByIdAsync(requestNumber, serialNumber, cancellationToken);
        if (goal != null)
        {
            _context.EmployeeGoals.Remove(goal);
        }
    }
}

/// <summary>
/// Unit of Work implementation coordinating all repositories
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly AppraisalDbContext _context;
    private IAppraisalRepository? _appraisalRepository;
    private IAppraisalBandRepository? _appraisalBandRepository;
    private ICompetencyAssessmentRepository? _competencyAssessmentRepository;
    private IEmployeeGoalRepository? _employeeGoalRepository;

    public UnitOfWork(AppraisalDbContext context)
    {
        _context = context;
    }

    public IAppraisalRepository Appraisals =>
        _appraisalRepository ??= new AppraisalRepository(_context);

    public IAppraisalBandRepository AppraisalBands =>
        _appraisalBandRepository ??= new AppraisalBandRepository(_context);

    public ICompetencyAssessmentRepository CompetencyAssessments =>
        _competencyAssessmentRepository ??= new CompetencyAssessmentRepository(_context);

    public IEmployeeGoalRepository EmployeeGoals =>
        _employeeGoalRepository ??= new EmployeeGoalRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}
