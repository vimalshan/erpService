using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileAppManagement.Application.Interfaces;

namespace MobileAppManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BlobStorageController(IBlobStorageService blobStorageService) : ControllerBase
{
    private const string ContainerName = "mobile-app-images";

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file, CancellationToken ct)
    {
        if (file.Length == 0)
            return BadRequest("File is empty.");

        var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
        if (!allowedTypes.Contains(file.ContentType))
            return BadRequest("Only image files (jpg, png, gif, webp) are allowed.");

        var blobName = $"{Guid.NewGuid()}{System.IO.Path.GetExtension(file.FileName)}";
        using var stream = file.OpenReadStream();
        var url = await blobStorageService.UploadAsync(ContainerName, blobName, stream, file.ContentType, ct);

        return Ok(new { blobName, url });
    }

    [HttpGet("{blobName}")]
    public async Task<IActionResult> Download(string blobName, CancellationToken ct)
    {
        var stream = await blobStorageService.DownloadAsync(ContainerName, blobName, ct);
        if (stream is null)
            return NotFound();

        return File(stream, "application/octet-stream", blobName);
    }

    [HttpDelete("{blobName}")]
    public async Task<IActionResult> Delete(string blobName, CancellationToken ct)
    {
        var deleted = await blobStorageService.DeleteAsync(ContainerName, blobName, ct);
        return deleted ? Ok(new { message = "Deleted." }) : NotFound();
    }
}
