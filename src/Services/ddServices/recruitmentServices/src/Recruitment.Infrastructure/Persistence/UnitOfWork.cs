using Microsoft.EntityFrameworkCore.Storage;
using Recruitment.Domain.Repositories;
using Recruitment.Infrastructure.Repositories;

namespace Recruitment.Infrastructure.Persistence;

/// <summary>
/// Unit of Work implementation
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly RecruitmentDbContext _context;
    private IDbContextTransaction _transaction;

    public IJobRepository Jobs { get; private set; }
    public IApplicationRepository Applications { get; private set; }
    public IRecruitmentCycleRepository RecruitmentCycles { get; private set; }
    public ICourseDetailRepository CourseDetails { get; private set; }
    public IAssessmentRepository Assessments { get; private set; }

    public UnitOfWork(RecruitmentDbContext context)
    {
        _context = context;
        Jobs = new JobRepository(context);
        Applications = new ApplicationRepository(context);
        RecruitmentCycles = new RecruitmentCycleRepository(context);
        CourseDetails = new CourseDetailRepository(context);
        Assessments = new AssessmentRepository(context);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await SaveChangesAsync();
            await _transaction?.CommitAsync();
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        try
        {
            await _transaction?.RollbackAsync();
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context?.Dispose();
    }
}
