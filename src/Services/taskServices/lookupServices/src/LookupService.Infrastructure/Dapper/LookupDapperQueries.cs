using Dapper;
using LookupService.Application.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace LookupService.Infrastructure.Dapper;

public class LookupDapperQueries(IConfiguration configuration)
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")!;

    public async Task<IEnumerable<LovMasterDto>> GetAllLovsAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<LovMasterDto>(
            "SELECT LOV_ID AS LovId, LOV_TYPE AS LovType, LOV_NAME AS LovName FROM LOV_MASTER");
    }

    public async Task<IEnumerable<LovMasterDto>> GetLovsByTypeAsync(string lovType)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<LovMasterDto>(
            "SELECT LOV_ID AS LovId, LOV_TYPE AS LovType, LOV_NAME AS LovName FROM LOV_MASTER WHERE LOV_TYPE = @LovType",
            new { LovType = lovType });
    }

    public async Task<IEnumerable<ProcessMasterDto>> GetAllProcessesAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<ProcessMasterDto>(
            "SELECT PROCESS_ID AS ProcessId, PROCESS_NAME AS ProcessName, PROCESS_LIVFLAG AS ProcessLivFlag FROM PROCESS_MASTER");
    }

    public async Task<IEnumerable<UnitProcessMapDto>> GetUnitProcessMapsByUnitAsync(string unitCode)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<UnitProcessMapDto>(
            "SELECT UP_MAPID AS UpMapId, UP_UNIT_CODE AS UpUnitCode, UP_PROCESS_ID AS UpProcessId FROM UNIT_PROCESS_MAP WHERE UP_UNIT_CODE = @UnitCode",
            new { UnitCode = unitCode });
    }

    public async Task<IEnumerable<LovUnitMapDto>> GetLovUnitMapsByLovAsync(long lovId)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<LovUnitMapDto>(
            "SELECT LU_MAPID AS LuMapId, LU_LOVID AS LuLovId, LU_UNITCODE AS LuUnitCode, LU_FLAG AS LuFlag FROM LOV_UNITMAP WHERE LU_LOVID = @LovId",
            new { LovId = lovId });
    }
}
