using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AuthProvider.Infrastructure.Dapper;

/// <summary>
/// Dapper-based read repository for complex /reporting queries.
/// Uses raw SQL / stored procedures where EF Core would be overkill.
/// </summary>
public sealed class DapperUserRepository
{
    private readonly string _connectionString;
    private readonly ILogger<DapperUserRepository> _logger;

    public DapperUserRepository(IConfiguration config, ILogger<DapperUserRepository> logger)
    {
        _connectionString = config.GetConnectionString("AuthProviderDB")
            ?? throw new InvalidOperationException("Connection string 'AuthProviderDB' not found.");
        _logger = logger;
    }

    /// <summary>Execute usp_GetUserSummary stored procedure via Dapper.</summary>
    public async Task<IEnumerable<UserSummaryDto>> GetUserSummaryAsync(int page, int pageSize)
    {
        _logger.LogDebug("Dapper: GetUserSummary page={Page} pageSize={PageSize}", page, pageSize);
        await using var conn = new SqlConnection(_connectionString);
        return await conn.QueryAsync<UserSummaryDto>(
            "usp_GetUserSummary",
            new { Page = page, PageSize = pageSize },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    /// <summary>Execute usp_GetUserWithRoles via Dapper.</summary>
    public async Task<UserRoleDetailDto?> GetUserWithRolesAsync(Guid userId)
    {
        await using var conn = new SqlConnection(_connectionString);

        UserRoleDetailDto? result = null;
        await conn.QueryAsync<UserRoleDetailDto, string, UserRoleDetailDto>(
            "usp_GetUserWithRoles",
            (user, roleName) =>
            {
                result ??= user;
                if (!string.IsNullOrWhiteSpace(roleName))
                    result.Roles.Add(roleName);
                return result;
            },
            new { UserId = userId },
            commandType: System.Data.CommandType.StoredProcedure,
            splitOn: "RoleName");

        return result;
    }

    /// <summary>Execute usp_GetUserAuditLog via Dapper.</summary>
    public async Task<IEnumerable<AuditLogDto>> GetAuditLogsAsync(Guid userId, int topN = 50)
    {
        await using var conn = new SqlConnection(_connectionString);
        return await conn.QueryAsync<AuditLogDto>(
            "usp_GetUserAuditLog",
            new { UserId = userId, TopN = topN },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    /// <summary>Inline Dapper query – active users count by role.</summary>
    public async Task<IEnumerable<RoleCountDto>> GetRoleCountsAsync()
    {
        await using var conn = new SqlConnection(_connectionString);
        const string sql = @"
            SELECT r.Name AS RoleName, COUNT(ur.UserId) AS UserCount
            FROM Roles r
            LEFT JOIN UserRoles ur ON ur.RoleId = r.Id
            INNER JOIN Users u ON u.Id = ur.UserId AND u.IsActive = 1
            GROUP BY r.Name
            ORDER BY UserCount DESC";
        return await conn.QueryAsync<RoleCountDto>(sql);
    }
}

// ─── Dapper Result DTOs ───────────────────────────────────────────────────────

public record UserSummaryDto(Guid Id, string Username, string Email, bool IsActive, DateTime CreatedAt);

public class UserRoleDetailDto
{
    public Guid Id { get; init; }
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public List<string> Roles { get; } = new();
}

public record AuditLogDto(Guid Id, string Action, string Resource, DateTime Timestamp, bool IsSuccess);
public record RoleCountDto(string RoleName, int UserCount);
