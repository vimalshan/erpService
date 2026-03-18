using Dapper;
using FinyearAPI.Domain.Entities;
using FinyearAPI.Domain.Repositories;
using System.Data;

namespace FinyearAPI.Infrastructure.Repositories
{
    /// <summary>
    /// Dapper-based implementation of Financial Year Aggregate Repository for optimized queries
    /// </summary>
    public class FinancialYearDapperRepository : IFinancialYearAggregateRepository
    {
        private readonly IDbConnection _connection;
        private readonly ILogger<FinancialYearDapperRepository> _logger;

        public FinancialYearDapperRepository(
            IDbConnection connection,
            ILogger<FinancialYearDapperRepository> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public async Task<FinancialYearAggregate?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Fetching financial year aggregate with ID: {Id} using Dapper", id);

                const string sql = @"
                    SELECT 
                        Id, 
                        Name, 
                        StartDate, 
                        EndDate, 
                        Status, 
                        UpdatedBy, 
                        UpdatedOn 
                    FROM FinancialYears 
                    WHERE Id = @Id";

                await _connection.OpenAsync();
                var result = await _connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { Id = id });

                if (result == null)
                    return null;

                // Map to aggregate - would need proper mapping logic
                return MapToDomain(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching financial year aggregate with ID: {Id}", id);
                throw;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public async Task<IEnumerable<FinancialYearAggregate>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Fetching all financial year aggregates using Dapper");

                const string sql = @"
                    SELECT 
                        Id, 
                        Name, 
                        StartDate, 
                        EndDate, 
                        Status, 
                        UpdatedBy, 
                        UpdatedOn 
                    FROM FinancialYears 
                    ORDER BY UpdatedOn DESC";

                await _connection.OpenAsync();
                var results = await _connection.QueryAsync<dynamic>(sql);

                return results.Select(MapToDomain).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all financial year aggregates");
                throw;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public async Task<FinancialYearAggregate?> GetCurrentAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Fetching current financial year aggregate using Dapper");

                const string sql = @"
                    SELECT 
                        Id, 
                        Name, 
                        StartDate, 
                        EndDate, 
                        Status, 
                        UpdatedBy, 
                        UpdatedOn 
                    FROM FinancialYears 
                    WHERE Status = @Status 
                        AND StartDate <= @Now 
                        AND EndDate >= @Now";

                var now = DateTime.UtcNow;
                await _connection.OpenAsync();
                var result = await _connection.QueryFirstOrDefaultAsync<dynamic>(
                    sql,
                    new { Status = "Open", Now = now });

                return result == null ? null : MapToDomain(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching current financial year aggregate");
                throw;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public async Task<FinancialYearAggregate?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Fetching financial year aggregate by name: {Name} using Dapper", name);

                const string sql = @"
                    SELECT 
                        Id, 
                        Name, 
                        StartDate, 
                        EndDate, 
                        Status, 
                        UpdatedBy, 
                        UpdatedOn 
                    FROM FinancialYears 
                    WHERE Name = @Name";

                await _connection.OpenAsync();
                var result = await _connection.QueryFirstOrDefaultAsync<dynamic>(
                    sql,
                    new { Name = name });

                return result == null ? null : MapToDomain(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching financial year aggregate by name: {Name}", name);
                throw;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public async Task<FinancialYearAggregate> AddAsync(FinancialYearAggregate aggregate, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Adding new financial year aggregate: {Name} using Dapper", aggregate.Name);

                const string sql = @"
                    INSERT INTO FinancialYears 
                        (Name, StartDate, EndDate, Status, UpdatedBy, UpdatedOn) 
                    VALUES 
                        (@Name, @StartDate, @EndDate, @Status, @UpdatedBy, @UpdatedOn);
                    SELECT CAST(SCOPE_IDENTITY() as long)";

                await _connection.OpenAsync();
                var id = await _connection.ExecuteScalarAsync<long>(
                    sql,
                    new
                    {
                        Name = aggregate.Name,
                        StartDate = aggregate.Period.StartDate,
                        EndDate = aggregate.Period.EndDate,
                        Status = aggregate.Status.ToString(),
                        UpdatedBy = aggregate.UpdatedBy,
                        UpdatedOn = aggregate.UpdatedOn
                    });

                // Publish domain events
                // await _eventPublisher.PublishAsync(aggregate.DomainEvents);

                return aggregate;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding financial year aggregate: {Name}", aggregate.Name);
                throw;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public async Task<FinancialYearAggregate> UpdateAsync(FinancialYearAggregate aggregate, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Updating financial year aggregate: {Id} using Dapper", aggregate.Id);

                const string sql = @"
                    UPDATE FinancialYears 
                    SET 
                        Name = @Name,
                        StartDate = @StartDate,
                        EndDate = @EndDate,
                        Status = @Status,
                        UpdatedBy = @UpdatedBy,
                        UpdatedOn = @UpdatedOn
                    WHERE Id = @Id";

                await _connection.OpenAsync();
                await _connection.ExecuteAsync(
                    sql,
                    new
                    {
                        Id = aggregate.Id,
                        Name = aggregate.Name,
                        StartDate = aggregate.Period.StartDate,
                        EndDate = aggregate.Period.EndDate,
                        Status = aggregate.Status.ToString(),
                        UpdatedBy = aggregate.UpdatedBy,
                        UpdatedOn = aggregate.UpdatedOn
                    });

                // Publish domain events
                // await _eventPublisher.PublishAsync(aggregate.DomainEvents);

                return aggregate;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating financial year aggregate: {Id}", aggregate.Id);
                throw;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Deleting financial year aggregate: {Id} using Dapper", id);

                const string sql = @"DELETE FROM FinancialYears WHERE Id = @Id";

                await _connection.OpenAsync();
                var result = await _connection.ExecuteAsync(sql, new { Id = id });

                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting financial year aggregate: {Id}", id);
                throw;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        private FinancialYearAggregate MapToDomain(dynamic record)
        {
            try
            {
                var startDate = (DateTime)record.StartDate;
                var endDate = (DateTime)record.EndDate;
                var dateRange = DateRange.Create(startDate, endDate);

                var statusEnum = Enum.Parse<FinancialYearStatus>(record.Status);

                // Reconstruct aggregate - implementation depends on FinancialYearAggregate public constructor
                return new FinancialYearAggregate
                {
                    Id = (long)record.Id,
                    Name = (string)record.Name,
                    Period = dateRange,
                    Status = statusEnum,
                    UpdatedBy = (string?)record.UpdatedBy,
                    UpdatedOn = (DateTime)record.UpdatedOn
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error mapping record to FinancialYearAggregate domain object");
                throw;
            }
        }
    }
}
