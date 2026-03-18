using Dapper;
using Microsoft.Data.SqlClient;
using TdsService.Application.DTOs;

namespace TdsService.Infrastructure.Dapper;

public interface ITdsVendorDapperRepository
{
    Task<IEnumerable<TdsVendorDto>> SearchByNameOrPanAsync(string searchTerm, CancellationToken ct = default);
    Task<IEnumerable<TdsVendorDto>> GetVendorsByPanListAsync(IEnumerable<string> panNumbers, CancellationToken ct = default);
}

public sealed class TdsVendorDapperRepository : ITdsVendorDapperRepository
{
    private readonly string _connectionString;

    public TdsVendorDapperRepository(string connectionString)
        => _connectionString = connectionString;

    public async Task<IEnumerable<TdsVendorDto>> SearchByNameOrPanAsync(
        string searchTerm,
        CancellationToken ct = default)
    {
        await using var connection = new SqlConnection(_connectionString);

        const string sql = """
            SELECT VENDOR_ID   AS VendorId,
                   VENDOR_NAME AS VendorName,
                   EMAIL_ADDRESS AS EmailAddress,
                   PAN_NO      AS PanNo
            FROM   TDS_VENDORS
            WHERE  VENDOR_NAME LIKE @Pattern
               OR  PAN_NO      LIKE @Pattern
            ORDER  BY VENDOR_NAME
            """;

        var cmd = new CommandDefinition(
            sql,
            new { Pattern = $"%{searchTerm}%" },
            cancellationToken: ct);

        return await connection.QueryAsync<TdsVendorDto>(cmd);
    }

    public async Task<IEnumerable<TdsVendorDto>> GetVendorsByPanListAsync(
        IEnumerable<string> panNumbers,
        CancellationToken ct = default)
    {
        await using var connection = new SqlConnection(_connectionString);

        const string sql = """
            SELECT VENDOR_ID   AS VendorId,
                   VENDOR_NAME AS VendorName,
                   EMAIL_ADDRESS AS EmailAddress,
                   PAN_NO      AS PanNo
            FROM   TDS_VENDORS
            WHERE  PAN_NO IN @PanNumbers
            """;

        var cmd = new CommandDefinition(
            sql,
            new { PanNumbers = panNumbers },
            cancellationToken: ct);

        return await connection.QueryAsync<TdsVendorDto>(cmd);
    }
}
