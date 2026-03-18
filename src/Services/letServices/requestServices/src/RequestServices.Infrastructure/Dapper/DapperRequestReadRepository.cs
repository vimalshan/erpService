using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RequestServices.Application.DTOs;

namespace RequestServices.Infrastructure.Dapper;

/// <summary>
/// Dapper-based read-side repository for complex query operations
/// that benefit from raw SQL performance.
/// </summary>
public class DapperRequestReadRepository(IConfiguration configuration)
{
    private SqlConnection CreateConnection()
        => new(configuration.GetConnectionString("DefaultConnection"));

    public async Task<RequestMainDto?> GetRequestDetailsAsync(long requestId, CancellationToken ct = default)
    {
        using var conn = CreateConnection();
        await conn.OpenAsync(ct);

        var results = await conn.QueryMultipleAsync(
            "EXEC dbo.usp_Request_GetRequestDetails @p_ReqID",
            new { p_ReqID = requestId });

        var rows = (await results.ReadAsync<dynamic>()).ToList();
        if (!rows.Any()) return null;

        var first = rows.First();
        var subs  = rows.Select(r =>
            new RequestSubDto(
                (long)  r.RQ_SRL_NUM,
                (string)r.RQ_TRN_NED,
                (char)  ((string)r.RQ_STS_COD)[0],
                (long)  r.RQ_CRS_ID,
                string.Empty,
                (DateTime)r.RQ_STR_DAT,
                (DateTime)r.RQ_END_DAT,
                string.Empty, string.Empty, null, null));

        return new RequestMainDto(
            (long)    first.RQ_REQ_ID,
            (string)  first.RQ_EMP_USR,
            (DateTime)first.RQ_REQ_DAT,
            (string)  first.RQ_SUP_USR,
            subs);
    }

    public async Task<IEnumerable<PendingRequestDto>> GetPendingRequestsAsync(
        string supervisorUser, CancellationToken ct = default)
    {
        using var conn = CreateConnection();
        await conn.OpenAsync(ct);

        return await conn.QueryAsync<PendingRequestDto>(
            "EXEC dbo.usp_Request_GetPendingRequests @p_SupervisorUser",
            new { p_SupervisorUser = supervisorUser });
    }

    public async Task<long> CreateTrainingRequestAsync(
        long requestId, string empUser, DateTime reqDate,
        string supervisorUser, CancellationToken ct = default)
    {
        using var conn = CreateConnection();
        await conn.OpenAsync(ct);

        var parameters = new DynamicParameters();
        parameters.Add("p_ReqID",            requestId);
        parameters.Add("p_EmpUser",          empUser);
        parameters.Add("p_ReqDate",          reqDate);
        parameters.Add("p_SupervisorUser",   supervisorUser);
        parameters.Add("p_ReqID_OUTPUT",     dbType: System.Data.DbType.Int64,
                        direction: System.Data.ParameterDirection.Output);

        await conn.ExecuteAsync("dbo.usp_Request_CreateTrainingRequest",
            parameters, commandType: System.Data.CommandType.StoredProcedure);

        return parameters.Get<long>("p_ReqID_OUTPUT");
    }
}
