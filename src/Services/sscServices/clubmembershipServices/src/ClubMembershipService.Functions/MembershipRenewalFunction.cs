using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using Dapper;

namespace ClubMembershipService.Functions;

public class MembershipRenewalFunction
{
    private readonly ILogger<MembershipRenewalFunction> _logger;
    private readonly string _connectionString;

    public MembershipRenewalFunction(
        ILogger<MembershipRenewalFunction> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    // Runs daily at midnight
    [Function("MembershipRenewalCheck")]
    public async Task RunAsync([TimerTrigger("0 0 0 * * *")] TimerInfo timer)
    {
        _logger.LogInformation("Membership renewal check triggered at: {Time}", DateTime.UtcNow);

        using var conn = new SqlConnection(_connectionString);
        var expiredMemberships = await conn.QueryAsync<long>(
            @"SELECT MEMBERSHIP_ID FROM CLUB_MEMBERSHIP
              WHERE MEMBERSHIP_STATUS = 'A'
              AND JOIN_DATE < DATEADD(YEAR, -1, GETDATE())");

        foreach (var membershipId in expiredMemberships)
        {
            _logger.LogWarning("Membership {MembershipId} is due for renewal.", membershipId);
            // Publish renewal notification events or update status
        }

        _logger.LogInformation("Renewal check complete. {Count} memberships checked.", expiredMemberships.Count());
    }
}
