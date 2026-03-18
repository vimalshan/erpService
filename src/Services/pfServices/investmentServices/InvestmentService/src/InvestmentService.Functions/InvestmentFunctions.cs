using InvestmentService.Application.Commands;
using InvestmentService.Application.Queries;
using InvestmentService.Domain.Interfaces;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace InvestmentService.Functions;

public class InvestmentMaturityCheckFunction
{
    private readonly IMediator _mediator;
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<InvestmentMaturityCheckFunction> _logger;

    public InvestmentMaturityCheckFunction(IMediator mediator, IMessagePublisher publisher,
        ILogger<InvestmentMaturityCheckFunction> logger)
    {
        _mediator = mediator;
        _publisher = publisher;
        _logger = logger;
    }

    [Function("CheckMaturedInvestments")]
    public async Task CheckMaturedInvestments(
        [TimerTrigger("0 0 6 * * *")] TimerInfo timer) // Daily at 6 AM
    {
        _logger.LogInformation("Checking for matured investments at {Time}", DateTime.UtcNow);

        var matured = await _mediator.Send(new GetMaturedInvestmentsQuery(DateTime.UtcNow));

        foreach (var investment in matured)
        {
            _logger.LogInformation("Investment {InvNo} has matured", investment.InvNo);
            await _publisher.PublishAsync("investment-events", "event.investment.matured",
                new { investment.InvNo, investment.MaturityDate }, default);
        }

        _logger.LogInformation("Maturity check complete. Found {Count} matured investments", matured.Count);
    }
}

public class InterestScheduleGenerationFunction
{
    private readonly IMediator _mediator;
    private readonly ILogger<InterestScheduleGenerationFunction> _logger;

    public InterestScheduleGenerationFunction(IMediator mediator,
        ILogger<InterestScheduleGenerationFunction> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [Function("GenerateInterestSchedules")]
    public async Task GenerateInterestSchedules(
        [TimerTrigger("0 0 1 1 * *")] TimerInfo timer) // Monthly on 1st at 1 AM
    {
        _logger.LogInformation("Generating interest schedules at {Time}", DateTime.UtcNow);

        var activeInvestments = await _mediator.Send(new GetActiveInvestmentsQuery());
        var year = DateTime.UtcNow.Year;

        foreach (var investment in activeInvestments)
        {
            try
            {
                await _mediator.Send(new GenerateInterestScheduleCommand(investment.InvNo, year));
                _logger.LogInformation("Generated schedule for investment {InvNo}", investment.InvNo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate schedule for investment {InvNo}", investment.InvNo);
            }
        }
    }
}

public class PendingInterestReminderFunction
{
    private readonly IMediator _mediator;
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<PendingInterestReminderFunction> _logger;

    public PendingInterestReminderFunction(IMediator mediator, IMessagePublisher publisher,
        ILogger<PendingInterestReminderFunction> logger)
    {
        _mediator = mediator;
        _publisher = publisher;
        _logger = logger;
    }

    [Function("SendPendingInterestReminders")]
    public async Task SendPendingInterestReminders(
        [TimerTrigger("0 0 8 * * 1")] TimerInfo timer) // Every Monday at 8 AM
    {
        _logger.LogInformation("Checking for pending interest receipts at {Time}", DateTime.UtcNow);

        var pending = await _mediator.Send(new GetPendingSchedulesQuery(DateTime.UtcNow));

        foreach (var schedule in pending)
        {
            await _publisher.PublishAsync("investment-events", "event.interest.pending",
                new { schedule.InvNo, schedule.DueDate, schedule.DueAmount }, default);
        }

        _logger.LogInformation("Sent {Count} pending interest reminders", pending.Count);
    }
}

public class PortfolioReportFunction
{
    private readonly IMediator _mediator;
    private readonly IBlobStorageService _blobStorage;
    private readonly ILogger<PortfolioReportFunction> _logger;

    public PortfolioReportFunction(IMediator mediator, IBlobStorageService blobStorage,
        ILogger<PortfolioReportFunction> logger)
    {
        _mediator = mediator;
        _blobStorage = blobStorage;
        _logger = logger;
    }

    [Function("GeneratePortfolioReport")]
    public async Task GeneratePortfolioReport(
        [TimerTrigger("0 0 2 1 * *")] TimerInfo timer) // Monthly on 1st at 2 AM
    {
        _logger.LogInformation("Generating portfolio report at {Time}", DateTime.UtcNow);

        var summary = await _mediator.Send(new GetPortfolioSummaryQuery());

        var reportContent = System.Text.Json.JsonSerializer.Serialize(summary, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true
        });

        var reportStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(reportContent));
        var blobName = $"reports/portfolio-{DateTime.UtcNow:yyyy-MM-dd}.json";

        await _blobStorage.UploadAsync("investment-reports", blobName, reportStream, "application/json");

        _logger.LogInformation("Portfolio report generated and saved to blob: {BlobName}", blobName);
    }
}
