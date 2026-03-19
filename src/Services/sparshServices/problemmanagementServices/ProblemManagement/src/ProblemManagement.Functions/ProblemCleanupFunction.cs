using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ProblemManagement.Domain.Interfaces;

namespace ProblemManagement.Functions;

public class ProblemCleanupFunction(
    IProblemRepository problemRepository,
    ILogger<ProblemCleanupFunction> logger)
{
    [Function("ProblemCleanupFunction")]
    public async Task Run([TimerTrigger("0 0 2 * * *")] TimerInfo timerInfo, CancellationToken ct)
    {
        logger.LogInformation("Problem cleanup function triggered at: {Time}", DateTime.UtcNow);

        var problems = await problemRepository.GetByStatusAsync('R', ct);
        var oldRejectedProblems = problems
            .Where(p => p.PrModOn < DateTime.UtcNow.AddDays(-90))
            .ToList();

        logger.LogInformation("Found {Count} rejected problems older than 90 days", oldRejectedProblems.Count);

        foreach (var problem in oldRejectedProblems)
        {
            logger.LogInformation("Archiving problem {ProblemId}", problem.PrId);
            // Archive logic can be implemented here
        }
    }
}
