using Microsoft.AspNetCore.Mvc;
using SettlementService.Domain.Interfaces;

namespace SettlementService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Microsoft.AspNetCore.Authorization.Authorize]
public class BlobStorageController : ControllerBase
{
    private readonly IBlobStorageService _blobStorageService;

    public BlobStorageController(IBlobStorageService blobStorageService)
    {
        _blobStorageService = blobStorageService;
    }

    [HttpPost("upload")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Upload(IFormFile file, [FromQuery] string container = "settlement-images", CancellationToken cancellationToken = default)
    {
        if (file.Length == 0)
            return BadRequest("File is empty.");

        var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
        if (!allowedTypes.Contains(file.ContentType))
            return BadRequest("Only image files (JPEG, PNG, GIF, WebP) are allowed.");

        var blobName = $"{Guid.NewGuid()}{System.IO.Path.GetExtension(file.FileName)}";
        using var stream = file.OpenReadStream();
        var url = await _blobStorageService.UploadAsync(container, blobName, stream, file.ContentType, cancellationToken);

        return Ok(new { Url = url, BlobName = blobName });
    }

    [HttpGet("download/{blobName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Download(string blobName, [FromQuery] string container = "settlement-images", CancellationToken cancellationToken = default)
    {
        var stream = await _blobStorageService.DownloadAsync(container, blobName, cancellationToken);
        if (stream is null)
            return NotFound();

        return File(stream, "application/octet-stream", blobName);
    }

    [HttpDelete("{blobName}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string blobName, [FromQuery] string container = "settlement-images", CancellationToken cancellationToken = default)
    {
        var deleted = await _blobStorageService.DeleteAsync(container, blobName, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
