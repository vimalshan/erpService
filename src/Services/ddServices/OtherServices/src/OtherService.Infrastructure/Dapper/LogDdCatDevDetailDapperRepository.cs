using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace OtherService.Infrastructure.Dapper;

/// <summary>
/// Lightweight read-side queries via Dapper for high-performance scenarios.
/// </summary>
public sealed class LogDdCatDevDetailDapperRepository
{
    private readonly string _connectionString;

    public LogDdCatDevDetailDapperRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection is not configured.");
    }

    public async Task<IEnumerable<dynamic>> GetAllRawAsync(CancellationToken ct = default)
    {
        await using var conn = new SqlConnection(_connectionString);
        var result = await conn.QueryAsync(
            "SELECT * FROM [LOG_DD_CAT_DEV_DETAIL]",
            commandTimeout: 30);
        return result;
    }

    public async Task<IEnumerable<dynamic>> GetByReqNumRawAsync(
        decimal reqNum, CancellationToken ct = default)
    {
        await using var conn = new SqlConnection(_connectionString);
        var result = await conn.QueryAsync(
            "SELECT * FROM [LOG_DD_CAT_DEV_DETAIL] WHERE [CT_REQ_NUM] = @ReqNum",
            new { ReqNum = reqNum },
            commandTimeout: 30);
        return result;
    }

    public async Task<dynamic?> GetByKeyRawAsync(
        string appId, decimal appNum, CancellationToken ct = default)
    {
        await using var conn = new SqlConnection(_connectionString);
        return await conn.QueryFirstOrDefaultAsync(
            "SELECT * FROM [LOG_DD_CAT_DEV_DETAIL] WHERE [CT_APP_ID] = @AppId AND [CT_APP_NUM] = @AppNum",
            new { AppId = appId, AppNum = appNum },
            commandTimeout: 30);
    }
}
