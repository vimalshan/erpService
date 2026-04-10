using TaskTransactional.Application.Queries;
using TaskTransactional.Infrastructure.Dapper;
using MediatR;

namespace TaskTransactional.Functions;

public class ComplaintMonitorWorker(
    ILogger<ComplaintMonitorWorker> logger,
    IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Complaint Monitor Worker started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var complaints = await mediator.Send(new GetAllComplaintMainsQuery(), stoppingToken);
                var tickets = await mediator.Send(new GetAllTicketsQuery(), stoppingToken);
                var actions = await mediator.Send(new GetAllActionsQuery(), stoppingToken);

                var openTickets = tickets.Where(t => t.CdClosureDate is null).Count();
                var closedTickets = tickets.Where(t => t.CdClosureDate is not null).Count();

                logger.LogInformation(
                    "Complaint Health: {Complaints} groups, {OpenTickets} open tickets, {ClosedTickets} closed tickets, {Actions} actions",
                    complaints.Count(), openTickets, closedTickets, actions.Count());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error monitoring complaint data");
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}

public class EscalationCheckWorker(
    ILogger<EscalationCheckWorker> logger,
    IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Escalation Check Worker started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var dapper = scope.ServiceProvider.GetRequiredService<ComplaintDapperQueries>();

                var complaints = await dapper.GetAllComplaintMainsAsync();
                logger.LogInformation("Escalation check: {Count} complaint groups active", complaints.Count());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in escalation check worker");
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}
