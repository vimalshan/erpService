using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace PromotionService.Functions;

/// <summary>
/// Timer-triggered function that runs nightly to process expired promotion periods
/// and notify employees whose promotions are pending.
/// </summary>
public class PromotionProcessingFunction
{
    private readonly PromotionFunctionsDbContext _db;
    private readonly ILogger<PromotionProcessingFunction> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public PromotionProcessingFunction(
        PromotionFunctionsDbContext db,
        ILogger<PromotionProcessingFunction> logger,
        IHttpClientFactory httpClientFactory)
    {
        _db = db;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// Runs every day at midnight UTC. Marks promotion periods that have passed their end date as inactive.
    /// CRON: "0 0 * * * *" = every hour | "0 0 0 * * *" = daily at midnight
    /// </summary>
    [Function("CloseExpiredPromotionPeriods")]
    public async Task RunCloseExpiredPeriods(
        [TimerTrigger("0 0 0 * * *")] TimerInfo timer,
        FunctionContext context)
    {
        _logger.LogInformation("CloseExpiredPromotionPeriods triggered at {Now}", DateTime.UtcNow);

        var expiredPeriods = await _db.PromotionPeriods
            .Where(p => p.IsActive == 1 && p.EndDate < DateTime.UtcNow)
            .ToListAsync();

        foreach (var period in expiredPeriods)
        {
            period.IsActive = 0;
            _logger.LogInformation("Closing promotion period {PeriodId}: {PeriodName}", period.PeriodId, period.PeriodName);
        }

        if (expiredPeriods.Count > 0)
            await _db.SaveChangesAsync();

        _logger.LogInformation("Closed {Count} expired promotion periods.", expiredPeriods.Count);
    }

    /// <summary>
    /// Runs weekly on Monday at 8 AM UTC. Summarizes pending promotions and calls the API to send notifications.
    /// </summary>
    [Function("WeeklyPromotionSummary")]
    public async Task RunWeeklySummary(
        [TimerTrigger("0 0 8 * * 1")] TimerInfo timer,
        FunctionContext context)
    {
        _logger.LogInformation("WeeklyPromotionSummary triggered at {Now}", DateTime.UtcNow);

        var pendingCount = await _db.PromotionRecommendations
            .CountAsync(r => r.Status == "Pending");

        var pendingIncrements = await _db.IncrementRequests
            .CountAsync(r => r.Status == "Pending");

        _logger.LogInformation(
            "Weekly summary: {PendingPromotions} pending promotions, {PendingIncrements} pending increments.",
            pendingCount, pendingIncrements);

        // Optionally call the Promotion Service API to trigger notification emails
        // var client = _httpClientFactory.CreateClient("PromotionApi");
        // await client.PostAsync("/api/v1/promotions/notify-pending", null);
    }
}

/// <summary>
/// HTTP-triggered function for on-demand operations (e.g., from an admin portal).
/// </summary>
public class PromotionAdminFunction
{
    private readonly PromotionFunctionsDbContext _db;
    private readonly ILogger<PromotionAdminFunction> _logger;

    public PromotionAdminFunction(PromotionFunctionsDbContext db, ILogger<PromotionAdminFunction> logger)
    {
        _db = db;
        _logger = logger;
    }

    /// <summary>
    /// Returns a summary of current promotion period status.
    /// GET /api/promotion/status
    /// </summary>
    [Function("GetPromotionStatus")]
    public async Task<HttpResponseData> GetStatus(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "promotion/status")] HttpRequestData req)
    {
        _logger.LogInformation("GetPromotionStatus triggered.");

        var activePeriods = await _db.PromotionPeriods
            .Where(p => p.IsActive == 1)
            .Select(p => new { p.PeriodId, p.PeriodName, p.StartDate, p.EndDate })
            .ToListAsync();

        var pendingRecs = await _db.PromotionRecommendations
            .CountAsync(r => r.Status == "Pending");

        var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
        await response.WriteAsJsonAsync(new
        {
            ActivePeriods = activePeriods,
            PendingRecommendations = pendingRecs,
            GeneratedAt = DateTime.UtcNow
        });

        return response;
    }

    /// <summary>
    /// Reactivates a promotion period by ID.
    /// POST /api/promotion/period/{periodId}/reactivate
    /// </summary>
    [Function("ReactivatePromotionPeriod")]
    public async Task<HttpResponseData> ReactivatePeriod(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "promotion/period/{periodId}/reactivate")] HttpRequestData req,
        decimal periodId)
    {
        _logger.LogInformation("ReactivatePromotionPeriod for PeriodId={PeriodId}", periodId);

        var period = await _db.PromotionPeriods.FindAsync(periodId);
        if (period == null)
        {
            var notFound = req.CreateResponse(System.Net.HttpStatusCode.NotFound);
            await notFound.WriteStringAsync($"Period {periodId} not found.");
            return notFound;
        }

        period.IsActive = 1;
        await _db.SaveChangesAsync();

        var ok = req.CreateResponse(System.Net.HttpStatusCode.OK);
        await ok.WriteAsJsonAsync(new { message = $"Period {periodId} reactivated.", periodId });
        return ok;
    }
}
