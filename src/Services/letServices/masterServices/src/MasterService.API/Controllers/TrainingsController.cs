using MasterService.Application.Features.Trainings.Commands;
using MasterService.Application.Features.Trainings.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MasterService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TrainingsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetTrainingsQuery(), ct));

    [HttpGet("{trainingCode:long}")]
    public async Task<IActionResult> GetById(long trainingCode, CancellationToken ct)
    {
        var result = await mediator.Send(new GetTrainingByCodeQuery(trainingCode), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTrainingCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { trainingCode = result.TrainingCode }, result);
    }

    [HttpDelete("{trainingCode:long}")]
    public async Task<IActionResult> Cancel(long trainingCode, [FromQuery] string? cancelRemark, CancellationToken ct)
    {
        await mediator.Send(new CancelTrainingCommand(trainingCode, cancelRemark), ct);
        return NoContent();
    }

    [HttpPost("{trainingCode:long}/brochure")]
    public async Task<IActionResult> UploadBrochure(long trainingCode, IFormFile file, CancellationToken ct)
    {
        if (file is null || file.Length == 0) return BadRequest("No file uploaded.");
        var blobName = $"brochures/{trainingCode}/{file.FileName}";
        await mediator.Send(new UpdateTrainingBrochureCommand(trainingCode, blobName), ct);
        return Ok(new { BlobPath = blobName });
    }
}
