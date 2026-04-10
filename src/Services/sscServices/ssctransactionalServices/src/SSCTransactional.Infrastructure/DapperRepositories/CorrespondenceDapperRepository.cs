using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SSCTransactional.Application.DTOs;

namespace SSCTransactional.Infrastructure.DapperRepositories;

public class CorrespondenceDapperRepository
{
    private readonly string _connectionString;

    public CorrespondenceDapperRepository(IConfiguration configuration)
        => _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection not configured.");

    private SqlConnection CreateConnection() => new(_connectionString);

    private const string CorrespondenceColumns = """
            c.CORR_ID          as CorrespondenceId,
            c.CORR_DOCID       as DocId,
            c.CORR_ALLID       as AllocationId,
            c.CORR_HOLDCAT     as HoldCategory,
            c.CORR_HOLDTYPE    as HoldType,
            c.CORR_HOLDDATE    as HoldDate,
            c.CORR_HOLDREMARKS as HoldRemarks,
            c.CORR_HOLDBY      as HoldBy,
            c.CORR_HOLDSTATUS  as HoldStatus,
            c.CORR_RELDATE     as ReleaseDate,
            c.CORR_RELREMARKS  as ReleaseRemarks,
            c.CORR_RELBY       as ReleasedBy,
            c.CORR_HOLDNATURE  as HoldNature
        """;

    public async Task<IEnumerable<CorrespondenceFlatDto>> GetCorrespondencesPagedAsync(int page = 1, int pageSize = 20)
    {
        using var conn = CreateConnection();
        var sql = $"""
            SELECT {CorrespondenceColumns}
            FROM DOC_CORRESPOND c
            ORDER BY c.CORR_HOLDDATE DESC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
            """;

        return await conn.QueryAsync<CorrespondenceFlatDto>(sql, new { Offset = (page - 1) * pageSize, PageSize = pageSize });
    }

    public async Task<IEnumerable<CorrespondenceFlatDto>> GetCorrespondencesByDocIdAsync(long docId)
    {
        using var conn = CreateConnection();
        var sql = $"""
            SELECT {CorrespondenceColumns}
            FROM DOC_CORRESPOND c
            WHERE c.CORR_DOCID = @DocId
            ORDER BY c.CORR_HOLDDATE DESC
            """;

        return await conn.QueryAsync<CorrespondenceFlatDto>(sql, new { DocId = docId });
    }

    public async Task<IEnumerable<CorrespondenceAttachmentFlatDto>> GetAttachmentsByCorrespondenceIdAsync(long correspondenceId)
    {
        using var conn = CreateConnection();
        const string sql = """
            SELECT a.ATT_ID          as AttachmentId,
                   a.ATT_CORRID      as CorrespondenceId,
                   a.ATT_CORRSTATUS  as Status,
                   a.ATT_FILEPATH    as FilePath
            FROM DOC_CORRESPONDATT a
            WHERE a.ATT_CORRID = @CorrespondenceId
            """;

        return await conn.QueryAsync<CorrespondenceAttachmentFlatDto>(sql, new { CorrespondenceId = correspondenceId });
    }

    public async Task<int> GetActiveHoldCountAsync()
    {
        using var conn = CreateConnection();
        return await conn.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM DOC_CORRESPOND WHERE CORR_HOLDSTATUS = 'H'");
    }
}
