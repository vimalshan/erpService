using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReferenceDataService.Application.Commands.CreatePathToSqlServer;
using ReferenceDataService.Application.Commands.DeletePathToSqlServer;
using ReferenceDataService.Application.Commands.UpdatePathToSqlServer;
using ReferenceDataService.Application.DTOs;
using ReferenceDataService.Application.Queries.GetAllPathToSqlServers;

namespace ReferenceDataService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PathToSqlServerController : ControllerBase
{
    private readonly IMediator _mediator;

    public PathToSqlServerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PathToSqlServerDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllPathToSqlServersQuery());
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PathToSqlServerDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreatePathToSqlServerCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAll), result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(PathToSqlServerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePathToSqlServerCommand command)
    {
        if (id != command.Id)
            return BadRequest("Route id does not match body id.");

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeletePathToSqlServerCommand(id));
        return result ? NoContent() : NotFound();
    }
}
