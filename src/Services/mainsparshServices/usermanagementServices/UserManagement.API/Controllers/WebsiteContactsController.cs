using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Features.WebsiteContact.Commands.CreateWebsiteContact;
using UserManagement.Application.Features.WebsiteContact.Commands.UpdateWebsiteContact;
using UserManagement.Application.Features.WebsiteContact.Queries;

namespace UserManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class WebsiteContactsController(IMediator mediator) : ControllerBase
{
    /// <summary>Get contact details by ID.</summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
        => Ok(await mediator.Send(new GetWebsiteContactByIdQuery(id), ct));

    /// <summary>Get all contacts for a user.</summary>
    [HttpGet("user/{userSysId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUser(long userSysId, CancellationToken ct)
        => Ok(await mediator.Send(new GetContactsByUserSysIdQuery(userSysId), ct));

    /// <summary>Create a new website contact entry.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateWebsiteContactCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.ContactId }, result);
    }

    /// <summary>Update website contact details.</summary>
    [HttpPut("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateWebsiteContactCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command with { ContactId = id }, ct);
        return Ok(result);
    }
}
