using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtherService.Application.CQRS.Commands.CreateLogDdCatDevDetail;
using OtherService.Application.CQRS.Commands.DeleteLogDdCatDevDetail;
using OtherService.Application.CQRS.Commands.UpdateLogDdCatDevDetail;
using OtherService.Application.CQRS.Queries.GetAllLogDdCatDevDetails;
using OtherService.Application.CQRS.Queries.GetLogDdCatDevDetailByKey;
using OtherService.Application.CQRS.Queries.GetLogDdCatDevDetailsByReqNum;
using OtherService.Application.DTOs;

namespace OtherService.API.Controllers;

/// <summary>
/// REST API for LOG_DD_CAT_DEV_DETAIL – Category Development Detail Log.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class LogDdCatDevDetailController : ControllerBase
{
    private readonly IMediator _mediator;

    public LogDdCatDevDetailController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get all entries.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<LogDdCatDevDetailDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllLogDdCatDevDetailsQuery(), ct);
        return Ok(result);
    }

    /// <summary>Get a single entry by composite key.</summary>
    [HttpGet("{appId}/{appNum:decimal}")]
    [ProducesResponseType(typeof(LogDdCatDevDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByKey(string appId, decimal appNum, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetLogDdCatDevDetailByKeyQuery(appId, appNum), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Get entries by request number.</summary>
    [HttpGet("by-req/{reqNum:decimal}")]
    [ProducesResponseType(typeof(IEnumerable<LogDdCatDevDetailDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByReqNum(decimal reqNum, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetLogDdCatDevDetailsByReqNumQuery(reqNum), ct);
        return Ok(result);
    }

    /// <summary>Create a new entry.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(LogDdCatDevDetailDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateLogDdCatDevDetailDto dto,
        CancellationToken ct)
    {
        var command = new CreateLogDdCatDevDetailCommand(
            dto.ReqNum, dto.QtnNum, dto.AnsSrl,
            dto.AppId, dto.AppNum, dto.EntDat,
            dto.Desc, dto.Need);

        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetByKey),
            new { appId = result.AppId, appNum = result.AppNum },
            result);
    }

    /// <summary>Update an existing entry.</summary>
    [HttpPut("{appId}/{appNum:decimal}")]
    [ProducesResponseType(typeof(LogDdCatDevDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        string appId,
        decimal appNum,
        [FromBody] UpdateLogDdCatDevDetailDto dto,
        CancellationToken ct)
    {
        var command = new UpdateLogDdCatDevDetailCommand(
            appId, appNum,
            dto.ReqNum, dto.QtnNum, dto.AnsSrl,
            dto.EntDat, dto.Desc, dto.Need);

        var result = await _mediator.Send(command, ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Delete an entry.</summary>
    [HttpDelete("{appId}/{appNum:decimal}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        string appId, decimal appNum, CancellationToken ct)
    {
        var deleted = await _mediator.Send(
            new DeleteLogDdCatDevDetailCommand(appId, appNum), ct);
        return deleted ? NoContent() : NotFound();
    }
}
