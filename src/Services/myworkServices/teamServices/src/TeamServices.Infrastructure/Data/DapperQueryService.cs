using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TeamServices.Application.DTOs;

namespace TeamServices.Infrastructure.Data;

public class DapperQueryService
{
    private readonly string _connectionString;

    public DapperQueryService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    public async Task<IEnumerable<TeamDto>> GetAllTeamsAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        const string sql = @"
            SELECT TEAM_ID AS TeamId, TEAM_NAME AS TeamName, 
                   TEAM_LASTMODIFIEDBY AS LastModifiedBy, TEAM_LASTMODIFIEDON AS LastModifiedOn
            FROM TEAM_MASTER";
        return await connection.QueryAsync<TeamDto>(sql);
    }

    public async Task<TeamDto?> GetTeamByIdAsync(long teamId)
    {
        using var connection = new SqlConnection(_connectionString);
        const string sql = @"
            SELECT TEAM_ID AS TeamId, TEAM_NAME AS TeamName, 
                   TEAM_LASTMODIFIEDBY AS LastModifiedBy, TEAM_LASTMODIFIEDON AS LastModifiedOn
            FROM TEAM_MASTER WHERE TEAM_ID = @TeamId";
        return await connection.QueryFirstOrDefaultAsync<TeamDto>(sql, new { TeamId = teamId });
    }

    public async Task<IEnumerable<TeamEmployeeMapDto>> GetTeamEmployeesAsync(long teamId)
    {
        using var connection = new SqlConnection(_connectionString);
        const string sql = @"
            SELECT TEAMEMP_ID AS Id, TEAMEMP_TEAMID AS TeamId, TEAMEMP_EMPSYSID AS EmployeeSysId,
                   TEAMEMP_EFFDATE AS EffectiveDate, TEAMEMP_CLOSEDATE AS CloseDate,
                   TEAMEMP_LASTMODIFIEDBY AS LastModifiedBy, TEAMEMP_LASTMODIFIEDON AS LastModifiedOn
            FROM TEAM_EMPMAP WHERE TEAMEMP_TEAMID = @TeamId";
        return await connection.QueryAsync<TeamEmployeeMapDto>(sql, new { TeamId = teamId });
    }

    public async Task<IEnumerable<TeamUnitMapDto>> GetTeamUnitMapsAsync(long teamId)
    {
        using var connection = new SqlConnection(_connectionString);
        const string sql = @"
            SELECT TEAM_MAPID AS MapId, TEAM_ID AS TeamId, TEAM_UNITID AS UnitId,
                   TEAM_GRADECATEGORY AS GradeCategory, TEAM_CADREID AS CadreId,
                   TEAM_LASTMODIFIEDBY AS LastModifiedBy, TEAM_LASTMODIFIEDON AS LastModifiedOn
            FROM TEAM_UNITMAP WHERE TEAM_ID = @TeamId";
        return await connection.QueryAsync<TeamUnitMapDto>(sql, new { TeamId = teamId });
    }
}
