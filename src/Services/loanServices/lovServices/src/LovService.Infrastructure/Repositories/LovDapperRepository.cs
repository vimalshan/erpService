using Dapper;
using Microsoft.Data.SqlClient;
using LovService.Application.DTOs;

namespace LovService.Infrastructure.Repositories;

/// <summary>
/// Dapper read-only query repository for fast projections
/// </summary>
public sealed class LovDapperRepository(string connectionString)
{
    public async Task<IEnumerable<LovMasterDto>> GetLovMastersByTypeIdAsync(int lovTypeId, CancellationToken ct)
    {
        const string sql = """
            SELECT m.LOV_ID         AS LovId,
                   m.LOV_TYPEID     AS LovTypeId,
                   m.LOV_NAME       AS LovName,
                   m.LOV_CREATEDON  AS LovCreatedOn,
                   m.LOV_CREATEDBY  AS LovCreatedBy,
                   m.LOV_UPDATEDBY  AS LovUpdatedBy,
                   m.LOV_UPDATEDON  AS LovUpdatedOn,
                   t.LOV_TYPENAME   AS LovTypeName
            FROM   LOV_MASTER m
            JOIN   LOV_TYPEMAST t ON t.LOV_TYPEID = m.LOV_TYPEID
            WHERE  m.LOV_TYPEID = @LovTypeId
            ORDER BY m.LOV_NAME;
            """;

        await using var conn = new SqlConnection(connectionString);
        var cmd = new CommandDefinition(sql, new { LovTypeId = lovTypeId }, cancellationToken: ct);
        return await conn.QueryAsync<LovMasterDto>(cmd);
    }

    public async Task<IEnumerable<ProgramLovMastDto>> GetProgramLovsByTypeCodeAsync(string prlovTypeCode, CancellationToken ct)
    {
        const string sql = """
            SELECT PRLOV_TYPECODE AS PrlovTypeCode,
                   PRLOV_CODE     AS PrlovCode,
                   PRLOV_NAME     AS PrlovName
            FROM   PROGRAMLOV_MAST
            WHERE  PRLOV_TYPECODE = @PrlovTypeCode
            ORDER BY PRLOV_NAME;
            """;

        await using var conn = new SqlConnection(connectionString);
        var cmd = new CommandDefinition(sql, new { PrlovTypeCode = prlovTypeCode }, cancellationToken: ct);
        return await conn.QueryAsync<ProgramLovMastDto>(cmd);
    }
}
