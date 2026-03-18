using TaxService.Domain.Entities;

namespace TaxService.Domain.Repositories;

/// <summary>
/// Repository interface for TaxMarginalDetail aggregate
/// </summary>
public interface ITaxMarginalDetailRepository
{
    Task<TaxMarginalDetail?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<TaxMarginalDetail?> GetByEmployeeAndYearAsync(
        long employeeSystemId,
        int financialYear,
        CancellationToken cancellationToken = default);
    Task<IEnumerable<TaxMarginalDetail>> GetByEmployeeAsync(
        long employeeSystemId,
        CancellationToken cancellationToken = default);
    Task AddAsync(TaxMarginalDetail entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TaxMarginalDetail entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}

/// <summary>
/// Repository interface for ConditionalMaster aggregate
/// </summary>
public interface IConditionalMasterRepository
{
    Task<ConditionalMaster?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ConditionalMaster?> GetByPayeeIdAsync(
        string payeeId,
        int? financialYear = null,
        CancellationToken cancellationToken = default);
    Task<IEnumerable<ConditionalMaster>> GetActiveAsync(
        int? financialYear = null,
        CancellationToken cancellationToken = default);
    Task AddAsync(ConditionalMaster entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(ConditionalMaster entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}
