using ExpenseService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BlobController : ControllerBase
{
    private readonly IBlobStorageService _blobService;

    public BlobController(IBlobStorageService blobService)
    {
        _blobService = blobService;
    }

    /// <summary>
    /// Upload a stationery item image
    /// </summary>
    [HttpPost("upload")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is required.");

        var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
        if (!allowedTypes.Contains(file.ContentType))
            return BadRequest("Only image files are allowed.");

        var blobName = $"{Guid.NewGuid()}{System.IO.Path.GetExtension(file.FileName)}";

        using var stream = file.OpenReadStream();
        var url = await _blobService.UploadAsync("stationery-images", blobName, stream, file.ContentType);

        return Ok(new { Url = url, BlobName = blobName });
    }

    /// <summary>
    /// Download a stationery item image
    /// </summary>
    [HttpGet("download/{blobName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Download(string blobName)
    {
        var stream = await _blobService.DownloadAsync("stationery-images", blobName);
        if (stream == null)
            return NotFound();

        return File(stream, "application/octet-stream", blobName);
    }

    /// <summary>
    /// Delete a stationery item image
    /// </summary>
    [HttpDelete("{blobName}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string blobName)
    {
        var deleted = await _blobService.DeleteAsync("stationery-images", blobName);
        return deleted ? NoContent() : NotFound();
    }
}
