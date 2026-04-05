using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Infrastructure.Dapper;

public class TransactionDapperRepository : ITransactionDapperRepository
{
    private readonly string _connectionString;

    public TransactionDapperRepository(IConfiguration configuration) =>
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    public async Task<IEnumerable<dynamic>> GetPendingApprovalsAsync(long? approverId = null, CancellationToken cancellationToken = default)
    {
        const string sql = "EXEC dbo.usp_GetPendingApprovals @ApproverId";
        await using var conn = new SqlConnection(_connectionString);
        return await conn.QueryAsync(sql, new { ApproverId = approverId });
    }

    public async Task<IEnumerable<dynamic>> GetAuditLogAsync(string? entityType = null, long? entityId = null, DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default)
    {
        const string sql = "EXEC dbo.usp_GetAuditLog @EntityType, @EntityId, @FromDate, @ToDate";
        await using var conn = new SqlConnection(_connectionString);
        return await conn.QueryAsync(sql, new { EntityType = entityType, EntityId = entityId, FromDate = fromDate, ToDate = toDate });
    }

    public async Task<IEnumerable<dynamic>> GetPendingDisbursementsAsync(CancellationToken cancellationToken = default)
    {
        const string sql = "EXEC dbo.usp_GetPendingDisbursements";
        await using var conn = new SqlConnection(_connectionString);
        return await conn.QueryAsync(sql);
    }

    public async Task<IEnumerable<dynamic>> GetAvailableRoomsAsync(DateTime date, TimeSpan startTime, TimeSpan endTime, CancellationToken cancellationToken = default)
    {
        const string sql = "EXEC dbo.usp_GetAvailableRooms @Date, @StartTime, @EndTime";
        await using var conn = new SqlConnection(_connectionString);
        return await conn.QueryAsync(sql, new { Date = date, StartTime = startTime, EndTime = endTime });
    }

    public async Task<bool> ValidateBookingAttendeesAsync(long bookingId, CancellationToken cancellationToken = default)
    {
        const string sql = "EXEC dbo.usp_ValidateBookingAttendees @BookingId";
        await using var conn = new SqlConnection(_connectionString);
        var result = await conn.ExecuteScalarAsync<int>(sql, new { BookingId = bookingId });
        return result == 1;
    }

    public async Task<decimal> CalculateSRFStipendAsync(long researchCategoryId, long rankId, CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT dbo.fn_CalculateSRFStipend(@ResearchCategoryId, @RankId)";
        await using var conn = new SqlConnection(_connectionString);
        return await conn.ExecuteScalarAsync<decimal>(sql, new { ResearchCategoryId = researchCategoryId, RankId = rankId });
    }

    public async Task<int> ProcessMonthlyStipendAsync(int month, int year, long processedBy, CancellationToken cancellationToken = default)
    {
        const string sql = "EXEC dbo.usp_ProcessSRFMonthlyStipend @Month, @Year, @ProcessedBy, @RowsProcessed OUTPUT";
        await using var conn = new SqlConnection(_connectionString);
        var parameters = new DynamicParameters();
        parameters.Add("@Month", month);
        parameters.Add("@Year", year);
        parameters.Add("@ProcessedBy", processedBy);
        parameters.Add("@RowsProcessed", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await conn.ExecuteAsync(sql, parameters);
        return parameters.Get<int>("@RowsProcessed");
    }
}
