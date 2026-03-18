using Dapper;
using System.Data;

namespace FinyearAPI.Repositories.Dapper
{
    /// <summary>
    /// Dapper-based repository for high-performance queries
    /// Uses SQL Server Connection directly for optimized data access
    /// </summary>
    public interface IDapperRepository
    {
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null);
        Task<T?> QuerySingleOrDefaultAsync<T>(string sql, object? param = null);
        Task<int> ExecuteAsync(string sql, object? param = null);
    }

    /// <summary>
    /// Dapper Repository Implementation
    /// </summary>
    public class DapperRepository : IDapperRepository
    {
        private readonly IDbConnection _dbConnection;

        public DapperRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null)
        {
            return await _dbConnection.QueryAsync<T>(sql, param);
        }

        public async Task<T?> QuerySingleOrDefaultAsync<T>(string sql, object? param = null)
        {
            return await _dbConnection.QuerySingleOrDefaultAsync<T>(sql, param);
        }

        public async Task<int> ExecuteAsync(string sql, object? param = null)
        {
            return await _dbConnection.ExecuteAsync(sql, param);
        }
    }

    /// <summary>
    /// Financial Year Dapper Repository
    /// Handles optimized queries for Financial Year operations
    /// </summary>
    public interface IFinancialYearDapperRepository : IDapperRepository
    {
        Task<dynamic?> GetCurrentFinancialYearAsync();
        Task<IEnumerable<dynamic>> GetAllFinancialYearsAsync();
        Task<int> CreateFinancialYearAsync(long fyId, string fyName, DateTime startDate, DateTime closeDate, long updatedBy);
    }

    public class FinancialYearDapperRepository : DapperRepository, IFinancialYearDapperRepository
    {
        private readonly IDbConnection _dbConnection;

        public FinancialYearDapperRepository(IDbConnection dbConnection)
            : base(dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<dynamic?> GetCurrentFinancialYearAsync()
        {
            const string sql = @"
                SELECT TOP 1
                    FY_ID as FinancialYearId,
                    FY_NAME as FinancialYearName,
                    FY_STARTDATE as StartDate,
                    FY_CLOSEDATE as CloseDate,
                    FY_UPDATED_BY as UpdatedBy,
                    FY_UPDATED_ON as UpdatedOn
                FROM FINYEAR_MASTER
                WHERE GETDATE() BETWEEN FY_STARTDATE AND FY_CLOSEDATE
                ORDER BY FY_STARTDATE DESC;
            ";
            return await QuerySingleOrDefaultAsync<dynamic>(sql);
        }

        public async Task<IEnumerable<dynamic>> GetAllFinancialYearsAsync()
        {
            const string sql = @"
                SELECT
                    FY_ID as FinancialYearId,
                    FY_NAME as FinancialYearName,
                    FY_STARTDATE as StartDate,
                    FY_CLOSEDATE as CloseDate,
                    FY_UPDATED_BY as UpdatedBy,
                    FY_UPDATED_ON as UpdatedOn
                FROM FINYEAR_MASTER
                ORDER BY FY_STARTDATE DESC;
            ";
            return await QueryAsync<dynamic>(sql);
        }

        public async Task<int> CreateFinancialYearAsync(long fyId, string fyName, DateTime startDate, DateTime closeDate, long updatedBy)
        {
            const string sql = @"
                INSERT INTO FINYEAR_MASTER
                (FY_ID, FY_NAME, FY_STARTDATE, FY_CLOSEDATE, FY_UPDATED_BY, FY_UPDATED_ON)
                VALUES
                (@FyId, @FyName, @StartDate, @CloseDate, @UpdatedBy, GETDATE());
            ";
            var parameters = new
            {
                FyId = fyId,
                FyName = fyName,
                StartDate = startDate,
                CloseDate = closeDate,
                UpdatedBy = updatedBy
            };
            return await ExecuteAsync(sql, parameters);
        }
    }
}
