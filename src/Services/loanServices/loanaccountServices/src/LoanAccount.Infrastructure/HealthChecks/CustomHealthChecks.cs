using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using LoanAccount.Domain.Interfaces;

namespace LoanAccount.Infrastructure.HealthChecks;

/// <summary>
/// Custom health check for RabbitMQ connectivity
/// </summary>
public class RabbitMQHealthCheck : IHealthCheck
{
    private readonly IConnection _connection;
    private readonly ILogger<RabbitMQHealthCheck> _logger;

    public RabbitMQHealthCheck(IConnection connection, ILogger<RabbitMQHealthCheck> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (_connection.IsOpen)
            {
                _logger.LogInformation("RabbitMQ health check passed");
                return HealthCheckResult.Healthy("RabbitMQ connection is healthy");
            }
            else
            {
                _logger.LogWarning("RabbitMQ connection is not open");
                return HealthCheckResult.Unhealthy("RabbitMQ connection is not open");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RabbitMQ health check failed");
            return HealthCheckResult.Unhealthy($"RabbitMQ health check failed: {ex.Message}");
        }
    }
}

/// <summary>
/// Custom health check for Loan database
/// </summary>
public class LoanDatabaseHealthCheck : IHealthCheck
{
    private readonly ILoanMainRepository _loanRepository;
    private readonly ILogger<LoanDatabaseHealthCheck> _logger;

    public LoanDatabaseHealthCheck(ILoanMainRepository loanRepository, ILogger<LoanDatabaseHealthCheck> logger)
    {
        _loanRepository = loanRepository;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Try to fetch a loan to check database connectivity
            var loans = await _loanRepository.GetActiveLoansAsync(cancellationToken);
            _logger.LogInformation("Loan database health check passed");
            return HealthCheckResult.Healthy($"Loan database is healthy. Found {loans.Count()} active loans");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Loan database health check failed");
            return HealthCheckResult.Unhealthy($"Loan database health check failed: {ex.Message}");
        }
    }
}
