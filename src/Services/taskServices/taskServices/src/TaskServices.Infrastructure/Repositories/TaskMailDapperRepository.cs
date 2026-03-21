using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TaskServices.Application.DTOs;

namespace TaskServices.Infrastructure.Repositories;

public class TaskMailDapperRepository
{
    private readonly string _connectionString;

    public TaskMailDapperRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("TaskDb")
            ?? throw new InvalidOperationException("Connection string 'TaskDb' not configured.");
    }

    public async Task<IEnumerable<TaskMailDto>> GetAllAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<TaskMailDto>("SELECT MID, SYSID FROM TASK_MAIL");
    }

    public async Task<TaskMailDto?> GetByIdAsync(decimal mid)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<TaskMailDto>(
            "SELECT MID, SYSID FROM TASK_MAIL WHERE MID = @MID",
            new { MID = mid });
    }

    public async Task CreateViaStoredProcAsync(decimal mailId, decimal sysId)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync(
            "dbo.usp_TASK_CreateTaskMail",
            new { p_MailID = mailId, p_SysID = sysId },
            commandType: System.Data.CommandType.StoredProcedure);
    }
}
