using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using DevelopmentService.Domain.Entities;
using DevelopmentService.Domain.Interfaces;

namespace DevelopmentService.Infrastructure.Repositories;

public class CompetencyRepository : ICompetencyRepository
{
    private readonly string _connectionString;

    public CompetencyRepository(IConfiguration configuration)
        => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

    public async Task<IEnumerable<CompetencyInd>> GetIndicatorsAsync(
        long? compNum, string? band, CancellationToken ct = default)
    {
        await using var conn = new SqlConnection(_connectionString);
        var rows = await conn.QueryAsync<dynamic>(
            "usp_Development_GetCompetencyIndicators",
            new { p_CompNum = compNum, p_Band = band },
            commandType: System.Data.CommandType.StoredProcedure);

        return rows.Select(r => new CompetencyInd
        {
            SrlNo   = (decimal?)r.SRL_NO,
            Band    = (string?)r.BAND,
            CompNum = (long?)r.COMP_NUM,
            IndFlag = r.IND_FLAG is string s && s.Length > 0 ? s[0] : (char?)null,
            IndDefn = (string?)r.IND_DEFN
        });
    }
}
