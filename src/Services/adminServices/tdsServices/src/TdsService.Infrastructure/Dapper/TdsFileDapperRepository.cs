using Dapper;
using Microsoft.Data.SqlClient;
using TdsService.Application.DTOs;

namespace TdsService.Infrastructure.Dapper;

public interface ITdsFileDapperRepository
{
    Task<IEnumerable<TdsFileDto>> GetFilesByPanAsync(string panNo, CancellationToken ct = default);
    Task<IEnumerable<TdsFileDto>> GetPendingEmailFilesAsync(CancellationToken ct = default);
}

public sealed class TdsFileDapperRepository : ITdsFileDapperRepository
{
    private readonly string _connectionString;

    public TdsFileDapperRepository(string connectionString)
        => _connectionString = connectionString;

    public async Task<IEnumerable<TdsFileDto>> GetFilesByPanAsync(
        string panNo,
        CancellationToken ct = default)
    {
        await using var connection = new SqlConnection(_connectionString);

        const string sql = """
            SELECT FILE_ID     AS FileId,
                   FILE_NAME   AS FileName,
                   PAN_NO      AS PanNo,
                   EMAIL_STATUS AS EmailStatus,
                   FILE_TYPE   AS FileType,
                   NULL        AS BlobStorageUri,
                   GETUTCDATE() AS CreatedAt,
                   NULL        AS UpdatedAt
            FROM   TDSFILE_DETAILS
            WHERE  PAN_NO = @PanNo
            ORDER  BY FILE_ID
            """;

        var cmd = new CommandDefinition(sql, new { PanNo = panNo }, cancellationToken: ct);
        return await connection.QueryAsync<TdsFileDto>(cmd);
    }

    public async Task<IEnumerable<TdsFileDto>> GetPendingEmailFilesAsync(
        CancellationToken ct = default)
    {
        await using var connection = new SqlConnection(_connectionString);

        const string sql = """
            SELECT FILE_ID     AS FileId,
                   FILE_NAME   AS FileName,
                   PAN_NO      AS PanNo,
                   EMAIL_STATUS AS EmailStatus,
                   FILE_TYPE   AS FileType,
                   NULL        AS BlobStorageUri,
                   GETUTCDATE() AS CreatedAt,
                   NULL        AS UpdatedAt
            FROM   TDSFILE_DETAILS
            WHERE  EMAIL_STATUS = 'N' OR EMAIL_STATUS IS NULL
            ORDER  BY FILE_ID
            """;

        var cmd = new CommandDefinition(sql, cancellationToken: ct);
        return await connection.QueryAsync<TdsFileDto>(cmd);
    }
}
