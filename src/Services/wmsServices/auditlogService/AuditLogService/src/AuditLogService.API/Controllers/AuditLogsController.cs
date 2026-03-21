using AuditLogService.Application.Commands;
using AuditLogService.Application.DTOs;
using AuditLogService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditLogService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AuditLogsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuditLogsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AuditLogDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllAuditLogsQuery());
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<AuditLogDto>> GetById(long id)
    {
        var result = await _mediator.Send(new GetAuditLogByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("table/{tableName}")]
    public async Task<ActionResult<IReadOnlyList<AuditLogDto>>> GetByTable(string tableName)
    {
        var result = await _mediator.Send(new GetAuditLogsByTableQuery(tableName));
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<AuditLogDto>> Create([FromBody] CreateAuditLogDto dto)
    {
        var command = new CreateAuditLogCommand(
            dto.TableName,
            dto.RecordId,
            dto.Action,
            dto.ChangedBy,
            dto.OldValues,
            dto.NewValues);

        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.LogId }, result);
    }
}
