using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace TravelService.Infrastructure.Services;

/// <summary>Dapper-based read-side query service for complex/reporting queries.</summary>
public class TravelQueryService
{
    private readonly string _connectionString;

    public TravelQueryService(IConfiguration configuration)
        => _connectionString = configuration.GetConnectionString("TravelDb")
            ?? throw new InvalidOperationException("Connection string 'TravelDb' not found.");

    public async Task<IEnumerable<TourPlanSummary>> GetTourPlanSummaryAsync(
        string? employeeSysId = null, CancellationToken cancellationToken = default)
    {
        using var conn = new SqlConnection(_connectionString);
        var sql = employeeSysId is null
            ? "SELECT TP_ID AS Id, TP_EMPSYSID AS EmployeeSysId, TP_STATUS AS Status, TP_STARTDATE AS StartDate, TP_PURPOSE AS Purpose FROM TOURPLAN_MAIN ORDER BY TP_CREATEDON DESC"
            : "SELECT TP_ID AS Id, TP_EMPSYSID AS EmployeeSysId, TP_STATUS AS Status, TP_STARTDATE AS StartDate, TP_PURPOSE AS Purpose FROM TOURPLAN_MAIN WHERE TP_EMPSYSID = @EmployeeSysId ORDER BY TP_CREATEDON DESC";

        return await conn.QueryAsync<TourPlanSummary>(sql, new { EmployeeSysId = employeeSysId });
    }

    public async Task<IEnumerable<BatchSummary>> GetBatchSummaryAsync(
        string? status = null, CancellationToken cancellationToken = default)
    {
        using var conn = new SqlConnection(_connectionString);
        var sql = status is null
            ? "SELECT BATCH_ID AS Id, BATCH_ADMINID AS AdminId, BATCH_STATUS AS Status, BATCH_BATCHDATE AS BatchDate, BATCH_TOTPAY AS TotalPayable FROM TRAVEL_BATCHMAIN ORDER BY BATCH_CREATEDON DESC"
            : "SELECT BATCH_ID AS Id, BATCH_ADMINID AS AdminId, BATCH_STATUS AS Status, BATCH_BATCHDATE AS BatchDate, BATCH_TOTPAY AS TotalPayable FROM TRAVEL_BATCHMAIN WHERE BATCH_STATUS = @Status ORDER BY BATCH_CREATEDON DESC";

        return await conn.QueryAsync<BatchSummary>(sql, new { Status = status });
    }
}

public record TourPlanSummary(string Id, string EmployeeSysId, string Status, DateTime StartDate, string Purpose);
public record BatchSummary(string Id, string AdminId, string Status, DateTime BatchDate, string TotalPayable);
