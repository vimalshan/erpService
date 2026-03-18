using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace EmailNotification.API.Controllers;

/// <summary>
/// Controller for managing mail access (recipients)
/// </summary>
[ApiController]
[Route("api/emailtypes/{emailTypeId}/recipients")]
[Produces("application/json")]
public class MailAccessController : BaseApiController
{
    /// <summary>
    /// Initializes a new instance of the MailAccessController class
    /// </summary>
    /// <param name="mediator">The MediatR mediator</param>
    public MailAccessController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Gets recipients for an email type by organization and business unit
    /// </summary>
    /// <param name="emailTypeId">The email type ID</param>
    /// <param name="orgId">The organization ID</param>
    /// <param name="businessId">The business unit ID (optional)</param>
    /// <returns>List of recipients</returns>
    [HttpGet]
    [Route("byorg")]
    [ProducesResponseType(typeof(IEnumerable<Application.Dtos.MailAccessDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<Application.Dtos.MailAccessDto>>> GetByOrgAndBusiness(
        long emailTypeId,
        [FromQuery] long orgId,
        [FromQuery] long? businessId = null)
    {
        var result = await Mediator.Send(
            new Application.Queries.GetRecipientsByOrgAndBusinessQuery(emailTypeId, orgId, businessId));
        return Ok(result);
    }

    /// <summary>
    /// Adds a recipient to an email type
    /// </summary>
    /// <param name="emailTypeId">The email type ID</param>
    /// <param name="command">The add recipient command</param>
    /// <returns>The created mail access ID</returns>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<long>> AddRecipient(long emailTypeId, [FromBody] Application.Commands.AddRecipientCommand command)
    {
        if (emailTypeId != command.EmailTypeId)
            return BadRequest("Email type ID mismatch");

        var result = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetByOrgAndBusiness), new { emailTypeId }, result);
    }

    /// <summary>
    /// Removes a recipient from an email type
    /// </summary>
    /// <param name="emailTypeId">The email type ID</param>
    /// <param name="mailAccessId">The mail access ID to remove</param>
    /// <param name="command">The remove recipient command</param>
    /// <returns>No content</returns>
    [HttpDelete("{mailAccessId}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveRecipient(long emailTypeId, long mailAccessId, [FromBody] Application.Commands.RemoveRecipientCommand command)
    {
        if (mailAccessId != command.MailAccessId)
            return BadRequest("Mail access ID mismatch");

        await Mediator.Send(command);
        return NoContent();
    }
}
