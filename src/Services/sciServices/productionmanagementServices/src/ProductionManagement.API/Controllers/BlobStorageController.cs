using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionManagement.Application.Interfaces;

namespace ProductionManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BlobStorageController : ControllerBase
{
    private readonly IBlobStorageService _blobStorage;
    private const string ContainerName = "stationery-item-images";

    public BlobStorageController(IBlobStorageService blobStorage) => _blobStorage = blobStorage;

    [HttpPost("upload")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> Upload(IFormFile file, CancellationToken cancellationToken)
    {
        if (file.Length == 0)
            return BadRequest("File is empty.");

        var blobName = $"{Guid.NewGuid()}{System.IO.Path.GetExtension(file.FileName)}";
        using var stream = file.OpenReadStream();
        var url = await _blobStorage.UploadAsync(ContainerName, blobName, stream, file.ContentType, cancellationToken);

        return Ok(new { blobName, url });
    }

    [HttpGet("{blobName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Download(string blobName, CancellationToken cancellationToken)
    {
        var stream = await _blobStorage.DownloadAsync(ContainerName, blobName, cancellationToken);
        if (stream is null)
            return NotFound();

        return File(stream, "application/octet-stream", blobName);
    }

    [HttpDelete("{blobName}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(string blobName, CancellationToken cancellationToken)
    {
        await _blobStorage.DeleteAsync(ContainerName, blobName, cancellationToken);
        return NoContent();
    }
}
