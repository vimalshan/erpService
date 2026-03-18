using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace EmailNotification.API.Controllers;

/// <summary>
/// Controller for managing email types
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class EmailTypesController : BaseApiController
{
    /// <summary>
    /// Initializes a new instance of the EmailTypesController class
    /// </summary>
    /// <param name="mediator">The MediatR mediator</param>
    public EmailTypesController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Gets all email types
    /// </summary>
    /// <returns>List of email types</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Application.Dtos.EmailTypeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<Application.Dtos.EmailTypeDto>>> GetAll()
    {
        var result = await Mediator.Send(new Application.Queries.GetAllEmailTypesQuery());
        return Ok(result);
    }

    /// <summary>
    /// Gets an email type by ID
    /// </summary>
    /// <param name="id">The email type ID</param>
    /// <returns>The email type</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Application.Dtos.EmailTypeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Application.Dtos.EmailTypeDto>> GetById(long id)
    {
        var result = await Mediator.Send(new Application.Queries.GetEmailTypeByIdQuery(id));
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Gets email types by type (Daily or Event)
    /// </summary>
    /// <param name="emailType">The email type (D or E)</param>
    /// <returns>List of email types matching the type</returns>
    [HttpGet("bytype/{emailType}")]
    [ProducesResponseType(typeof(IEnumerable<Application.Dtos.EmailTypeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<Application.Dtos.EmailTypeDto>>> GetByType(string emailType)
    {
        var result = await Mediator.Send(new Application.Queries.GetEmailTypesByTypeQuery(emailType));
        return Ok(result);
    }

    /// <summary>
    /// Creates a new email type
    /// </summary>
    /// <param name="command">The create command</param>
    /// <returns>The created email type ID</returns>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<long>> Create([FromBody] Application.Commands.CreateEmailTypeCommand command)
    {
        var result = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result }, result);
    }

    /// <summary>
    /// Updates an email type
    /// </summary>
    /// <param name="id">The email type ID</param>
    /// <param name="command">The update command</param>
    /// <returns>No content</returns>
    [HttpPut("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(long id, [FromBody] Application.Commands.UpdateEmailTypeCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID mismatch");

        await Mediator.Send(command);
        return NoContent();
    }
}
