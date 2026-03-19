using FilingAndArchiveService.Application.DTOs;
using FilingAndArchiveService.Application.Files.Commands.CreateFile;
using FilingAndArchiveService.Application.Files.Commands.DeleteFile;
using FilingAndArchiveService.Application.Files.Commands.DispatchFile;
using FilingAndArchiveService.Application.Files.Commands.UpdateFile;
using FilingAndArchiveService.Application.Files.Queries.GetAllFiles;
using FilingAndArchiveService.Application.Files.Queries.GetFileById;
using FilingAndArchiveService.Application.Files.Queries.GetFilesByOrg;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FilingAndArchiveService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FilesController : ControllerBase
{
    private readonly IMediator _mediator;

    public FilesController(IMediator mediator) => _mediator = mediator;

    /// <summary>Gets all files (paginated).</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FileMasterDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetAllFilesQuery(page, pageSize), ct);
        return Ok(result);
    }

    /// <summary>Gets a file by ID.</summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(FileMasterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetFileByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Gets files by organization.</summary>
    [HttpGet("org/{orgId}")]
    [ProducesResponseType(typeof(IEnumerable<FileMasterDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByOrg(string orgId, [FromQuery] long? year, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetFilesByOrgQuery(orgId, year), ct);
        return Ok(result);
    }

    /// <summary>Creates a new file.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(FileMasterDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateFileCommand command, CancellationToken ct = default)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.FileId }, result);
    }

    /// <summary>Updates a file's details.</summary>
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(FileMasterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateFileRequest request, CancellationToken ct = default)
    {
        var command = new UpdateFileCommand(id, request.Remarks, request.PodNo, request.CourierName, request.UpdatedBy);
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }

    /// <summary>Dispatches a file.</summary>
    [HttpPost("{id:long}/dispatch")]
    [ProducesResponseType(typeof(FileMasterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Dispatch(long id, [FromBody] DispatchFileRequest request, CancellationToken ct = default)
    {
        var command = new DispatchFileCommand(id, request.PodNo, request.CourierName, request.DispatchedBy);
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }

    /// <summary>Deletes a file.</summary>
    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(long id, [FromQuery] long deletedBy, CancellationToken ct = default)
    {
        await _mediator.Send(new DeleteFileCommand(id, deletedBy), ct);
        return NoContent();
    }
}

public record UpdateFileRequest(string? Remarks, string? PodNo, string? CourierName, long UpdatedBy);
public record DispatchFileRequest(string PodNo, string CourierName, long DispatchedBy);
