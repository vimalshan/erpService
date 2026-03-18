using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainingDevelopment.Application.DTOs;
using TrainingDevelopment.Application.Features.TrainingDetails.Commands.CompleteTrainingDetail;
using TrainingDevelopment.Application.Features.TrainingDetails.Commands.CreateTrainingDetail;
using TrainingDevelopment.Application.Features.TrainingDetails.Commands.DeleteTrainingDetail;
using TrainingDevelopment.Application.Features.TrainingDetails.Commands.DropTrainingDetail;
using TrainingDevelopment.Application.Features.TrainingDetails.Commands.UpdateTrainingDetail;
using TrainingDevelopment.Application.Features.TrainingDetails.Queries.GetTrainingDetail;
using TrainingDevelopment.Application.Features.TrainingDetails.Queries.GetTrainingDetailList;

namespace TrainingDevelopment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class TrainingDetailsController : ControllerBase
{
    private readonly ISender _sender;

    public TrainingDetailsController(ISender sender) => _sender = sender;

    /// <summary>Get all training details with optional filters.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TrainingDetailDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] decimal? employeeSysId,
        [FromQuery] decimal? financialYear,
        [FromQuery] string? status,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetTrainingDetailListQuery(employeeSysId, financialYear, status), cancellationToken);
        return Ok(result);
    }

    /// <summary>Get training detail by ID.</summary>
    [HttpGet("{id:decimal}")]
    [ProducesResponseType(typeof(TrainingDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(decimal id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetTrainingDetailQuery(id), cancellationToken);
        return Ok(result);
    }

    /// <summary>Create a new training detail.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(TrainingDetailDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateTrainingDetailCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Update an existing training detail.</summary>
    [HttpPut("{id:decimal}")]
    [ProducesResponseType(typeof(TrainingDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(decimal id, [FromBody] UpdateTrainingDetailCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest("ID mismatch.");
        var result = await _sender.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>Mark training as completed.</summary>
    [HttpPatch("{id:decimal}/complete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Complete(decimal id, [FromBody] CompleteTrainingDetailCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest("ID mismatch.");
        await _sender.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>Drop a training record.</summary>
    [HttpPatch("{id:decimal}/drop")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Drop(decimal id, [FromBody] string remarks, CancellationToken cancellationToken)
    {
        await _sender.Send(new DropTrainingDetailCommand(id, remarks), cancellationToken);
        return NoContent();
    }

    /// <summary>Delete a training detail.</summary>
    [HttpDelete("{id:decimal}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(decimal id, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteTrainingDetailCommand(id), cancellationToken);
        return NoContent();
    }
}
