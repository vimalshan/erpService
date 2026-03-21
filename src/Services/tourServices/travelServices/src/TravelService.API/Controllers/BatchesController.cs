using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelService.Application.Batches.Commands.CreateBatch;
using TravelService.Application.Batches.Queries.GetBatch;

namespace TravelService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class BatchesController : ControllerBase
{
    private readonly ISender _sender;

    public BatchesController(ISender sender) => _sender = sender;

    /// <summary>Get a batch by ID.</summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetBatchByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Create a new batch.</summary>
    [HttpPost]
    [Authorize(Roles = "Admin,Finance")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateBatchCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
}
