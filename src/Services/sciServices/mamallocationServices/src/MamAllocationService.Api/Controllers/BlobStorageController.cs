using MamAllocationService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MamAllocationService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BlobStorageController(IBlobStorageService blobService) : ControllerBase
{
    private const string ContainerName = "stationery-images";

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file, CancellationToken ct)
    {
        if (file.Length == 0) return BadRequest("File is empty");

        var blobName = $"{Guid.NewGuid()}{System.IO.Path.GetExtension(file.FileName)}";
        using var stream = file.OpenReadStream();
        var uri = await blobService.UploadAsync(ContainerName, blobName, stream, file.ContentType, ct);
        return Ok(new { uri, blobName });
    }

    [HttpGet("download/{blobName}")]
    public async Task<IActionResult> Download(string blobName, CancellationToken ct)
    {
        var stream = await blobService.DownloadAsync(ContainerName, blobName, ct);
        if (stream is null) return NotFound();
        return File(stream, "application/octet-stream", blobName);
    }

    [HttpDelete("{blobName}")]
    public async Task<IActionResult> Delete(string blobName, CancellationToken ct)
    {
        var deleted = await blobService.DeleteAsync(ContainerName, blobName, ct);
        return deleted ? NoContent() : NotFound();
    }
}
