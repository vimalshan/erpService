using AuditLogService.Application.DTOs;
using Dapper;
using Microsoft.Data.SqlClient;

namespace AuditLogService.Infrastructure.Persistence.Dapper;

public class AuditLogDapperRepository
{
    private readonly string _connectionString;

    public AuditLogDapperRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<AuditLogDto>> GetRecentLogsAsync(int count)
    {
        const string sql = @"
            SELECT TOP (@Count)
                log_id AS LogId,
                table_name AS TableName,
                record_id AS RecordId,
                action AS Action,
                changed_by AS ChangedBy,
                change_date AS ChangeDate,
                old_values AS OldValues,
                new_values AS NewValues
            FROM AuditLog
            ORDER BY change_date DESC";

        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<AuditLogDto>(sql, new { Count = count });
    }

    public async Task<IEnumerable<AuditLogDto>> SearchLogsAsync(string? tableName, string? action, DateTime? fromDate, DateTime? toDate)
    {
        var sql = @"
            SELECT 
                log_id AS LogId,
                table_name AS TableName,
                record_id AS RecordId,
                action AS Action,
                changed_by AS ChangedBy,
                change_date AS ChangeDate,
                old_values AS OldValues,
                new_values AS NewValues
            FROM AuditLog
            WHERE 1=1";

        var parameters = new DynamicParameters();

        if (!string.IsNullOrEmpty(tableName))
        {
            sql += " AND table_name = @TableName";
            parameters.Add("TableName", tableName);
        }

        if (!string.IsNullOrEmpty(action))
        {
            sql += " AND action = @Action";
            parameters.Add("Action", action);
        }

        if (fromDate.HasValue)
        {
            sql += " AND change_date >= @FromDate";
            parameters.Add("FromDate", fromDate.Value);
        }

        if (toDate.HasValue)
        {
            sql += " AND change_date <= @ToDate";
            parameters.Add("ToDate", toDate.Value);
        }

        sql += " ORDER BY change_date DESC";

        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<AuditLogDto>(sql, parameters);
    }
}
