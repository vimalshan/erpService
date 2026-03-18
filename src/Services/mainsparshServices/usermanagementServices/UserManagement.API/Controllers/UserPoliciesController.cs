using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Features.UserPolicy.Commands.CreateUserPolicy;
using UserManagement.Application.Features.UserPolicy.Commands.DeleteUserPolicy;
using UserManagement.Application.Features.UserPolicy.Commands.UpdateUserPolicy;
using UserManagement.Application.Features.UserPolicy.Queries.GetAllUserPolicies;
using UserManagement.Application.Features.UserPolicy.Queries.GetUserPolicyById;

namespace UserManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class UserPoliciesController(IMediator mediator) : ControllerBase
{
    /// <summary>Get all user policies, optionally filtered by policy type.</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] string? policyType, CancellationToken ct)
        => Ok(await mediator.Send(new GetAllUserPoliciesQuery(policyType), ct));

    /// <summary>Get a specific user policy by ID.</summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetUserPolicyByIdQuery(id), ct);
        return Ok(result);
    }

    /// <summary>Create a new user policy.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateUserPolicyCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.PolicyId }, result);
    }

    /// <summary>Update an existing user policy.</summary>
    [HttpPut("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateUserPolicyCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command with { PolicyId = id }, ct);
        return Ok(result);
    }

    /// <summary>Deactivate (soft delete) a user policy.</summary>
    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(long id, [FromQuery] long deletedBy, CancellationToken ct)
    {
        await mediator.Send(new DeleteUserPolicyCommand(id, deletedBy), ct);
        return NoContent();
    }
}
