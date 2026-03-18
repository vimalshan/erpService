using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProxyModule.Application.DTOs;
using ProxyModule.Application.Interfaces;

namespace ProxyModule.Infrastructure.Repositories;

public class ProxyRightReadRepository : IProxyRightReadRepository
{
    private readonly string _connectionString;

    public ProxyRightReadRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<ProxyRightDto?> GetByIdAsync(long proxyId, CancellationToken ct = default)
    {
        const string sql = """
            SELECT PROXY_ID AS ProxyId, PROXY_USER_ID AS ProxyUserId, DELEGATED_USER_ID AS DelegatedUserId,
                   PROXY_START_DATE AS ProxyStartDate, PROXY_END_DATE AS ProxyEndDate,
                   PROXY_TYPE AS ProxyType, PROXY_STATUS AS ProxyStatus, SCOPE, NOTES,
                   CREATED_BY AS CreatedBy, CREATED_ON AS CreatedOn,
                   UPDATED_BY AS UpdatedBy, UPDATED_ON AS UpdatedOn,
                   CASE WHEN PROXY_STATUS = 'A' AND PROXY_START_DATE <= GETDATE()
                        AND (PROXY_END_DATE IS NULL OR PROXY_END_DATE >= GETDATE())
                        THEN 1 ELSE 0 END AS IsCurrentlyActive
            FROM PROXY_RIGHTS WHERE PROXY_ID = @ProxyId
            """;

        await using var connection = new SqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<ProxyRightDto>(
            new CommandDefinition(sql, new { ProxyId = proxyId }, cancellationToken: ct));
    }

    public async Task<IEnumerable<ProxyRightDto>> GetByProxyUserIdAsync(long proxyUserId, CancellationToken ct = default)
    {
        const string sql = """
            SELECT PROXY_ID AS ProxyId, PROXY_USER_ID AS ProxyUserId, DELEGATED_USER_ID AS DelegatedUserId,
                   PROXY_START_DATE AS ProxyStartDate, PROXY_END_DATE AS ProxyEndDate,
                   PROXY_TYPE AS ProxyType, PROXY_STATUS AS ProxyStatus, SCOPE, NOTES,
                   CREATED_BY AS CreatedBy, CREATED_ON AS CreatedOn,
                   UPDATED_BY AS UpdatedBy, UPDATED_ON AS UpdatedOn,
                   CASE WHEN PROXY_STATUS = 'A' AND PROXY_START_DATE <= GETDATE()
                        AND (PROXY_END_DATE IS NULL OR PROXY_END_DATE >= GETDATE())
                        THEN 1 ELSE 0 END AS IsCurrentlyActive
            FROM PROXY_RIGHTS WHERE PROXY_USER_ID = @ProxyUserId
            ORDER BY CREATED_ON DESC
            """;

        await using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<ProxyRightDto>(
            new CommandDefinition(sql, new { ProxyUserId = proxyUserId }, cancellationToken: ct));
    }

    public async Task<IEnumerable<ProxyRightDto>> GetByDelegatedUserIdAsync(long delegatedUserId, CancellationToken ct = default)
    {
        const string sql = """
            SELECT PROXY_ID AS ProxyId, PROXY_USER_ID AS ProxyUserId, DELEGATED_USER_ID AS DelegatedUserId,
                   PROXY_START_DATE AS ProxyStartDate, PROXY_END_DATE AS ProxyEndDate,
                   PROXY_TYPE AS ProxyType, PROXY_STATUS AS ProxyStatus, SCOPE, NOTES,
                   CREATED_BY AS CreatedBy, CREATED_ON AS CreatedOn,
                   UPDATED_BY AS UpdatedBy, UPDATED_ON AS UpdatedOn,
                   CASE WHEN PROXY_STATUS = 'A' AND PROXY_START_DATE <= GETDATE()
                        AND (PROXY_END_DATE IS NULL OR PROXY_END_DATE >= GETDATE())
                        THEN 1 ELSE 0 END AS IsCurrentlyActive
            FROM PROXY_RIGHTS WHERE DELEGATED_USER_ID = @DelegatedUserId
            ORDER BY CREATED_ON DESC
            """;

        await using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<ProxyRightDto>(
            new CommandDefinition(sql, new { DelegatedUserId = delegatedUserId }, cancellationToken: ct));
    }

    public async Task<IEnumerable<ProxyRightDto>> GetActiveProxyRightsAsync(CancellationToken ct = default)
    {
        const string sql = """
            SELECT PROXY_ID AS ProxyId, PROXY_USER_ID AS ProxyUserId, DELEGATED_USER_ID AS DelegatedUserId,
                   PROXY_START_DATE AS ProxyStartDate, PROXY_END_DATE AS ProxyEndDate,
                   PROXY_TYPE AS ProxyType, PROXY_STATUS AS ProxyStatus, SCOPE, NOTES,
                   CREATED_BY AS CreatedBy, CREATED_ON AS CreatedOn,
                   UPDATED_BY AS UpdatedBy, UPDATED_ON AS UpdatedOn,
                   1 AS IsCurrentlyActive
            FROM PROXY_RIGHTS
            WHERE PROXY_STATUS = 'A'
              AND PROXY_START_DATE <= GETDATE()
              AND (PROXY_END_DATE IS NULL OR PROXY_END_DATE >= GETDATE())
            ORDER BY CREATED_ON DESC
            """;

        await using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<ProxyRightDto>(
            new CommandDefinition(sql, cancellationToken: ct));
    }
}
