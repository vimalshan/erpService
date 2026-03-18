using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using UserManagement.Domain.Entities;

namespace UserManagement.Infrastructure.Dapper;

/// <summary>
/// Dapper-based read store for high-performance queries (reporting, bulk reads).
/// </summary>
public class UserManagementDapperContext(IConfiguration configuration)
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    private SqlConnection CreateConnection() => new(_connectionString);

    public async Task<IEnumerable<dynamic>> GetActiveUserPoliciesSummaryAsync(CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT 
                p.POLICY_ID        AS PolicyId,
                p.USER_SYSID       AS UserSysId,
                p.POLICY_CODE      AS PolicyCode,
                p.POLICY_TYPE      AS PolicyType,
                p.POLICY_STATUS    AS PolicyStatus,
                p.EFFECTIVE_FROM   AS EffectiveFrom,
                p.SESSION_TIMEOUT_MINS AS SessionTimeoutMins,
                COUNT(h.HIST_ID)   AS ChangeCount
            FROM USER_POLICY p
            LEFT JOIN USER_PROFILEHIST h ON p.POLICY_ID = h.POLICY_ID
            WHERE p.POLICY_STATUS = 'A'
            GROUP BY p.POLICY_ID, p.USER_SYSID, p.POLICY_CODE, p.POLICY_TYPE,
                     p.POLICY_STATUS, p.EFFECTIVE_FROM, p.SESSION_TIMEOUT_MINS
            ORDER BY p.POLICY_ID
            """;

        using var conn = CreateConnection();
        return await conn.QueryAsync(sql);
    }

    public async Task<IEnumerable<dynamic>> GetUserContactSummaryAsync(long userSysId)
    {
        const string sql = """
            SELECT 
                c.CONTACT_ID       AS ContactId,
                c.USER_SYSID       AS UserSysId,
                c.PRIMARY_EMAIL    AS PrimaryEmail,
                c.PHONE            AS Phone,
                c.NEWSLETTER_OPT_IN AS NewsletterOptIn,
                c.CONTACT_STATUS   AS ContactStatus,
                c.CREATED_ON       AS CreatedOn
            FROM WEBSITE_CON_MAILID c
            WHERE c.USER_SYSID = @UserSysId
            ORDER BY c.CONTACT_ID
            """;

        using var conn = CreateConnection();
        return await conn.QueryAsync(sql, new { UserSysId = userSysId });
    }

    public async Task<IEnumerable<dynamic>> GetAuditReportAsync(DateTime from, DateTime to)
    {
        const string sql = """
            SELECT 
                h.HIST_ID          AS HistId,
                h.USER_SYSID       AS UserSysId,
                h.PROFILE_FIELD    AS ProfileField,
                h.OLD_VALUE        AS OldValue,
                h.NEW_VALUE        AS NewValue,
                h.CHANGED_BY       AS ChangedBy,
                h.CHANGED_ON       AS ChangedOn
            FROM USER_PROFILEHIST h
            WHERE h.CHANGED_ON BETWEEN @From AND @To
            ORDER BY h.CHANGED_ON DESC
            """;

        using var conn = CreateConnection();
        return await conn.QueryAsync(sql, new { From = from, To = to });
    }
}
