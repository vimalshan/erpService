using EmployeeTransactionsService.Application.Features.AlertGroups.Commands;
using EmployeeTransactionsService.Application.Features.AlertGroups.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTransactionsService.API.Controllers;

[ApiController]
[Route("api/alert-groups")]
[Authorize(Policy = "Reader")]
public sealed class AlertGroupsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        => Ok(await mediator.Send(new ListAlertGroupsQuery(), cancellationToken));

    [HttpGet("{alertGroupId:decimal}")]
    public async Task<IActionResult> GetById(decimal alertGroupId, CancellationToken cancellationToken)
    {
        var alertGroup = await mediator.Send(new GetAlertGroupByIdQuery(alertGroupId), cancellationToken);
        return alertGroup is null ? NotFound() : Ok(alertGroup);
    }

    [HttpPost]
    [Authorize(Policy = "Writer")]
    public async Task<IActionResult> Create([FromBody] CreateAlertGroupCommand command, CancellationToken cancellationToken)
    {
        var alertGroupId = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { alertGroupId }, new { alertGroupId });
    }
}