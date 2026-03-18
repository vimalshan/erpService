using LoanDefinition.Infrastructure.Persistence;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LoanDefinition.Functions;

public class LoanDefinitionFunctions(ILoggerFactory loggerFactory, LoanDefinitionDbContext dbContext)
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<LoanDefinitionFunctions>();

    [Function("ProcessExpiredLoanRates")]
    public async Task ProcessExpiredLoanRates([TimerTrigger("0 0 1 * * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("Processing expired loan rates at {Time}", DateTime.UtcNow);

        var expiredRates = await dbContext.LoanInterestRates
            .Where(r => r.ClosureDate.HasValue && r.ClosureDate < DateTime.UtcNow)
            .ToListAsync();

        _logger.LogInformation("Found {Count} expired interest rates", expiredRates.Count);
    }

    [Function("RecalculateLoanLimits")]
    public async Task RecalculateLoanLimits([TimerTrigger("0 0 2 * * 1")] TimerInfo timerInfo)
    {
        _logger.LogInformation("Recalculating loan limits at {Time}", DateTime.UtcNow);

        var activeLoans = await dbContext.LoanMasters
            .Where(l => l.ClosureDate == null || l.ClosureDate > DateTime.UtcNow)
            .ToListAsync();

        _logger.LogInformation("Processing limits for {Count} active loans", activeLoans.Count);
    }

    [Function("CleanupExpiredFestivals")]
    public async Task CleanupExpiredFestivals([TimerTrigger("0 0 0 1 * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("Cleaning up expired festivals at {Time}", DateTime.UtcNow);

        var expiredFestivals = await dbContext.LoanFestivals
            .Where(f => f.EndDate < DateTime.UtcNow.AddMonths(-6))
            .ToListAsync();

        _logger.LogInformation("Found {Count} expired festivals for cleanup", expiredFestivals.Count);
    }
}
