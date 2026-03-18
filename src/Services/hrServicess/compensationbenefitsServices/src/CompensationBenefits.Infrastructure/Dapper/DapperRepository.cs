using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CompensationBenefits.Infrastructure.Dapper;

public interface IDapperRepository
{
    Task<IEnumerable<dynamic>> GetEmployeeCTCBreakdownAsync(long empSysId);
    Task<IEnumerable<dynamic>> GetStructureDetailsAsync(long structureId);
}

/// <summary>Low-level Dapper read-model queries for reporting and bulk fetches.</summary>
public class DapperRepository(IConfiguration configuration) : IDapperRepository
{
    private SqlConnection CreateConnection()
        => new(configuration.GetConnectionString("DefaultConnection"));

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null)
    {
        await using var conn = CreateConnection();
        await conn.OpenAsync();
        return await conn.QueryAsync<T>(sql, parameters);
    }

    public async Task<T?> QuerySingleOrDefaultAsync<T>(string sql, object? parameters = null)
    {
        await using var conn = CreateConnection();
        await conn.OpenAsync();
        return await conn.QuerySingleOrDefaultAsync<T>(sql, parameters);
    }

    /// <summary>Get salary breakdown for an employee via TEVCTC for reporting.</summary>
    public async Task<IEnumerable<dynamic>> GetEmployeeCTCBreakdownAsync(long empSysId)
    {
        const string sql = """
            SELECT CTC_ED_ID, CTC_ED_FREQ, CTC_ED_AMTPA, CTC_EFF_DAT, CTC_CLS_DAT
            FROM TEVCTC
            WHERE CTC_EMP_SYSID = @EmpSysId AND CTC_CLS_DAT IS NULL
            ORDER BY CTC_ED_ID
            """;
        return await QueryAsync<dynamic>(sql, new { EmpSysId = empSysId });
    }

    /// <summary>Get salary structure details with ED names via raw SQL join.</summary>
    public async Task<IEnumerable<dynamic>> GetStructureDetailsAsync(long structureId)
    {
        const string sql = """
            SELECT sd.STRUCTDET_EDID, sd.STRUCTDET_CATEGORY, sd.STRUCTDET_EDAMT,
                   sd.STRUCTDET_FREQUENCY, sd.STRUCTDET_MINVALUE, sd.STRUCTDET_MAXVALUE
            FROM SALSTRUCTURE_DET sd
            WHERE sd.STRUCTDET_STRUCTUREID = @StructureId
            ORDER BY sd.STRUCTDET_ID
            """;
        return await QueryAsync<dynamic>(sql, new { StructureId = structureId });
    }
}
