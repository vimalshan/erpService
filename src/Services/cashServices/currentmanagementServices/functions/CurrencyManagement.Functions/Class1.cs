using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using CurrencyManagement.Application.Common.Interfaces;
using CurrencyManagement.Application.ExchangeRates.Commands.SetExchangeRate;
using MediatR;

namespace CurrencyManagement.Functions;

/// <summary>
/// Azure Function to update exchange rates on a schedule
/// </summary>
public class ExchangeRateUpdateFunction
{
    private readonly ILogger<ExchangeRateUpdateFunction> _logger;
    private readonly IMediator _mediator;
    private readonly IExchangeRateQueryService _queryService;

    public ExchangeRateUpdateFunction(ILogger<ExchangeRateUpdateFunction> logger, IMediator mediator, IExchangeRateQueryService queryService)
    {
        _logger = logger;
        _mediator = mediator;
        _queryService = queryService;
    }

    [Function("ExchangeRateUpdateFunction")]
    public async Task Run([TimerTrigger("0 0 */6 * * *")] TimerInfo myTimer) // Every 6 hours
    {
        try
        {
            _logger.LogInformation($"Exchange Rate Update Function started at {DateTime.UtcNow}");

            // Example: Update EUR to USD rate
            // In production, fetch from external rate provider (e.g., ECB, Reuters)
            var command = new SetExchangeRateCommand(
                RateId: 100,
                FinancialYear: DateTime.Now.Year,
                Month: DateTime.Now.Month,
                FromCurrencyId: 2,  // EUR
                ToCurrencyId: 1,    // USD
                Rate: 1.18m,        // Example rate
                ModifiedBy: 1
            );

            await _mediator.Send(command);

            _logger.LogInformation($"Exchange Rate Update Function completed at {DateTime.UtcNow}");

            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule: {myTimer.ScheduleStatus.Next}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Exchange Rate Update Function");
            throw;
        }
    }
}

/// <summary>
/// Azure Function to clean up old exchange rate data
/// </summary>
public class CurrencyCleanupFunction
{
    private readonly ILogger<CurrencyCleanupFunction> _logger;

    public CurrencyCleanupFunction(ILogger<CurrencyCleanupFunction> logger)
    {
        _logger = logger;
    }

    [Function("CurrencyCleanupFunction")]
    public async Task Run([TimerTrigger("0 0 0 1 * *")] TimerInfo myTimer) // Monthly at midnight on the 1st
    {
        try
        {
            _logger.LogInformation($"Currency Cleanup Function started at {DateTime.UtcNow}");

            // Archive old exchange rate records (older than 3 years)
            var thresholdDate = DateTime.UtcNow.AddYears(-3);

            _logger.LogInformation($"Archiving records older than {thresholdDate}");

            // TODO: Implement archival logic based on project requirements

            _logger.LogInformation($"Currency Cleanup Function completed at {DateTime.UtcNow}");

            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule: {myTimer.ScheduleStatus.Next}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Currency Cleanup Function");
            throw;
        }
    }
}
