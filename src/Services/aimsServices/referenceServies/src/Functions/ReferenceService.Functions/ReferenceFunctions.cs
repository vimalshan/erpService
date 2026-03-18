using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ReferenceService.Infrastructure.Persistence;

namespace ReferenceService.Functions;

/// <summary>
/// Azure Function for background data cleanup tasks.
/// Runs on a schedule (daily at 2 AM UTC).
/// </summary>
public class DataCleanupFunction
{
    private readonly ReferenceDbContext _dbContext;
    private readonly ILogger<DataCleanupFunction> _logger;
    
    public DataCleanupFunction(ReferenceDbContext dbContext, ILogger<DataCleanupFunction> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    [Function("DataCleanup")]
    public async Task Run([TimerTrigger("0 0 2 * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation("Data cleanup function started at {time}", DateTime.UtcNow);
        
        try
        {
            // Example: Archive old audit logs, clean up temporary records, etc.
            // This is a placeholder for actual cleanup logic
            
            _logger.LogInformation("Data cleanup completed successfully at {time}", DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during data cleanup at {time}", DateTime.UtcNow);
            throw;
        }
    }
}

/// <summary>
/// Azure Function for HTTP-triggered operations.
/// Endpoint: POST /api/reference-data/sync
/// </summary>
public class SyncDataFunction
{
    private readonly ReferenceDbContext _dbContext;
    private readonly ILogger<SyncDataFunction> _logger;
    
    public SyncDataFunction(ReferenceDbContext dbContext, ILogger<SyncDataFunction> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    [Function("SyncData")]
    public async Task<Microsoft.Azure.Functions.Worker.Http.HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "reference-data/sync")] 
        Microsoft.Azure.Functions.Worker.Http.HttpRequestData req)
    {
        _logger.LogInformation("Sync data function triggered");
        
        try
        {
            // Implement data synchronization logic
            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");
            await response.WriteAsJsonAsync(new { message = "Sync completed successfully" });
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during data sync");
            var response = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
            response.Headers.Add("Content-Type", "application/json");
            await response.WriteAsJsonAsync(new { error = ex.Message });
            return response;
        }
    }
}
