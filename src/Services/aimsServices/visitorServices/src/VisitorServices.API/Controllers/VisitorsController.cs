using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisitorServices.Application.Visitors.Commands.AddVisitorItem;
using VisitorServices.Application.Visitors.Commands.CheckoutVisitor;
using VisitorServices.Application.Visitors.Commands.RegisterVisitor;
using VisitorServices.Application.Visitors.Queries.GetActiveVisitors;
using VisitorServices.Application.Visitors.Queries.GetVisitorById;

namespace VisitorServices.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class VisitorsController(ISender sender) : ControllerBase
{
    /// <summary>Get all currently active (checked-in) visitors.</summary>
    [HttpGet("active")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActiveVisitors(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetActiveVisitorsQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>Get a visitor by ID.</summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetVisitorByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Register a new visitor (check-in).</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterVisitorCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.VisitorId }, result);
    }

    /// <summary>Check out a visitor.</summary>
    [HttpPost("{id:long}/checkout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Checkout(long id, [FromQuery] long checkedOutBy, CancellationToken cancellationToken)
    {
        await sender.Send(new CheckoutVisitorCommand(id, checkedOutBy), cancellationToken);
        return NoContent();
    }

    /// <summary>Add an item/article to a visitor record.</summary>
    [HttpPost("{id:long}/items")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddItem(long id, [FromBody] AddVisitorItemCommand command, CancellationToken cancellationToken)
    {
        var actualCommand = command with { VisitorId = id };
        var result = await sender.Send(actualCommand, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, result);
    }
}
