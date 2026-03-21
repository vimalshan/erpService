using ConfigService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConfigService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BlobStorageController(IBlobStorageService blobService) : ControllerBase
{
    private const string ContainerName = "stationery-images";

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file, CancellationToken ct)
    {
        if (file.Length == 0) return BadRequest("No file uploaded.");

        var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
        if (!allowedTypes.Contains(file.ContentType))
            return BadRequest("Only image files (JPEG, PNG, GIF, WebP) are allowed.");

        var fileName = $"{Guid.NewGuid()}{System.IO.Path.GetExtension(file.FileName)}";
        using var stream = file.OpenReadStream();
        var url = await blobService.UploadAsync(ContainerName, fileName, stream, file.ContentType, ct);
        return Ok(new { FileName = fileName, Url = url });
    }

    [HttpGet("{fileName}")]
    public async Task<IActionResult> Download(string fileName, CancellationToken ct)
    {
        var stream = await blobService.DownloadAsync(ContainerName, fileName, ct);
        if (stream is null) return NotFound();
        return File(stream, "application/octet-stream", fileName);
    }

    [HttpDelete("{fileName}")]
    public async Task<IActionResult> Delete(string fileName, CancellationToken ct)
    {
        var result = await blobService.DeleteAsync(ContainerName, fileName, ct);
        return result ? NoContent() : NotFound();
    }
}
