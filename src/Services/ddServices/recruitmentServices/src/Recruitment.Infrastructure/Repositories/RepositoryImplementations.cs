using Microsoft.EntityFrameworkCore;
using Recruitment.Domain.Entities;
using Recruitment.Domain.Repositories;
using Recruitment.Infrastructure.Persistence;
using AppApplication = Recruitment.Domain.Entities.Application;

namespace Recruitment.Infrastructure.Repositories;

public class JobRepository : IJobRepository
{
    private readonly RecruitmentDbContext _context;

    public JobRepository(RecruitmentDbContext context)
    {
        _context = context;
    }

    public async Task<Job> GetByIdAsync(decimal jobId)
    {
        return await _context.Jobs.FirstOrDefaultAsync(j => j.JobId == jobId);
    }

    public async Task<IEnumerable<Job>> GetAllAsync()
    {
        return await _context.Jobs.ToListAsync();
    }

    public async Task<IEnumerable<Job>> GetAllByRecruitmentCycleAsync(decimal cycleNo)
    {
        return await _context.Jobs
            .Where(j => j.RecruitmentCycleNo == cycleNo)
            .ToListAsync();
    }

    public async Task<IEnumerable<Job>> GetActiveJobsAsync()
    {
        return await _context.Jobs
            .Where(j => j.IsActive)
            .ToListAsync();
    }

    public async Task AddAsync(Job job)
    {
        await _context.Jobs.AddAsync(job);
    }

    public async Task UpdateAsync(Job job)
    {
        _context.Jobs.Update(job);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(decimal jobId)
    {
        var job = await GetByIdAsync(jobId);
        if (job != null)
        {
            _context.Jobs.Remove(job);
        }
    }
}

public class ApplicationRepository : IApplicationRepository
{
    private readonly RecruitmentDbContext _context;

    public ApplicationRepository(RecruitmentDbContext context)
    {
        _context = context;
    }

    public async Task<AppApplication> GetByIdAsync(decimal applicationNumber)
    {
        return await _context.Applications
            .Include(a => a.StatusHistories)
            .Include(a => a.CourseDetails)
            .FirstOrDefaultAsync(a => a.ApplicationNumber == applicationNumber);
    }

    public async Task<IEnumerable<AppApplication>> GetAllAsync()
    {
        return await _context.Applications
            .Include(a => a.StatusHistories)
            .Include(a => a.CourseDetails)
            .ToListAsync();
    }

    public async Task<IEnumerable<AppApplication>> GetAllByJobIdAsync(decimal jobId)
    {
        return await _context.Applications
            .Where(a => a.JobId == jobId)
            .Include(a => a.StatusHistories)
            .Include(a => a.CourseDetails)
            .ToListAsync();
    }

    public async Task<IEnumerable<AppApplication>> GetAllByStatusAsync(Domain.Enums.ApplicationStatus status)
    {
        return await _context.Applications
            .Where(a => a.Status == status)
            .Include(a => a.StatusHistories)
            .Include(a => a.CourseDetails)
            .ToListAsync();
    }

    public async Task<IEnumerable<AppApplication>> GetAllBySparshIdAsync(string sparshId)
    {
        return await _context.Applications
            .Where(a => a.ContactInfo.SparshId == sparshId)
            .Include(a => a.StatusHistories)
            .Include(a => a.CourseDetails)
            .ToListAsync();
    }

    public async Task AddAsync(AppApplication application)
    {
        await _context.Applications.AddAsync(application);
    }

    public async Task UpdateAsync(AppApplication application)
    {
        _context.Applications.Update(application);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(decimal applicationNumber)
    {
        var application = await GetByIdAsync(applicationNumber);
        if (application != null)
        {
            _context.Applications.Remove(application);
        }
    }

    public async Task<bool> ExistsAsync(decimal applicationNumber)
    {
        return await _context.Applications.AnyAsync(a => a.ApplicationNumber == applicationNumber);
    }

    public async Task<IEnumerable<AppApplication>> GetByRecruitmentCycleAsync(decimal cycleNo)
    {
        return await _context.Applications
            .Join(_context.Jobs, a => a.JobId, j => j.JobId, (a, j) => new { Application = a, Job = j })
            .Where(x => x.Job.RecruitmentCycleNo == cycleNo)
            .Select(x => x.Application)
            .Include(a => a.StatusHistories)
            .Include(a => a.CourseDetails)
            .ToListAsync();
    }
}

public class RecruitmentCycleRepository : IRecruitmentCycleRepository
{
    private readonly RecruitmentDbContext _context;

    public RecruitmentCycleRepository(RecruitmentDbContext context)
    {
        _context = context;
    }

    public async Task<RecruitmentCycle> GetByIdAsync(decimal cycleNo)
    {
        return await _context.RecruitmentCycles.FirstOrDefaultAsync(c => c.RecruitmentCycleNo == cycleNo);
    }

    public async Task<IEnumerable<RecruitmentCycle>> GetAllAsync()
    {
        return await _context.RecruitmentCycles.ToListAsync();
    }

    public async Task<IEnumerable<RecruitmentCycle>> GetActiveAsync()
    {
        return await _context.RecruitmentCycles.Where(c => c.IsActive).ToListAsync();
    }

    public async Task AddAsync(RecruitmentCycle cycle)
    {
        await _context.RecruitmentCycles.AddAsync(cycle);
    }

    public async Task UpdateAsync(RecruitmentCycle cycle)
    {
        _context.RecruitmentCycles.Update(cycle);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(decimal cycleNo)
    {
        var cycle = await GetByIdAsync(cycleNo);
        if (cycle != null)
        {
            _context.RecruitmentCycles.Remove(cycle);
        }
    }
}

public class CourseDetailRepository : ICourseDetailRepository
{
    private readonly RecruitmentDbContext _context;

    public CourseDetailRepository(RecruitmentDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CourseDetail>> GetByApplicationNumberAsync(decimal applicationNumber)
    {
        return await _context.CourseDetails
            .Where(c => c.ApplicationNumber == applicationNumber)
            .ToListAsync();
    }

    public async Task AddAsync(CourseDetail courseDetail)
    {
        await _context.CourseDetails.AddAsync(courseDetail);
    }

    public async Task AddRangeAsync(IEnumerable<CourseDetail> courseDetails)
    {
        await _context.CourseDetails.AddRangeAsync(courseDetails);
    }

    public async Task DeleteByApplicationNumberAsync(decimal applicationNumber)
    {
        var courseDetails = await GetByApplicationNumberAsync(applicationNumber);
        _context.CourseDetails.RemoveRange(courseDetails);
        await Task.CompletedTask;
    }
}

public class AssessmentRepository : IAssessmentRepository
{
    private readonly RecruitmentDbContext _context;

    public AssessmentRepository(RecruitmentDbContext context)
    {
        _context = context;
    }

    public async Task<SteeringCommitteeAssessment> GetByIdAsync(decimal parameterNo)
    {
        return await _context.SteeringCommitteeAssessments
            .FirstOrDefaultAsync(a => a.ParameterNo == parameterNo);
    }

    public async Task<IEnumerable<SteeringCommitteeAssessment>> GetByApplicationNumberAsync(decimal applicationNumber)
    {
        return await _context.SteeringCommitteeAssessments
            .Where(a => a.ApplicationNumber == applicationNumber)
            .ToListAsync();
    }

    public async Task AddAsync(SteeringCommitteeAssessment assessment)
    {
        await _context.SteeringCommitteeAssessments.AddAsync(assessment);
    }

    public async Task UpdateAsync(SteeringCommitteeAssessment assessment)
    {
        _context.SteeringCommitteeAssessments.Update(assessment);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(decimal parameterNo)
    {
        var assessment = await GetByIdAsync(parameterNo);
        if (assessment != null)
        {
            _context.SteeringCommitteeAssessments.Remove(assessment);
        }
    }
}
