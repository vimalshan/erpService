using MediatR;
using TransactionService.Application.Features.StoredProcedures.Commands;

namespace TransactionService.Functions;

public class TransactionWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TransactionWorker> _logger;

    public TransactionWorker(IServiceProvider serviceProvider, ILogger<TransactionWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("TransactionWorker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;
            var nextRun = new DateTime(now.Year, now.Month, 1, 0, 5, 0, DateTimeKind.Utc).AddMonths(1);
            var delay = nextRun - now;

            if (delay < TimeSpan.Zero) delay = TimeSpan.FromSeconds(30);

            _logger.LogInformation("Next transaction processing scheduled at {NextRun}.", nextRun);

            try
            {
                await Task.Delay(delay, stoppingToken);

                if (stoppingToken.IsCancellationRequested) break;

                var month = DateTime.UtcNow.Month;
                var year = DateTime.UtcNow.Year;

                _logger.LogInformation("Running monthly stipend processing for {Month}/{Year}.", month, year);

                using var scope = _serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var result = await mediator.Send(
                    new ProcessMonthlyStipendSpCommand(month, year, 1),
                    stoppingToken);

                _logger.LogInformation("Monthly stipend processing complete: {Message}", result.Message);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during monthly transaction processing.");
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
