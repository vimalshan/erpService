using SparshTransactional.Domain.Entities;

namespace SparshTransactional.Domain.Interfaces;

public interface IScholarshipMasterRepository
{
    Task<ScholarshipMaster?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<ScholarshipMaster>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<ScholarshipMaster>> GetActiveAsync(CancellationToken ct = default);
    Task<ScholarshipMaster> AddAsync(ScholarshipMaster scholarship, CancellationToken ct = default);
    Task UpdateAsync(ScholarshipMaster scholarship, CancellationToken ct = default);
}

public interface IEligibilityCriteriaRepository
{
    Task<EligibilityCriteria?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<EligibilityCriteria>> GetByScholarshipIdAsync(long scholarshipId, CancellationToken ct = default);
    Task<EligibilityCriteria> AddAsync(EligibilityCriteria criteria, CancellationToken ct = default);
    Task UpdateAsync(EligibilityCriteria criteria, CancellationToken ct = default);
    Task DeleteAsync(long id, CancellationToken ct = default);
}

public interface IScholarshipApplicationRepository
{
    Task<ScholarshipApplication?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<ScholarshipApplication>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<ScholarshipApplication>> GetByStatusAsync(string status, CancellationToken ct = default);
    Task<IReadOnlyList<ScholarshipApplication>> GetByStudentIdAsync(long studentId, CancellationToken ct = default);
    Task<IReadOnlyList<ScholarshipApplication>> GetByScholarshipIdAsync(long scholarshipId, CancellationToken ct = default);
    Task<ScholarshipApplication> AddAsync(ScholarshipApplication application, CancellationToken ct = default);
    Task UpdateAsync(ScholarshipApplication application, CancellationToken ct = default);
}

public interface IScholarshipDisbursementRepository
{
    Task<ScholarshipDisbursement?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<ScholarshipDisbursement>> GetByApplicationIdAsync(long applicationId, CancellationToken ct = default);
    Task<IReadOnlyList<ScholarshipDisbursement>> GetByStatusAsync(string status, CancellationToken ct = default);
    Task<IReadOnlyList<ScholarshipDisbursement>> GetAllAsync(CancellationToken ct = default);
    Task<ScholarshipDisbursement> AddAsync(ScholarshipDisbursement disbursement, CancellationToken ct = default);
    Task UpdateAsync(ScholarshipDisbursement disbursement, CancellationToken ct = default);
}
