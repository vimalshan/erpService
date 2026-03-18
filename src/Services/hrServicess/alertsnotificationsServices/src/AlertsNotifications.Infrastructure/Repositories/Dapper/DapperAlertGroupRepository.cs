using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AlertsNotifications.Infrastructure.Repositories.Dapper;

public class DapperAlertGroupRepository
{
    private readonly string _connectionString;

    public DapperAlertGroupRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task SetupAlertGroupAsync(decimal alertGroupId, string alertGroupName, char alertGroupType, long createdBy)
    {
        using var connection = CreateConnection();
        await connection.ExecuteAsync(
            "usp_SetupAlertGroup",
            new
            {
                ALGRP_ID = alertGroupId,
                ALGRP_NAME = alertGroupName,
                ALGRP_TYPE = alertGroupType,
                ALGRP_CREATEDBY = createdBy
            },
            commandType: CommandType.StoredProcedure);
    }
}
