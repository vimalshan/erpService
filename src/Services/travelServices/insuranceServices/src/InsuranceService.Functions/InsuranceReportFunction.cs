using InsuranceService.Application.Queries;
using InsuranceService.Infrastructure.Messaging;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace InsuranceService.Functions;

public class InsuranceReportFunction
{
    private readonly IMediator _mediator;
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<InsuranceReportFunction> _logger;

    public InsuranceReportFunction(
        IMediator mediator,
        IMessagePublisher publisher,
        ILogger<InsuranceReportFunction> logger)
    {
        _mediator = mediator;
        _publisher = publisher;
        _logger = logger;
    }

    /// <summary>
    /// Timer-triggered function that runs weekly to generate insurance summary reports
    /// </summary>
    [Function("InsuranceWeeklyReport")]
    public async Task RunWeeklyReport(
        [TimerTrigger("0 0 6 * * 1")] TimerInfo timerInfo,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Weekly insurance report generation started at: {Time}", DateTime.UtcNow);

        var insurances = await _mediator.Send(new GetInsuranceDetailsQuery(null, null), cancellationToken);

        var report = new
        {
            GeneratedAt = DateTime.UtcNow,
            TotalPolicies = insurances.Count,
            ActivePolicies = insurances.Count(i => i.Status == "A"),
            InactivePolicies = insurances.Count(i => i.Status == "I"),
            ExpiredPolicies = insurances.Count(i => i.Status == "E")
        };

        await _publisher.PublishAsync("insurance.events", "insurance.report.generated", report, cancellationToken);

        _logger.LogInformation("Weekly insurance report generated: {Report}", report);
    }
}
