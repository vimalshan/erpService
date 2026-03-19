using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using ApprovalGroup.Application.DTOs;

namespace ApprovalGroup.Infrastructure.Repositories;

public interface IApprovalGroupDapperQuery
{
    Task<IEnumerable<ApprovalGroupDto>> GetApprovalGroupsPagedAsync(int page, int pageSize, CancellationToken ct = default);
    Task<IEnumerable<PullMatrixDetailDto>> GetPullMatrixByMainCatAsync(long mainCat, CancellationToken ct = default);
}

public class ApprovalGroupDapperQuery : IApprovalGroupDapperQuery
{
    private readonly string _connectionString;

    public ApprovalGroupDapperQuery(string connectionString) => _connectionString = connectionString;

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<IEnumerable<ApprovalGroupDto>> GetApprovalGroupsPagedAsync(int page, int pageSize, CancellationToken ct = default)
    {
        const string sql = """
            SELECT GROUP_ID AS GroupId, GROUP_NAME AS GroupName, 
                   GROUP_CREATEDBY AS GroupCreatedBy, GROUP_CREATEDON AS GroupCreatedOn,
                   GROUP_PRIORITYID AS GroupPriorityId
            FROM APGROUP_MAST
            ORDER BY GROUP_ID
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
            """;

        using var conn = CreateConnection();
        var result = await conn.QueryAsync<ApprovalGroupDto>(sql,
            new { Offset = (page - 1) * pageSize, PageSize = pageSize });
        return result;
    }

    public async Task<IEnumerable<PullMatrixDetailDto>> GetPullMatrixByMainCatAsync(long mainCat, CancellationToken ct = default)
    {
        const string sql = """
            SELECT MAT_ID AS MatId, MAT_UNITID AS MatUnitId, MAT_PAYBY AS MatPayBy,
                   MAT_FLAG AS MatFlag, MAT_MAINCAT AS MatMainCat, MAT_EMPSYSID AS MatEmpSysId,
                   MAT_MAXNOS AS MatMaxNos
            FROM PULLMATRIX_DET
            WHERE MAT_MAINCAT = @MainCat
            """;

        using var conn = CreateConnection();
        return await conn.QueryAsync<PullMatrixDetailDto>(sql, new { MainCat = mainCat });
    }
}
