using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TdsService.Application.DTOs;
using TdsService.Application.Files.Commands.UpdateEmailStatus;
using TdsService.Application.Files.Commands.UploadTdsFile;
using TdsService.Application.Files.Queries.GetAllTdsFiles;
using TdsService.Application.Files.Queries.GetTdsFileById;

namespace TdsService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public sealed class FilesController : ControllerBase
{
    private readonly IMediator _mediator;

    public FilesController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get paged list of all TDS files.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<TdsFileDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetAllTdsFilesQuery(page, pageSize), ct);
        return Ok(result);
    }

    /// <summary>Get a TDS file by ID.</summary>
    [HttpGet("{fileId:long}")]
    [ProducesResponseType(typeof(TdsFileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long fileId, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetTdsFileByIdQuery(fileId), ct);
        return result is not null ? Ok(result) : NotFound();
    }

    /// <summary>Upload a new TDS file (with optional binary content).</summary>
    [HttpPost]
    [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload(
        [FromForm] long fileId,
        [FromForm] string fileName,
        [FromForm] string? panNo,
        [FromForm] string? emailStatus,
        [FromForm] string? fileType,
        IFormFile? file,
        CancellationToken ct = default)
    {
        Stream? contentStream = null;
        string? contentType = null;

        if (file is not null)
        {
            contentStream = file.OpenReadStream();
            contentType = file.ContentType;
        }

        var command = new UploadTdsFileCommand(
            fileId, fileName, panNo, emailStatus, fileType,
            contentStream, contentType);

        var id = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { fileId = id }, id);
    }

    /// <summary>Mark email as sent for a TDS file.</summary>
    [HttpPatch("{fileId:long}/email-sent")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> MarkEmailSent(long fileId, CancellationToken ct = default)
    {
        await _mediator.Send(new UpdateEmailStatusCommand(fileId), ct);
        return NoContent();
    }
}
