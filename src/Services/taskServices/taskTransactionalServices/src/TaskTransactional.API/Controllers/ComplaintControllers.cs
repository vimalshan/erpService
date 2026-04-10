using TaskTransactional.Application.Commands;
using TaskTransactional.Application.DTOs;
using TaskTransactional.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskTransactional.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ComplaintsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ComplaintMainDto>>> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetAllComplaintMainsQuery(), ct));

    [HttpGet("{groupId}")]
    public async Task<ActionResult<ComplaintMainDto>> GetByGroupId(string groupId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetComplaintMainByGroupIdQuery(groupId), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("unit/{unitCode}")]
    public async Task<ActionResult<IEnumerable<ComplaintMainDto>>> GetByUnitCode(string unitCode, CancellationToken ct)
        => Ok(await mediator.Send(new GetComplaintMainsByUnitCodeQuery(unitCode), ct));

    [HttpPost]
    public async Task<ActionResult<string>> Create([FromBody] CreateComplaintMainCommand command, CancellationToken ct)
    {
        var groupId = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetByGroupId), new { groupId }, groupId);
    }

    [HttpPut("{groupId}")]
    public async Task<IActionResult> Update(string groupId, [FromBody] UpdateComplaintMainCommand command, CancellationToken ct)
    {
        if (groupId != command.GroupId) return BadRequest();
        return await mediator.Send(command, ct) ? NoContent() : NotFound();
    }

    [HttpDelete("{groupId}")]
    public async Task<IActionResult> Delete(string groupId, CancellationToken ct)
        => await mediator.Send(new DeleteComplaintMainCommand(groupId), ct) ? NoContent() : NotFound();
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TicketsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ComplaintDetailDto>>> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetAllTicketsQuery(), ct));

    [HttpGet("{ticketNum}")]
    public async Task<ActionResult<ComplaintDetailDto>> GetByNum(decimal ticketNum, CancellationToken ct)
    {
        var result = await mediator.Send(new GetTicketByNumQuery(ticketNum), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("group/{groupId}")]
    public async Task<ActionResult<IEnumerable<ComplaintDetailDto>>> GetByGroupId(decimal groupId, CancellationToken ct)
        => Ok(await mediator.Send(new GetTicketsByGroupIdQuery(groupId), ct));

    [HttpPost]
    public async Task<ActionResult<decimal>> Create([FromBody] CreateTicketCommand command, CancellationToken ct)
    {
        var ticketNum = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetByNum), new { ticketNum }, ticketNum);
    }

    [HttpPost("{ticketNum}/close")]
    public async Task<IActionResult> Close(decimal ticketNum, CancellationToken ct)
        => await mediator.Send(new CloseTicketCommand(ticketNum), ct) ? NoContent() : NotFound();
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ActionsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ComplaintActionDto>>> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetAllActionsQuery(), ct));

    [HttpGet("{actionNum}")]
    public async Task<ActionResult<ComplaintActionDto>> GetByNum(decimal actionNum, CancellationToken ct)
    {
        var result = await mediator.Send(new GetActionByNumQuery(actionNum), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("task/{taskNum}")]
    public async Task<ActionResult<ComplaintActionDto>> GetByTaskNum(decimal taskNum, CancellationToken ct)
    {
        var result = await mediator.Send(new GetActionByTaskNumQuery(taskNum), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<decimal>> Create([FromBody] CreateActionCommand command, CancellationToken ct)
    {
        var actionNum = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetByNum), new { actionNum }, actionNum);
    }

    [HttpPut("{actionNum}/primary")]
    public async Task<IActionResult> UpdatePrimary(decimal actionNum, [FromBody] UpdatePrimaryActionCommand command, CancellationToken ct)
    {
        if (actionNum != command.ActionNum) return BadRequest();
        return await mediator.Send(command, ct) ? NoContent() : NotFound();
    }

    [HttpPut("{actionNum}/secondary")]
    public async Task<IActionResult> UpdateSecondary(decimal actionNum, [FromBody] UpdateSecondaryActionCommand command, CancellationToken ct)
    {
        if (actionNum != command.ActionNum) return BadRequest();
        return await mediator.Send(command, ct) ? NoContent() : NotFound();
    }

    [HttpPut("{actionNum}/forward")]
    public async Task<IActionResult> UpdateForward(decimal actionNum, [FromBody] UpdateForwardActionCommand command, CancellationToken ct)
    {
        if (actionNum != command.ActionNum) return BadRequest();
        return await mediator.Send(command, ct) ? NoContent() : NotFound();
    }

    [HttpPut("{actionNum}/corrective")]
    public async Task<IActionResult> UpdateCorrective(decimal actionNum, [FromBody] UpdateCorrectiveActionCommand command, CancellationToken ct)
    {
        if (actionNum != command.ActionNum) return BadRequest();
        return await mediator.Send(command, ct) ? NoContent() : NotFound();
    }

    [HttpPost("{actionNum}/close")]
    public async Task<IActionResult> Close(decimal actionNum, CancellationToken ct)
        => await mediator.Send(new CloseActionCommand(actionNum), ct) ? NoContent() : NotFound();

    [HttpPost("{actionNum}/reopen")]
    public async Task<IActionResult> Reopen(decimal actionNum, [FromBody] ReopenActionCommand command, CancellationToken ct)
    {
        if (actionNum != command.ActionNum) return BadRequest();
        return await mediator.Send(command, ct) ? NoContent() : NotFound();
    }

    [HttpGet("{actionNum}/history")]
    public async Task<ActionResult<IEnumerable<ComplaintHistoryDto>>> GetHistory(decimal actionNum, CancellationToken ct)
        => Ok(await mediator.Send(new GetHistoryByActionNumQuery(actionNum), ct));
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EscalationsController(IMediator mediator) : ControllerBase
{
    [HttpGet("ticket/{ticketNum}")]
    public async Task<ActionResult<IEnumerable<ComplaintEscalationDto>>> GetByTicket(decimal ticketNum, CancellationToken ct)
        => Ok(await mediator.Send(new GetEscalationsByTicketNumQuery(ticketNum), ct));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEscalationCommand command, CancellationToken ct)
        => await mediator.Send(command, ct) ? Created() : BadRequest();

    [HttpPost("{ticketNum}/{levelNum}/close")]
    public async Task<IActionResult> Close(decimal ticketNum, decimal levelNum, [FromQuery] decimal updatedBy, CancellationToken ct)
        => await mediator.Send(new CloseEscalationCommand(ticketNum, levelNum, updatedBy), ct) ? NoContent() : NotFound();
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ComplaintFilesController(Application.Interfaces.IBlobStorageService blobService) : ControllerBase
{
    private const string ContainerName = "complaint-files";

    [HttpPost("{ticketNum}")]
    public async Task<IActionResult> Upload(decimal ticketNum, IFormFile file, CancellationToken ct)
    {
        if (file.Length == 0) return BadRequest("File is empty");

        var blobName = $"{ticketNum}/{Guid.NewGuid()}{System.IO.Path.GetExtension(file.FileName)}";
        using var stream = file.OpenReadStream();
        var url = await blobService.UploadAsync(ContainerName, blobName, stream, file.ContentType, ct);
        return Ok(new { Url = url, BlobName = blobName });
    }

    [HttpGet("{*blobName}")]
    public async Task<IActionResult> Download(string blobName, CancellationToken ct)
    {
        var stream = await blobService.DownloadAsync(ContainerName, blobName, ct);
        if (stream is null) return NotFound();
        return File(stream, "application/octet-stream", System.IO.Path.GetFileName(blobName));
    }

    [HttpDelete("{*blobName}")]
    public async Task<IActionResult> Delete(string blobName, CancellationToken ct)
        => await blobService.DeleteAsync(ContainerName, blobName, ct) ? NoContent() : NotFound();
}
