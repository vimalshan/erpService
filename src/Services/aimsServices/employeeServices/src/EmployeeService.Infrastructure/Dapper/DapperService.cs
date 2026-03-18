using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace EmployeeService.Infrastructure.Dapper;

/// <summary>Thin Dapper wrapper for complex read queries bypassing EF.</summary>
public sealed class DapperService
{
    private readonly string _connectionString;

    public DapperService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("EmployeeDb")
            ?? throw new InvalidOperationException("Connection string 'EmployeeDb' not found.");
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null, CancellationToken ct = default)
    {
        await using var conn = new SqlConnection(_connectionString);
        return await conn.QueryAsync<T>(new CommandDefinition(sql, param, cancellationToken: ct));
    }

    public async Task<T?> QuerySingleOrDefaultAsync<T>(string sql, object? param = null, CancellationToken ct = default)
    {
        await using var conn = new SqlConnection(_connectionString);
        return await conn.QuerySingleOrDefaultAsync<T>(new CommandDefinition(sql, param, cancellationToken: ct));
    }

    public async Task<int> ExecuteAsync(string sql, object? param = null, CancellationToken ct = default)
    {
        await using var conn = new SqlConnection(_connectionString);
        return await conn.ExecuteAsync(new CommandDefinition(sql, param, cancellationToken: ct));
    }

    /// <summary>Executes usp_AssignApprover stored procedure via Dapper.</summary>
    public async Task<int> AssignApproverProcAsync(long empSysId, long approverSysId, int level, long assignedBy, CancellationToken ct = default)
    {
        await using var conn = new SqlConnection(_connectionString);
        var p = new DynamicParameters();
        p.Add("@p_EmpSysID", empSysId);
        p.Add("@p_ApproverSysID", approverSysId);
        p.Add("@p_Level", level);
        p.Add("@p_AssignedBy", assignedBy);
        p.Add("@p_ApproverID", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

        await conn.ExecuteAsync(new CommandDefinition("dbo.usp_AssignApprover", p,
            commandType: System.Data.CommandType.StoredProcedure, cancellationToken: ct));

        return p.Get<int>("@p_ApproverID");
    }

    /// <summary>Executes usp_MapEmployeeCalendar stored procedure via Dapper.</summary>
    public async Task<long> MapEmployeeCalendarProcAsync(long empSysId, int calendarId, long mappedBy, CancellationToken ct = default)
    {
        await using var conn = new SqlConnection(_connectionString);
        var p = new DynamicParameters();
        p.Add("@p_EmpSysID", empSysId);
        p.Add("@p_CalendarID", calendarId);
        p.Add("@p_MappedBy", mappedBy);
        p.Add("@p_CalendarMapID", dbType: System.Data.DbType.Int64, direction: System.Data.ParameterDirection.Output);

        await conn.ExecuteAsync(new CommandDefinition("dbo.usp_MapEmployeeCalendar", p,
            commandType: System.Data.CommandType.StoredProcedure, cancellationToken: ct));

        return p.Get<long>("@p_CalendarMapID");
    }
}
