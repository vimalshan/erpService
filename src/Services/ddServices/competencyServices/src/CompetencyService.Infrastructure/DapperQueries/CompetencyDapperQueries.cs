using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using CompetencyService.Application.DTOs;

namespace CompetencyService.Infrastructure.DapperQueries;

/// <summary>Read-heavy queries using Dapper for performance.</summary>
public class CompetencyDapperQueries(string connectionString)
{
    private IDbConnection CreateConnection() => new SqlConnection(connectionString);

    public async Task<IEnumerable<CompetencyDto>> GetCompetenciesPagedAsync(int page, int pageSize)
    {
        const string sql = """
            SELECT CM_CPD_NUM AS Id, CM_CPD_NAM AS Name, CM_EFF_DAT AS EffectiveDate,
                   CM_CLS_DAT AS ClosureDate, CM_CPD_REM AS Remarks, CM_JOB_COD AS JobCode,
                   CM_POS_IND AS PositiveIndicator, CM_NEG_IND AS NegativeIndicator,
                   CM_CPD_SLF AS SelfDescription, CM_CPD_TYPE AS CompetencyType,
                   CM_PARENTID AS ParentId
            FROM DD_COMPENDMAST
            ORDER BY CM_CPD_NUM
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
            """;
        using var conn = CreateConnection();
        return await conn.QueryAsync<CompetencyDto>(sql,
            new { Offset = (page - 1) * pageSize, PageSize = pageSize });
    }

    public async Task<IEnumerable<CompetencyIndicatorDto>> GetIndicatorsByCompetencyAsync(decimal competencyId)
    {
        const string sql = """
            SELECT SRL_NO AS SerialNo, BAND AS Band, COMP_NUM AS CompetencyNo,
                   IND_FLAG AS IndicatorFlag, IND_DEFN AS IndicatorDefinition
            FROM DD_COMPETENCY_IND
            WHERE COMP_NUM = @CompetencyId
            """;
        using var conn = CreateConnection();
        return await conn.QueryAsync<CompetencyIndicatorDto>(sql, new { CompetencyId = competencyId });
    }

    public async Task<IEnumerable<CompetencyDto>> SearchCompetenciesAsync(string searchTerm)
    {
        const string sql = """
            SELECT CM_CPD_NUM AS Id, CM_CPD_NAM AS Name, CM_EFF_DAT AS EffectiveDate,
                   CM_CLS_DAT AS ClosureDate, CM_CPD_REM AS Remarks, CM_JOB_COD AS JobCode,
                   CM_POS_IND AS PositiveIndicator, CM_NEG_IND AS NegativeIndicator,
                   CM_CPD_SLF AS SelfDescription, CM_CPD_TYPE AS CompetencyType,
                   CM_PARENTID AS ParentId
            FROM DD_COMPENDMAST
            WHERE CM_CPD_NAM LIKE @SearchTerm OR CM_CPD_REM LIKE @SearchTerm
            ORDER BY CM_CPD_NUM
            """;
        using var conn = CreateConnection();
        return await conn.QueryAsync<CompetencyDto>(sql, new { SearchTerm = $"%{searchTerm}%" });
    }
}
