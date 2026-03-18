using FinyearAPI.Data;
using FinyearAPI.Domain.Entities;
using FinyearAPI.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FinyearAPI.Infrastructure.Repositories
{
    /// <summary>
    /// EF Core implementation of Financial Year Aggregate Repository
    /// </summary>
    public class FinancialYearAggregateRepository : IFinancialYearAggregateRepository
    {
        private readonly AdminDbContext _context;
        private readonly ILogger<FinancialYearAggregateRepository> _logger;

        public FinancialYearAggregateRepository(
            AdminDbContext context,
            ILogger<FinancialYearAggregateRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<FinancialYearAggregate?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Fetching financial year aggregate with ID: {Id}", id);
                // Note: This would need proper EF Core mapping configuration
                return await _context.Set<FinancialYearAggregate>()
                    .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching financial year aggregate with ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<FinancialYearAggregate>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Fetching all financial year aggregates");
                return await _context.Set<FinancialYearAggregate>()
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all financial year aggregates");
                throw;
            }
        }

        public async Task<FinancialYearAggregate?> GetCurrentAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Fetching current financial year aggregate");
                var now = DateTime.UtcNow;
                return await _context.Set<FinancialYearAggregate>()
                    .Where(x => x.Status == FinancialYearStatus.Open 
                        && x.Period.StartDate <= now 
                        && x.Period.EndDate >= now)
                    .FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching current financial year aggregate");
                throw;
            }
        }

        public async Task<FinancialYearAggregate?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Fetching financial year aggregate by name: {Name}", name);
                return await _context.Set<FinancialYearAggregate>()
                    .FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching financial year aggregate by name: {Name}", name);
                throw;
            }
        }

        public async Task<FinancialYearAggregate> AddAsync(FinancialYearAggregate aggregate, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Adding new financial year aggregate: {Name}", aggregate.Name);
                
                await _context.Set<FinancialYearAggregate>().AddAsync(aggregate, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                // Publish domain events
                // await _eventPublisher.PublishAsync(aggregate.DomainEvents);

                return aggregate;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding financial year aggregate: {Name}", aggregate.Name);
                throw;
            }
        }

        public async Task<FinancialYearAggregate> UpdateAsync(FinancialYearAggregate aggregate, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Updating financial year aggregate: {Id}", aggregate.Id);

                _context.Set<FinancialYearAggregate>().Update(aggregate);
                await _context.SaveChangesAsync(cancellationToken);

                // Publish domain events
                // await _eventPublisher.PublishAsync(aggregate.DomainEvents);

                return aggregate;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating financial year aggregate: {Id}", aggregate.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Deleting financial year aggregate: {Id}", id);

                var aggregate = await GetByIdAsync(id, cancellationToken);
                if (aggregate == null)
                {
                    _logger.LogWarning("Financial year aggregate not found: {Id}", id);
                    return false;
                }

                _context.Set<FinancialYearAggregate>().Remove(aggregate);
                await _context.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting financial year aggregate: {Id}", id);
                throw;
            }
        }
    }
}
