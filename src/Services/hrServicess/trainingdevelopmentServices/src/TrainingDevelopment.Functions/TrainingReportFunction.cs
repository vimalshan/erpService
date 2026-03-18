using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TrainingDevelopment.Infrastructure.Dapper;

namespace TrainingDevelopment.Functions;

public class TrainingReportFunction
{
    private readonly ILogger<TrainingReportFunction> _logger;
    private readonly TrainingDetailDapperRepository _dapperRepo;

    public TrainingReportFunction(ILogger<TrainingReportFunction> logger, TrainingDetailDapperRepository dapperRepo)
    {
        _logger = logger;
        _dapperRepo = dapperRepo;
    }

    /// <summary>
    /// Runs daily at midnight — generates training summary report.
    /// Schedule: "0 0 0 * * *" = every day at midnight (UTC).
    /// </summary>
    [Function(nameof(TrainingReportFunction))]
    public async Task Run(
        [TimerTrigger("0 0 0 * * *")] TimerInfo timerInfo,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Training Report Function triggered at {Time}", DateTime.UtcNow);

        try
        {
            var summary = await _dapperRepo.GetTrainingSummaryByStatusAsync(cancellationToken);
            foreach (var item in summary)
                _logger.LogInformation("Status: {Status} | Count: {Count} | TotalCost: {TotalCost}",
                    (string)item.Status, (int)item.Count, (decimal)item.TotalCost);

            _logger.LogInformation("Training Report completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Training Report Function failed.");
            throw;
        }
    }
}
