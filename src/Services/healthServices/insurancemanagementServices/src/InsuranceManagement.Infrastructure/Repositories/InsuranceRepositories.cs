using Microsoft.EntityFrameworkCore;
using InsuranceManagement.Domain.Entities;
using InsuranceManagement.Infrastructure.Data;

namespace InsuranceManagement.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Insurance Plan
/// </summary>
public class InsurancePlanRepository : EFRepository<InsurancePlan>, IInsurancePlanRepository
{
    private readonly InsuranceManagementDbContext _context;

    public InsurancePlanRepository(InsuranceManagementDbContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task<InsurancePlan?> GetByNameAsync(string planName)
    {
        return await _context.InsurancePlans
            .FirstOrDefaultAsync(p => p.PlanName == planName);
    }

    public async Task<List<InsurancePlan>> GetAllActiveAsync()
    {
        return await _context.InsurancePlans
            .Where(p => p.IsActive)
            .ToListAsync();
    }

    public async Task<bool> AnyActiveEnrollmentsAsync(long planId)
    {
        return await _context.InsuranceEnrollments
            .AnyAsync(e => e.InsurancePlanId == planId && e.Status.Value == "A");
    }
}

/// <summary>
/// Repository implementation for Insurance Enrollment
/// </summary>
public class InsuranceEnrollmentRepository : EFRepository<InsuranceEnrollment>, IInsuranceEnrollmentRepository
{
    private readonly InsuranceManagementDbContext _context;

    public InsuranceEnrollmentRepository(InsuranceManagementDbContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task<InsuranceEnrollment?> GetWithClaimsAsync(long enrollmentId)
    {
        return await _context.InsuranceEnrollments
            .Include(e => e.Claims)
            .Include(e => e.InsurancePlan)
            .FirstOrDefaultAsync(e => e.EnrollmentId == enrollmentId);
    }

    public async Task<List<InsuranceEnrollment>> GetByEmployeeAsync(long empSysId)
    {
        return await _context.InsuranceEnrollments
            .Where(e => e.EmpSysId == empSysId)
            .Include(e => e.InsurancePlan)
            .ToListAsync();
    }

    public async Task<List<InsuranceEnrollment>> GetActiveByEmployeeAsync(long empSysId)
    {
        return await _context.InsuranceEnrollments
            .Where(e => e.EmpSysId == empSysId && e.Status.Value == "A")
            .Include(e => e.InsurancePlan)
            .ToListAsync();
    }

    public async Task<InsuranceEnrollment?> GetActiveEnrollmentAsync(long empSysId, long planId)
    {
        return await _context.InsuranceEnrollments
            .FirstOrDefaultAsync(e => e.EmpSysId == empSysId && 
                                  e.InsurancePlanId == planId && 
                                  e.Status.Value == "A");
    }

    public async Task<bool> HasActiveEnrollmentAsync(long empSysId)
    {
        return await _context.InsuranceEnrollments
            .AnyAsync(e => e.EmpSysId == empSysId && e.Status.Value == "A");
    }
}

/// <summary>
/// Repository implementation for Insurance Claim
/// </summary>
public class InsuranceClaimRepository : EFRepository<InsuranceClaim>, IInsuranceClaimRepository
{
    private readonly InsuranceManagementDbContext _context;

    public InsuranceClaimRepository(InsuranceManagementDbContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task<InsuranceClaim?> GetWithEnrollmentAsync(long claimId)
    {
        return await _context.InsuranceClaims
            .Include(c => c.Enrollment)
            .FirstOrDefaultAsync(c => c.ClaimId == claimId);
    }

    public async Task<List<InsuranceClaim>> GetByEmployeeAsync(long empSysId, int skip, int take)
    {
        return await _context.InsuranceClaims
            .Where(c => c.EmpSysId == empSysId)
            .OrderByDescending(c => c.CreatedOn)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<int> GetCountByEmployeeAsync(long empSysId)
    {
        return await _context.InsuranceClaims
            .CountAsync(c => c.EmpSysId == empSysId);
    }

    public async Task<List<InsuranceClaim>> GetByEnrollmentAsync(long enrollmentId)
    {
        return await _context.InsuranceClaims
            .Where(c => c.EnrollmentId == enrollmentId)
            .OrderByDescending(c => c.CreatedOn)
            .ToListAsync();
    }

    public async Task<List<InsuranceClaim>> GetPendingClaimsAsync(int skip, int take)
    {
        return await _context.InsuranceClaims
            .Where(c => c.Status.Value == "S" || c.Status.Value == "PND")
            .OrderBy(c => c.CreatedOn)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<int> GetPendingClaimsCountAsync()
    {
        return await _context.InsuranceClaims
            .CountAsync(c => c.Status.Value == "S" || c.Status.Value == "PND");
    }

    public async Task<List<InsuranceClaim>> GetByStatusAsync(string status, int skip, int take)
    {
        return await _context.InsuranceClaims
            .Where(c => c.Status.Value == status)
            .OrderByDescending(c => c.CreatedOn)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }
}
