using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MobileAppManagement.Application.DTOs;
using MobileAppManagement.Application.Interfaces;

namespace MobileAppManagement.Infrastructure.Dapper;

public class DapperQueryService(IConfiguration configuration) : IDapperQueryService
{
    private IDbConnection CreateConnection() =>
        new SqlConnection(configuration.GetConnectionString("DefaultConnection"));

    public async Task<IEnumerable<AppDeviceDetailDto>> GetDevicesByEmployeeAsync(decimal employeeSysId,
        CancellationToken ct = default)
    {
        using var connection = CreateConnection();
        const string sql = """
            SELECT MD_EMPSYSID AS EmployeeSysId, MD_DEVICEID AS DeviceId, MD_ACTIVE AS Active,
                   MD_DEVICETYPE AS DeviceType, MD_IMEINO AS ImeiNo, MD_CREATEDON AS CreatedOn,
                   MD_UPDATEDON AS UpdatedOn
            FROM MOB_APPDEVICE_DETAILS WHERE MD_EMPSYSID = @EmpSysId ORDER BY MD_UPDATEDON DESC
            """;
        return await connection.QueryAsync<AppDeviceDetailDto>(
            new CommandDefinition(sql, new { EmpSysId = employeeSysId }, cancellationToken: ct));
    }

    public async Task<IEnumerable<LoginDetailDto>> GetLoginsByUserAsync(decimal userSysId,
        CancellationToken ct = default)
    {
        using var connection = CreateConnection();
        const string sql = """
            SELECT LD_LOGINID AS LoginId, LD_USERSYSID AS UserSysId, LD_DEVICEID AS DeviceId,
                   LD_LOGON AS Logon, LD_GUID AS Guid, LD_IMEINO AS ImeiNo, LD_DEVICETYPE AS DeviceType
            FROM MOB_LOGINDET WHERE LD_USERSYSID = @UserSysId ORDER BY LD_LOGON DESC
            """;
        return await connection.QueryAsync<LoginDetailDto>(
            new CommandDefinition(sql, new { UserSysId = userSysId }, cancellationToken: ct));
    }

    public async Task<string> RegisterDeviceViaProcAsync(decimal empSysId, string deviceId,
        char deviceType, string? imeiNo, decimal updatedBy, CancellationToken ct = default)
    {
        using var connection = CreateConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@p_EmpSysId", empSysId, DbType.Decimal);
        parameters.Add("@p_DeviceId", deviceId, DbType.String, size: 200);
        parameters.Add("@p_DeviceType", deviceType.ToString(), DbType.StringFixedLength, size: 1);
        parameters.Add("@p_ImeiNo", imeiNo, DbType.String, size: 200);
        parameters.Add("@p_UpdatedBy", updatedBy, DbType.Decimal);
        parameters.Add("@p_ErrorMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

        await connection.ExecuteAsync(
            new CommandDefinition("usp_MOB_RegisterDevice", parameters,
                commandType: CommandType.StoredProcedure, cancellationToken: ct));

        return parameters.Get<string>("@p_ErrorMessage");
    }

    public async Task<(decimal LoginId, string Message)> LogUserLoginViaProcAsync(decimal userSysId,
        string? deviceId, string? imeiNo, char? deviceType, CancellationToken ct = default)
    {
        using var connection = CreateConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@p_UserSysId", userSysId, DbType.Decimal);
        parameters.Add("@p_DeviceId", deviceId, DbType.String, size: 200);
        parameters.Add("@p_ImeiNo", imeiNo, DbType.String, size: 200);
        parameters.Add("@p_DeviceType", deviceType?.ToString(), DbType.StringFixedLength, size: 1);
        parameters.Add("@p_LoginId", dbType: DbType.Decimal, direction: ParameterDirection.Output);
        parameters.Add("@p_ErrorMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

        await connection.ExecuteAsync(
            new CommandDefinition("usp_MOB_LogUserLogin", parameters,
                commandType: CommandType.StoredProcedure, cancellationToken: ct));

        return (parameters.Get<decimal>("@p_LoginId"), parameters.Get<string>("@p_ErrorMessage"));
    }
}
