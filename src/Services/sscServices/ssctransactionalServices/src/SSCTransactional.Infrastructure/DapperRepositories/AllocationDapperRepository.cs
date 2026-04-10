using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SSCTransactional.Application.DTOs;

namespace SSCTransactional.Infrastructure.DapperRepositories;

public class AllocationDapperRepository
{
    private readonly string _connectionString;

    public AllocationDapperRepository(IConfiguration configuration)
        => _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection not configured.");

    private SqlConnection CreateConnection() => new(_connectionString);

    private const string AllocationColumns = """
            a.APALL_ID        as AllocationId,
            a.APALL_DOCID     as DocId,
            a.APALL_ACTION    as Action,
            a.APALL_GROUPID   as GroupId,
            a.APALL_PULLSTATUS as PullStatus,
            a.APALL_PULLUSERID as PullUserId,
            a.APALL_PRIORITY  as Priority,
            a.APALL_ALLBY     as AllocatedBy,
            a.APALL_ALLON     as AllocatedOn,
            a.APALL_REMARKS   as Remarks,
            a.APALL_ACTIONFLAG as ActionFlag,
            a.APALL_ACTIONDATE as ActionDate,
            a.APALL_CORRID    as CorrespondenceId,
            a.APALL_DEFTYPE   as DefectType,
            a.APALL_CLOSEREMARKS as CloseRemarks,
            a.APALL_MODIFIEDBY as ModifiedBy,
            a.APALL_MODIFIEDON as ModifiedOn,
            a.APALL_PULLEDON  as PulledOn
        """;

    public async Task<IEnumerable<AllocationFlatDto>> GetAllocationsPagedAsync(int page = 1, int pageSize = 20)
    {
        using var conn = CreateConnection();
        var sql = $"""
            SELECT {AllocationColumns}
            FROM DOC_APALLDET a
            ORDER BY a.APALL_ALLON DESC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
            """;

        return await conn.QueryAsync<AllocationFlatDto>(sql, new { Offset = (page - 1) * pageSize, PageSize = pageSize });
    }

    public async Task<IEnumerable<AllocationFlatDto>> GetAllocationsByGroupAsync(long groupId, int page = 1, int pageSize = 20)
    {
        using var conn = CreateConnection();
        var sql = $"""
            SELECT {AllocationColumns}
            FROM DOC_APALLDET a
            WHERE a.APALL_GROUPID = @GroupId
            ORDER BY a.APALL_ALLON DESC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
            """;

        return await conn.QueryAsync<AllocationFlatDto>(sql, new { GroupId = groupId, Offset = (page - 1) * pageSize, PageSize = pageSize });
    }

    public async Task<int> GetPendingCountByGroupAsync(long groupId)
    {
        using var conn = CreateConnection();
        return await conn.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM DOC_APALLDET WHERE APALL_GROUPID = @GroupId AND APALL_ACTIONFLAG = 'N'",
            new { GroupId = groupId });
    }

    public async Task<IEnumerable<AllocationFlatDto>> GetAllocationsByDocIdAsync(long docId)
    {
        using var conn = CreateConnection();
        var sql = $"""
            SELECT {AllocationColumns}
            FROM DOC_APALLDET a
            WHERE a.APALL_DOCID = @DocId
            ORDER BY a.APALL_ALLON DESC
            """;

        return await conn.QueryAsync<AllocationFlatDto>(sql, new { DocId = docId });
    }
}
