using FinyearAPI.Domain.Entities;

namespace FinyearAPI.Domain.Repositories
{
    /// <summary>
    /// Repository interface for FinancialYearAggregate
    /// Defines contracts for data persistence of the aggregate
    /// </summary>
    public interface IFinancialYearAggregateRepository
    {
        /// <summary>
        /// Get financial year aggregate by ID
        /// </summary>
        Task<FinancialYearAggregate?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get all financial years
        /// </summary>
        Task<IEnumerable<FinancialYearAggregate>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get current active financial year
        /// </summary>
        Task<FinancialYearAggregate?> GetCurrentAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get financial year by name
        /// </summary>
        Task<FinancialYearAggregate?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

        /// <summary>
        /// Add new financial year aggregate
        /// </summary>
        Task<FinancialYearAggregate> AddAsync(FinancialYearAggregate aggregate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update financial year aggregate
        /// </summary>
        Task<FinancialYearAggregate> UpdateAsync(FinancialYearAggregate aggregate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete financial year aggregate
        /// </summary>
        Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}
