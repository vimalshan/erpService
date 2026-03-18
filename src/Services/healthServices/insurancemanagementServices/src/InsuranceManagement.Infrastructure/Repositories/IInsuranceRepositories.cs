using InsuranceManagement.Domain.Entities;

namespace InsuranceManagement.Infrastructure.Repositories;

/// <summary>
/// Repository interface for Insurance Plan
/// </summary>
public interface IInsurancePlanRepository : IAsyncRepository<InsurancePlan>
{
    Task<InsurancePlan?> GetByNameAsync(string planName);
    Task<List<InsurancePlan>> GetAllActiveAsync();
    Task<bool> AnyActiveEnrollmentsAsync(long planId);
}

/// <summary>
/// Repository interface for Insurance Enrollment
/// </summary>
public interface IInsuranceEnrollmentRepository : IAsyncRepository<InsuranceEnrollment>
{
    Task<InsuranceEnrollment?> GetWithClaimsAsync(long enrollmentId);
    Task<List<InsuranceEnrollment>> GetByEmployeeAsync(long empSysId);
    Task<List<InsuranceEnrollment>> GetActiveByEmployeeAsync(long empSysId);
    Task<InsuranceEnrollment?> GetActiveEnrollmentAsync(long empSysId, long planId);
    Task<bool> HasActiveEnrollmentAsync(long empSysId);
}

/// <summary>
/// Repository interface for Insurance Claim
/// </summary>
public interface IInsuranceClaimRepository : IAsyncRepository<InsuranceClaim>
{
    Task<InsuranceClaim?> GetWithEnrollmentAsync(long claimId);
    Task<List<InsuranceClaim>> GetByEmployeeAsync(long empSysId, int skip, int take);
    Task<int> GetCountByEmployeeAsync(long empSysId);
    Task<List<InsuranceClaim>> GetByEnrollmentAsync(long enrollmentId);
    Task<List<InsuranceClaim>> GetPendingClaimsAsync(int skip, int take);
    Task<int> GetPendingClaimsCountAsync();
    Task<List<InsuranceClaim>> GetByStatusAsync(string status, int skip, int take);
}

/// <summary>
/// Unit of work interface
/// </summary>
public interface IInsuranceManagementUnitOfWork : IAsyncUnitOfWork
{
    IInsurancePlanRepository PlanRepository { get; }
    IInsuranceEnrollmentRepository EnrollmentRepository { get; }
    IInsuranceClaimRepository ClaimRepository { get; }
}
