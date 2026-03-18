using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using UserSecurityService.Application.DTOs;

namespace UserSecurityService.Infrastructure.Dapper;

/// <summary>Provides read-optimised queries via Dapper for complex reporting scenarios.</summary>
public class DapperUserRepository(IConfiguration configuration)
{
    private string ConnectionString => configuration.GetConnectionString("DefaultConnection")!;

    public async Task<IEnumerable<UserProfileDto>> SearchUsersAsync(
        string? nameFilter, string? unitCode, int pageSize, int pageNumber, CancellationToken ct = default)
    {
        var offset = (pageNumber - 1) * pageSize;
        const string sql = """
            SELECT EM_USR_ID         AS UserId,
                   EM_EMP_NUM        AS EmpNum,
                   EM_UNT_COD        AS UnitCode,
                   EM_NICK_NAM       AS NickName,
                   EM_USR_TYP        AS UserType,
                   EM_EML_FLG        AS EmailFlag,
                   EM_OEML_ID        AS OfficeEmail,
                   EM_PEML_ID        AS PersonalEmail,
                   EM_EFF_DAT        AS EffectiveDate,
                   EM_CLS_DAT        AS CloseDate,
                   EM_EMP_NAM        AS EmpName,
                   EM_FRS_NAM        AS FirstName,
                   EM_MID_NAM        AS MiddleName,
                   EM_LST_NAM        AS LastName,
                   EM_EMP_DSG        AS Designation,
                   EM_DIV_NAM        AS Division,
                   EM_PHT_PTH        AS PhotoPath,
                   EM_REGSTATUS      AS RegStatus
            FROM   USER_PROFILE_PFS
            WHERE  EM_CLS_DAT IS NULL
              AND  (@NameFilter IS NULL OR EM_EMP_NAM LIKE '%' + @NameFilter + '%')
              AND  (@UnitCode   IS NULL OR EM_UNT_COD = @UnitCode)
            ORDER BY EM_EMP_NAM
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
            """;

        using var connection = new SqlConnection(ConnectionString);
        var cmd = new CommandDefinition(sql,
            new { NameFilter = nameFilter, UnitCode = unitCode, Offset = offset, PageSize = pageSize },
            cancellationToken: ct);

        return await connection.QueryAsync<UserProfileDto>(cmd);
    }

    public async Task<IEnumerable<UserAppsMappingDto>> GetUserRolesByAppAsync(string appCode, CancellationToken ct = default)
    {
        const string sql = """
            SELECT USER_EMPSYSID  AS EmpSysId,
                   USER_APPS      AS AppCode,
                   USER_EFFDATE   AS EffectiveDate,
                   USER_CLSDATE   AS CloseDate,
                   USER_HRROLEID  AS HrRoleId,
                   USER_REMARKS   AS Remarks
            FROM   USER_APPSMAP
            WHERE  USER_APPS = @AppCode
              AND  USER_CLSDATE IS NULL;
            """;

        using var connection = new SqlConnection(ConnectionString);
        return await connection.QueryAsync<UserAppsMappingDto>(new CommandDefinition(sql, new { AppCode = appCode }, cancellationToken: ct));
    }
}
