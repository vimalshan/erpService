using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApprovalGroup.Application.ApprovalGroups.Queries;

namespace ApprovalGroup.Functions;

/// <summary>
/// Timer-triggered function: Runs daily to report approval group statistics
/// </summary>
public class ApprovalGroupReportFunction
{
    private readonly ILogger<ApprovalGroupReportFunction> _logger;
    private readonly IMediator _mediator;

    public ApprovalGroupReportFunction(ILogger<ApprovalGroupReportFunction> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [Function("ApprovalGroupDailyReport")]
    public async Task RunAsync([TimerTrigger("0 0 8 * * *")] TimerInfo timerInfo, CancellationToken ct)
    {
        _logger.LogInformation("ApprovalGroupDailyReport triggered at: {Time}", DateTime.UtcNow);

        var groups = await _mediator.Send(new GetAllApprovalGroupsQuery(), ct);
        _logger.LogInformation("Total approval groups: {Count}", groups.Count());

        foreach (var group in groups)
            _logger.LogInformation("Group: {GroupId} - {GroupName} (Priority: {Priority})",
                group.GroupId, group.GroupName, group.GroupPriorityId);
    }
}

/// <summary>
/// HTTP-triggered function: On-demand approval group sync
/// </summary>
public class ApprovalGroupSyncFunction
{
    private readonly ILogger<ApprovalGroupSyncFunction> _logger;
    private readonly IMediator _mediator;

    public ApprovalGroupSyncFunction(ILogger<ApprovalGroupSyncFunction> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [Function("ApprovalGroupSync")]
    public async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "approval-groups/sync")]
        HttpRequest req, CancellationToken ct)
    {
        _logger.LogInformation("ApprovalGroupSync triggered");
        var groups = await _mediator.Send(new GetAllApprovalGroupsQuery(), ct);
        return new OkObjectResult(new { synced = groups.Count(), timestamp = DateTime.UtcNow });
    }
}
