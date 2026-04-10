using Microsoft.EntityFrameworkCore;
using SparshTransactional.Domain.Entities;
using SparshTransactional.Domain.Interfaces;
using SparshTransactional.Infrastructure.Data;

namespace SparshTransactional.Infrastructure.Repositories;

public class ScholarshipMasterRepository(SparshTransactionalDbContext context) : IScholarshipMasterRepository
{
    public async Task<ScholarshipMaster?> GetByIdAsync(long id, CancellationToken ct = default) =>
        await context.Scholarships
            .Include(s => s.EligibilityCriteria)
            .Include(s => s.Applications)
            .FirstOrDefaultAsync(s => s.ScholarshipId == id, ct);

    public async Task<IReadOnlyList<ScholarshipMaster>> GetAllAsync(CancellationToken ct = default) =>
        await context.Scholarships
            .OrderByDescending(s => s.CreatedOn)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<ScholarshipMaster>> GetActiveAsync(CancellationToken ct = default) =>
        await context.Scholarships
            .Where(s => s.Status == "A")
            .OrderBy(s => s.ScholarshipName)
            .ToListAsync(ct);

    public async Task<ScholarshipMaster> AddAsync(ScholarshipMaster scholarship, CancellationToken ct = default)
    {
        await context.Scholarships.AddAsync(scholarship, ct);
        return scholarship;
    }

    public Task UpdateAsync(ScholarshipMaster scholarship, CancellationToken ct = default)
    {
        context.Scholarships.Update(scholarship);
        return Task.CompletedTask;
    }
}

public class EligibilityCriteriaRepository(SparshTransactionalDbContext context) : IEligibilityCriteriaRepository
{
    public async Task<EligibilityCriteria?> GetByIdAsync(long id, CancellationToken ct = default) =>
        await context.EligibilityCriteria.FirstOrDefaultAsync(c => c.CriteriaId == id, ct);

    public async Task<IReadOnlyList<EligibilityCriteria>> GetByScholarshipIdAsync(long scholarshipId, CancellationToken ct = default) =>
        await context.EligibilityCriteria
            .Where(c => c.ScholarshipId == scholarshipId)
            .ToListAsync(ct);

    public async Task<EligibilityCriteria> AddAsync(EligibilityCriteria criteria, CancellationToken ct = default)
    {
        await context.EligibilityCriteria.AddAsync(criteria, ct);
        return criteria;
    }

    public Task UpdateAsync(EligibilityCriteria criteria, CancellationToken ct = default)
    {
        context.EligibilityCriteria.Update(criteria);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long id, CancellationToken ct = default)
    {
        var criteria = await context.EligibilityCriteria.FindAsync([id], ct);
        if (criteria is not null)
            context.EligibilityCriteria.Remove(criteria);
    }
}

public class ScholarshipApplicationRepository(SparshTransactionalDbContext context) : IScholarshipApplicationRepository
{
    public async Task<ScholarshipApplication?> GetByIdAsync(long id, CancellationToken ct = default) =>
        await context.Applications
            .Include(a => a.Disbursements)
            .FirstOrDefaultAsync(a => a.ApplicationId == id, ct);

    public async Task<IReadOnlyList<ScholarshipApplication>> GetAllAsync(CancellationToken ct = default) =>
        await context.Applications
            .OrderByDescending(a => a.ApplicationDate)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<ScholarshipApplication>> GetByStatusAsync(string status, CancellationToken ct = default) =>
        await context.Applications
            .Where(a => a.ApplicationStatus == status)
            .OrderByDescending(a => a.ApplicationDate)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<ScholarshipApplication>> GetByStudentIdAsync(long studentId, CancellationToken ct = default) =>
        await context.Applications
            .Where(a => a.StudentId == studentId)
            .OrderByDescending(a => a.ApplicationDate)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<ScholarshipApplication>> GetByScholarshipIdAsync(long scholarshipId, CancellationToken ct = default) =>
        await context.Applications
            .Where(a => a.ScholarshipId == scholarshipId)
            .OrderByDescending(a => a.ApplicationDate)
            .ToListAsync(ct);

    public async Task<ScholarshipApplication> AddAsync(ScholarshipApplication application, CancellationToken ct = default)
    {
        await context.Applications.AddAsync(application, ct);
        return application;
    }

    public Task UpdateAsync(ScholarshipApplication application, CancellationToken ct = default)
    {
        context.Applications.Update(application);
        return Task.CompletedTask;
    }
}

public class ScholarshipDisbursementRepository(SparshTransactionalDbContext context) : IScholarshipDisbursementRepository
{
    public async Task<ScholarshipDisbursement?> GetByIdAsync(long id, CancellationToken ct = default) =>
        await context.Disbursements.FirstOrDefaultAsync(d => d.DisbursementId == id, ct);

    public async Task<IReadOnlyList<ScholarshipDisbursement>> GetByApplicationIdAsync(long applicationId, CancellationToken ct = default) =>
        await context.Disbursements
            .Where(d => d.ApplicationId == applicationId)
            .OrderByDescending(d => d.CreatedOn)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<ScholarshipDisbursement>> GetByStatusAsync(string status, CancellationToken ct = default) =>
        await context.Disbursements
            .Where(d => d.DisbursementStatus == status)
            .OrderByDescending(d => d.CreatedOn)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<ScholarshipDisbursement>> GetAllAsync(CancellationToken ct = default) =>
        await context.Disbursements
            .OrderByDescending(d => d.CreatedOn)
            .ToListAsync(ct);

    public async Task<ScholarshipDisbursement> AddAsync(ScholarshipDisbursement disbursement, CancellationToken ct = default)
    {
        await context.Disbursements.AddAsync(disbursement, ct);
        return disbursement;
    }

    public Task UpdateAsync(ScholarshipDisbursement disbursement, CancellationToken ct = default)
    {
        context.Disbursements.Update(disbursement);
        return Task.CompletedTask;
    }
}
