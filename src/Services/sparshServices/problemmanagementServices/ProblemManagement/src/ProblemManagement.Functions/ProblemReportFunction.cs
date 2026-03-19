using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ProblemManagement.Domain.Interfaces;

namespace ProblemManagement.Functions;

public class ProblemReportFunction(
    IProblemRepository problemRepository,
    ILogger<ProblemReportFunction> logger)
{
    [Function("ProblemReportFunction")]
    public async Task Run([TimerTrigger("0 0 8 * * 1")] TimerInfo timerInfo, CancellationToken ct)
    {
        logger.LogInformation("Weekly problem report function triggered at: {Time}", DateTime.UtcNow);

        var allProblems = await problemRepository.GetAllAsync(ct);
        var posted = allProblems.Count(p => p.PrStatus == 'P');
        var accepted = allProblems.Count(p => p.PrStatus == 'A');
        var rejected = allProblems.Count(p => p.PrStatus == 'R');

        logger.LogInformation(
            "Weekly Report - Total: {Total}, Posted: {Posted}, Accepted: {Accepted}, Rejected: {Rejected}",
            allProblems.Count, posted, accepted, rejected);

        // In production: send email report or push to dashboard
    }
}
