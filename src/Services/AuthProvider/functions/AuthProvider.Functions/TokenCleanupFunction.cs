using AuthProvider.Infrastructure.Data;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuthProvider.Functions;

/// <summary>
/// Azure Timer Function – runs every hour to clean up expired refresh tokens.
/// Demonstrates: Azure Functions timer trigger, EF Core in Azure Functions, Scaling.
/// </summary>
public sealed class TokenCleanupFunction
{
    private readonly ILogger<TokenCleanupFunction> _logger;
    private readonly AuthDbContext _dbContext;

    public TokenCleanupFunction(ILogger<TokenCleanupFunction> logger, AuthDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    // Runs every hour  – "0 0 * * * *" = every hour at :00
    [Function("TokenCleanupFunction")]
    public async Task Run([TimerTrigger("0 0 * * * *")] TimerInfo timer, FunctionContext context)
    {
        _logger.LogInformation("TokenCleanup triggered at {UtcNow}", DateTime.UtcNow);

        var cutoff = DateTime.UtcNow;

        var expiredTokens = await _dbContext.RefreshTokens
            .Where(rt => rt.ExpiresAt < cutoff || rt.RevokedAt != null)
            .ToListAsync();

        if (expiredTokens.Count == 0)
        {
            _logger.LogInformation("No expired tokens to clean up.");
            return;
        }

        _dbContext.RefreshTokens.RemoveRange(expiredTokens);
        var deleted = await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Token cleanup complete. Deleted {Count} expired/revoked tokens.", deleted);
    }
}
