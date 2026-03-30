using EmployeeTransactionsService.Application.Features.Employees.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTransactionsService.API.Controllers;

[ApiController]
[Route("api/stationery")]
[Authorize(Policy = "Writer")]
public sealed class StationeryController(IMediator mediator) : ControllerBase
{
    [HttpPost("images")]
    [RequestSizeLimit(10_000_000)]
    public async Task<IActionResult> UploadImage([FromForm] string itemReference, [FromForm] decimal uploadedBy, [FromForm] IFormFile file, CancellationToken cancellationToken)
    {
        await using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream, cancellationToken);

        var command = new RegisterStationeryImageCommand(itemReference, file.FileName, file.ContentType, memoryStream.ToArray(), uploadedBy);
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}