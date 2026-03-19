using Microsoft.Extensions.DependencyInjection;

namespace UserService.Infrastructure.Services;

/// <summary>
/// Health Check service
/// </summary>
public class HealthCheckService
{
    private readonly IServiceProvider _serviceProvider;

    public HealthCheckService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<HealthCheckResult> CheckDatabaseHealthAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var context = _serviceProvider.GetRequiredService<Data.UserServiceDbContext>();
            var canConnect = await context.Database.CanConnectAsync(cancellationToken);

            if (canConnect)
            {
                return new HealthCheckResult { Status = "Healthy", Message = "Database connection successful" };
            }

            return new HealthCheckResult { Status = "Unhealthy", Message = "Database connection failed" };
        }
        catch (Exception ex)
        {
            return new HealthCheckResult { Status = "Unhealthy", Message = $"Database health check failed: {ex.Message}" };
        }
    }

    public Task<HealthCheckResult> CheckApiHealthAsync()
    {
        return Task.FromResult(new HealthCheckResult { Status = "Healthy", Message = "API is running" });
    }
}

/// <summary>
/// Health check result
/// </summary>
public class HealthCheckResult
{
    public string Status { get; set; } = "Unknown";
    public string Message { get; set; } = string.Empty;
}
