using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelRequestService.Domain.Interfaces;

namespace TravelRequestService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BlobStorageController : ControllerBase
{
    private readonly IBlobStorageService _blobStorage;

    public BlobStorageController(IBlobStorageService blobStorage)
    {
        _blobStorage = blobStorage;
    }

    [HttpPost("upload")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> Upload(IFormFile file, [FromQuery] string container = "travel-documents", CancellationToken cancellationToken = default)
    {
        if (file.Length == 0)
            return BadRequest("File is empty.");

        var fileName = $"{Guid.NewGuid()}{System.IO.Path.GetExtension(file.FileName)}";
        using var stream = file.OpenReadStream();
        var url = await _blobStorage.UploadFileAsync(container, fileName, stream, file.ContentType, cancellationToken);

        return Ok(new { Url = url, FileName = fileName });
    }

    [HttpGet("download")]
    public async Task<IActionResult> Download([FromQuery] string container, [FromQuery] string fileName, CancellationToken cancellationToken)
    {
        var stream = await _blobStorage.DownloadFileAsync(container, fileName, cancellationToken);
        if (stream is null)
            return NotFound();

        return File(stream, "application/octet-stream", fileName);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] string container, [FromQuery] string fileName, CancellationToken cancellationToken)
    {
        var result = await _blobStorage.DeleteFileAsync(container, fileName, cancellationToken);
        return result ? Ok() : NotFound();
    }
}
